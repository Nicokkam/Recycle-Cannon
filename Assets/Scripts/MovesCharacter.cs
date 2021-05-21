using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesCharacter : MonoBehaviour
{
  public Vector3 Direction { get; protected set; }
  private Rigidbody myRigidbody;

  void Awake()
  {
    myRigidbody = GetComponent<Rigidbody>();
  }

  public void SetDirection(Vector2 Direction)
  {
    this.Direction = new Vector3(Direction.x, 0, Direction.y);
  }

  public void SetDirection(Vector3 Direction)
  {
    this.Direction = Direction.normalized;
  }
  
  public void Move(float speed)
  {
    myRigidbody.MovePosition(
            myRigidbody.position +
            Direction * speed * Time.deltaTime);

  }

  public void Rotate(Vector3 Direction)
  {
    if (Direction != Vector3.zero)
    {
      Quaternion newRotation = Quaternion.LookRotation(Direction);
      myRigidbody.MoveRotation(newRotation);
    }
  }

  public void Die()
  {
    myRigidbody.constraints = RigidbodyConstraints.None;
    myRigidbody.velocity = Vector3.zero;
    myRigidbody.isKinematic = false;
    GetComponent<Collider>().enabled = false;
    Destroy(gameObject); ////

    if (gameObject.tag == "OrganicEnemy" || gameObject.tag == "MetalEnemy" || gameObject.tag == "PlasticEnemy")
    {
      GeneralInformation.roundKills += 1; ///
      GeneralInformation.totalKills += 1; ///
    }

    if (transform.tag == "Boss")
    {
      GeneralInformation.bossKilled += 1; ///
    }
  }

  public void Restart()
  {
    myRigidbody.isKinematic = true;
    GetComponent<Collider>().enabled = true;
  }
}