using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
  Transform camTransform;
  Quaternion originalRotation;

  void Start()
  {
    camTransform = Camera.main.transform;
    originalRotation = transform.rotation;
  }

  void LateUpdate()
  {
    transform.rotation = camTransform.rotation * originalRotation;
  }
}
