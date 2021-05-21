using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHandler : MonoBehaviour
{
  [HideInInspector]
  public GameObject wall;

  private Vector3 direction;
  private Stats bossStats;
  private MovesCharacter moveBoss;
  private bool wallCanTakeDamage = true;

  [Header("Health")]
  public Slider bossSlider;

  public float distanceToDealDamage; // Editor!
  private float distanceToWall; // Editor!
  private Vector3 whereToStrike;

  private void Awake()
  {
    moveBoss = GetComponent<MovesCharacter>();
  }

  void Start()
  {
    wall = GameObject.FindWithTag("Wall");

    bossStats = GetComponent<Stats>();
    bossSlider.maxValue = bossStats.health;
    UpdateBossHpSlider();

    distanceToDealDamage += transform.GetComponent<CapsuleCollider>().bounds.extents.x;
  }

  void FixedUpdate()
  {
    GetPositionToStrike();

    direction.y = 0;
    moveBoss.Rotate(direction);

    if (distanceToWall > distanceToDealDamage) //Go to Wall
    {
      direction = whereToStrike - transform.position;
      moveBoss.SetDirection(direction);
      moveBoss.Move(bossStats.speed);
    }
    else if (distanceToWall <= distanceToDealDamage) //Damage Wall
    {
      if (wallCanTakeDamage == true)
      {
        direction = whereToStrike - transform.position;
        StartCoroutine(AttacksWall());
      }
    }
  }

  private void GetPositionToStrike()
  {
    float currentDistance;
    float previousDistance = Mathf.Infinity;
    Vector3 previousPosition = new Vector3();

    foreach (Transform item in wall.transform)
    {
      currentDistance = Vector3.Distance(transform.position, item.position);

      if (currentDistance < previousDistance)
      {
        previousDistance = currentDistance;
        previousPosition = item.position;
      }
    }

    whereToStrike = previousPosition;
    distanceToWall = previousDistance;
  }

  public IEnumerator AttacksWall()
  {
    wallCanTakeDamage = false;
    int bossDamage = 3;
    wall.GetComponent<WallHandler>().TakeDamage(bossDamage);
    yield return new WaitForSecondsRealtime(1);
    wallCanTakeDamage = true;
  }

  public void TakeDamage(int damage)
  {
    bossStats.health -= damage;
    UpdateBossHpSlider();
    if (bossStats.health <= 0)
    {
      Die();
    }
  }

  private void UpdateBossHpSlider()
  {
    bossSlider.value = bossStats.health;
  }

  public void Die()
  {
    moveBoss.Die();
    this.enabled = false;
  }

}
