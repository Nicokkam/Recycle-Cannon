using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{

  [Header("Joystic")]
  private Rigidbody rigidBody;
  public LeanJoystick joystick;
  private float speed;

  [Header("Screen Bounds")]
  public Camera MainCamera;
  private Vector3 screenBounds;
  private float objectWidth;
  private float objectHeight;

  [Header("Health")]
  public GameObject lifeHLG;
  private Stats playerStats;

  [Header("Garbage")]
  public float minDistanceToGrab;
  private GameObject garbageCollectors;
  private GameObject scatteredGarbage;
  private bool isHoldingGarbage = false;
  private Transform pickedGarbage = null;
  private float pickedGarbageWidth = 0;

  private GameObject gameManager;
  private Vector3 playerRotationDir;
  private CannonBallHandler CannonBallHandlercs;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    playerStats = GetComponent<Stats>();
    speed = playerStats.speed;

    gameManager = GameObject.FindWithTag("GameManager");
    scatteredGarbage = GameObject.FindWithTag("ScatteredGarbage");
    garbageCollectors = GameObject.FindWithTag("GarbageCollectors");
    CannonBallHandlercs = GameObject.FindWithTag("GameManager").GetComponent<CannonBallHandler>();



    screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.y));
    objectWidth = transform.GetComponent<CapsuleCollider>().bounds.extents.x; //extents = size of width / 2

    minDistanceToGrab += objectWidth;
  }

  void Update()
  {
    rigidBody.velocity = new Vector3(joystick.ScaledValue.x * speed, rigidBody.velocity.y, joystick.ScaledValue.y * speed);

    playerRotationDir = new Vector3(joystick.ScaledValue.x, 0f, joystick.ScaledValue.y);
    if (playerRotationDir != Vector3.zero)
    {
      Quaternion targetRotation = Quaternion.LookRotation(playerRotationDir, Vector3.up);
      transform.rotation = targetRotation;
    }
  }

  void LateUpdate()
  {
    Vector3 viewPos = transform.position;
    viewPos.x = Mathf.Clamp(viewPos.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
    viewPos.z = Mathf.Clamp(viewPos.z, -screenBounds.z + objectWidth, screenBounds.z - objectWidth);
    transform.position = viewPos;
  }

  public void HandleGarbage()
  {
    float distanceOfGarbage;


    if (isHoldingGarbage == false)
    {
      foreach (Transform garbage in scatteredGarbage.transform)
      {
        distanceOfGarbage = Vector3.Distance(transform.position, garbage.position);
        float _itemWidth = garbage.GetComponent<SphereCollider>().bounds.extents.x;

        if (distanceOfGarbage <= minDistanceToGrab + _itemWidth)
        {
          pickedGarbage = garbage;
          pickedGarbageWidth = _itemWidth;
          PickUpGarbage(pickedGarbage, pickedGarbageWidth);
        }
      }
    }
    else if (isHoldingGarbage == true)
    {
      Transform dumpster = IsCloseToDumpster();

      if (dumpster != null)
      {
        if (IsCorrectDumpster(pickedGarbage, dumpster))
        {
          if (CannonBallHandlercs.isMagazineFull() == false)
            DropOnDumpster(dumpster);
          else
            Debug.Log("Magazine Full");
        }
      }
      else
        DropGarbage(pickedGarbage, pickedGarbageWidth);
    }
  }

  public void DropOnDumpster(Transform _dumpster)
  {
    Vector3 rotateTowards = _dumpster.position;
    rotateTowards.y = transform.position.y;
    transform.LookAt(rotateTowards, Vector3.up);

    Vector3 midPoint = (pickedGarbage.position + _dumpster.position) / 2f;
    midPoint.y = pickedGarbage.position.y;

    LeanTween.move(pickedGarbage.gameObject, midPoint + pickedGarbage.up, 0.2f)
      .setOnComplete(x => LeanTween.move(pickedGarbage.gameObject, _dumpster.position, 0.2f)
        .setOnComplete(x =>
        {
          string _garbageType = pickedGarbage.GetComponent<GarbageSpecs>().type;
          string _garbageSize = pickedGarbage.GetComponent<GarbageSpecs>().size;

          CannonBallHandlercs.AddToMagazine(_garbageType, _garbageSize);

          Destroy(pickedGarbage.gameObject);

          isHoldingGarbage = false;
          pickedGarbage = null;
          pickedGarbageWidth = 0;
        })
      );
  }

  public bool IsCorrectDumpster(Transform _garbage, Transform _dumpster)
  {
    GarbageSpecs garbageSpecs = _garbage.GetComponent<GarbageSpecs>();

    if (garbageSpecs.type == "organic")
      if (_dumpster.tag == "OrganicCollector")
        return true;

    if (garbageSpecs.type == "metal")
      if (_dumpster.tag == "MetalCollector")
        return true;

    if (garbageSpecs.type == "plastic")
      if (_dumpster.tag == "PlasticCollector")
        return true;

    return false;
  }

  public Transform IsCloseToDumpster()
  {
    float distanceOfDumpsters;

    foreach (Transform dumpster in garbageCollectors.transform)
    {
      distanceOfDumpsters = Vector3.Distance(transform.position, dumpster.position);
      float _itemWidth = dumpster.GetComponent<BoxCollider>().bounds.extents.x; //extents = size of width / 2

      if (distanceOfDumpsters <= minDistanceToGrab + _itemWidth)
        return dumpster;
    }

    return null;
  }

  public void PickUpGarbage(Transform _garbage, float _garbageWidth) //TODO
  {
    isHoldingGarbage = true;
    _garbage.GetComponent<SphereCollider>().enabled = false;
    _garbage.GetComponent<Rigidbody>().useGravity = false;

    Vector3 rotateTowards = _garbage.position;
    rotateTowards.y = transform.position.y;
    transform.LookAt(rotateTowards, Vector3.up);

    _garbage.position = transform.position + playerRotationDir.normalized + transform.forward * (objectWidth + _garbageWidth);
    _garbage.parent = transform;

    LeanTween.move(_garbage.gameObject, _garbage.position + _garbage.up, 0.2f)
      .setOnComplete(x => LeanTween.move(_garbage.gameObject, _garbage.position - _garbage.up, 0.2f));
  }

  public void DropGarbage(Transform _garbage, float _garbageWidth) //TODO
  {
    isHoldingGarbage = false;
    pickedGarbage = null;
    pickedGarbageWidth = 0;
    _garbage.GetComponent<SphereCollider>().enabled = true;
    _garbage.GetComponent<Rigidbody>().useGravity = true;

    _garbage.rotation = transform.rotation;
    LeanTween.move(_garbage.gameObject, _garbage.position + _garbage.forward + playerRotationDir.normalized, 0.2f);
    _garbage.transform.parent = scatteredGarbage.transform;
  }

  public void TakeDamage(int _damage, Vector3 _enemyRot)
  {
    playerStats.health -= _damage;

    transform.eulerAngles = _enemyRot;
    float initialYPos = transform.position.y;
    Vector3 pushBackPos = transform.position + transform.forward * speed;
    LeanTween.move(gameObject, pushBackPos, 0.2f)
      .setOnComplete(x => transform.position = new Vector3(transform.position.x, initialYPos, transform.position.z));

    if (playerStats.health == 2)
      lifeHLG.transform.Find("1").transform.GetComponent<Image>().enabled = false;

    if (playerStats.health == 1)
      lifeHLG.transform.Find("2").transform.GetComponent<Image>().enabled = false;

    if (playerStats.health <= 0)
    {
      lifeHLG.transform.Find("3").transform.GetComponent<Image>().enabled = false;
      GameOver();
    }
  }

  public void GameOver()
  {
    gameManager.GetComponent<InterfaceHandler>().Defeat();
  }

  // private void OnDrawGizmos()
  // {
  //   Gizmos.color = Color.blue;
  //   Gizmos.DrawSphere(transform.position, minDistanceToGrab);
  // }

}
