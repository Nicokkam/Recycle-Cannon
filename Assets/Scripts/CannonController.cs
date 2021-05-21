using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;

public class CannonController : MonoBehaviour
{

  public LeanJoystick joystick;
  public int speed;

  void Update()
  {
    transform.Rotate(0, joystick.ScaledValue.x * speed, 0, Space.Self);
  }

  void LateUpdate()
  {
    Vector3 viewRot = transform.eulerAngles;
    viewRot.y = ClampAngle(viewRot.y, -90, 90);
    transform.eulerAngles = viewRot;
  }

  float ClampAngle(float angle, float from, float to)
  {
    if (angle < 0f) 
      angle = 360 + angle;
    if (angle > 180f) 
      return Mathf.Max(angle, 360 + from);
      
    return Mathf.Min(angle, to);
  }
}
