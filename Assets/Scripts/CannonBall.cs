using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
  public float speed = 10;
  private Rigidbody rigidbodyCannonBall;

  void Start()
  {
    rigidbodyCannonBall = transform.GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    rigidbodyCannonBall.MovePosition(rigidbodyCannonBall.position + transform.forward * speed * Time.deltaTime);
  }

  void OnTriggerEnter(Collider collidedObject)
  {
    Quaternion rotacaoOpostaABala = Quaternion.LookRotation(-transform.forward);

    switch (collidedObject.tag) //TODO Here is where we will decide if the zombie gets damaged according to its type and the shot type
    {
      case "Enemy":
        EnemyHandler enemy = collidedObject.GetComponent<EnemyHandler>();
        enemy.TakeDamage(1);
        Destroy(gameObject);
        break;

      case "OrganicEnemy":
        if (transform.tag == "MetalShot" || transform.tag == "PlasticShot")
        {
          EnemyHandler organicEnemy = collidedObject.GetComponent<EnemyHandler>();
          organicEnemy.TakeDamage(1);
          Destroy(gameObject);
        }
        break;

      case "MetalEnemy":
        if (transform.tag == "OrganicShot")
        {
          EnemyHandler MetalEnemy = collidedObject.GetComponent<EnemyHandler>();
          MetalEnemy.TakeDamage(1);
          Destroy(gameObject);
        }
        break;

      case "PlasticEnemy":
        if (transform.tag == "OrganicShot")
        {
          EnemyHandler PlasticEnemy = collidedObject.GetComponent<EnemyHandler>();
          PlasticEnemy.TakeDamage(1);
          Destroy(gameObject);
        }
        break;

      case "Boss":
        BossHandler boss = collidedObject.GetComponent<BossHandler>();
        boss.TakeDamage(1);
        Destroy(gameObject);
        break;
    }
  }

}
