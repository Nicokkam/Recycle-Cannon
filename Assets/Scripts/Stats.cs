using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
  public int initHealth = 3;
  [HideInInspector]
  public int health;
  public float speed = 2;

  void Awake()
  {
    health = initHealth;
  }
}