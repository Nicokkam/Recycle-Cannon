using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{
  [HideInInspector]
  public GameObject player;
  [HideInInspector]
  public GameObject wall;

  private Vector3 direction;
  private Stats enemyStats;
  private MovesCharacter moveEnemy;
  private bool wallCanTakeDamage = true;
  private bool playerCanTakeDamage = true;

  [Header("Health")]
  public Slider enemySlider;

  public float distanceToDealDamage; // Editor!
  private float distanceToWall; // Editor!
  private Vector3 whereToStrike;

  private void Awake()
  {
    moveEnemy = GetComponent<MovesCharacter>();
  }

  void Start()
  {
    player = GameObject.FindWithTag("Player");
    wall = GameObject.FindWithTag("Wall");

    enemyStats = GetComponent<Stats>();
    enemySlider.maxValue = enemyStats.health;
    UpdateEnemyHpSlider();

    distanceToDealDamage += transform.GetComponent<CapsuleCollider>().bounds.extents.x;
  }

  void FixedUpdate()
  {
    float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    GetPositionToStrike();

    direction.y = 0;
    moveEnemy.Rotate(direction);

    if (distanceToPlayer <= 3 && distanceToPlayer > distanceToDealDamage) //Go to player
    {
      direction = player.transform.position - transform.position;
      moveEnemy.SetDirection(direction);
      moveEnemy.Move(enemyStats.speed);
    }
    else if (distanceToPlayer > 3)
    {
      if (distanceToWall > distanceToDealDamage) //Go to Wall
      {
        direction = whereToStrike - transform.position;
        moveEnemy.SetDirection(direction);
        moveEnemy.Move(enemyStats.speed);
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
    else if (distanceToPlayer <= distanceToDealDamage) //Damage Player
    {
      if (playerCanTakeDamage == true)
      {
        direction = player.transform.position - transform.position;
        StartCoroutine(AttacksPlayer());
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

  public IEnumerator AttacksPlayer()
  {
    playerCanTakeDamage = false;
    int damage = 1;

    yield return new WaitForSecondsRealtime(0.5f);

    float _distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
    if (_distanceToPlayer <= distanceToDealDamage)
      player.GetComponent<PlayerController>().TakeDamage(damage, transform.eulerAngles);

    playerCanTakeDamage = true;
  }

  public IEnumerator AttacksWall()
  {
    wallCanTakeDamage = false;
    int damage = 1;
    wall.GetComponent<WallHandler>().TakeDamage(damage);
    yield return new WaitForSecondsRealtime(1);
    wallCanTakeDamage = true;
  }

  public void TakeDamage(int damage)
  {
    enemyStats.health -= damage;
    UpdateEnemyHpSlider();
    if (enemyStats.health <= 0)
    {
      Die();
    }
  }

  private void UpdateEnemyHpSlider()
  {
    enemySlider.value = enemyStats.health;
  }

  public void Die()
  {
    moveEnemy.Die();
    this.enabled = false;
  }

}
