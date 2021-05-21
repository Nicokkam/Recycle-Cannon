using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickingInteractions : MonoBehaviour
{
  private int screenWidth;
  private Vector2 touchPos;

  [Header("CannonBall")]
  public GameObject cannonBallPrefab;
  public Transform cannonShootingPos;
  private CannonBallHandler CannonBallHandlercs;

  private Touch touch;
  public Vector3 lastPos;
  public int touchID;
  public bool isOverUI;
  public bool firstTouchOverUI;

  private GameObject player;

  void Start()
  {
    screenWidth = Screen.width;

    player = GameObject.FindWithTag("Player");
    CannonBallHandlercs = GetComponent<CannonBallHandler>();
  }

  private void Update()
  {
    if (Input.touchCount > 0)
    {
      if (Input.GetTouch(0).phase == TouchPhase.Began)
        isOverUI = IsPointerOverUIObject();

      if (Input.GetTouch(0).phase == TouchPhase.Ended)
      {
        touchPos = Input.GetTouch(0).position;
        if (isOverUI == false)
        {
          if (touchPos.x < screenWidth / 2)
          {
            if (CannonBallHandlercs != null)
              CannonBallHandlercs.HandleCannonBall();
          }
          else
            player.GetComponent<PlayerController>().HandleGarbage();
        }
      }
    }
  }

  private bool IsPointerOverUIObject()
  {
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    return results.Count > 0;
  }
}
