using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHandler : MonoBehaviour
{
  private Stats wallStats;
  private InterfaceHandler InterfaceHandlercs;

  private GameObject gameManager;

  void Start()
  {
    wallStats = GetComponent<Stats>();
    InterfaceHandlercs = GameObject.FindWithTag("GameManager").GetComponent<InterfaceHandler>();

    gameManager = GameObject.FindWithTag("GameManager");
  }

  public void TakeDamage(int _damage)
  {
    wallStats.health -= _damage;
    InterfaceHandlercs.UpdateWallSlider();
    if (wallStats.health <= 0)
    {
      GameOver();
    }
  }

  public void Heal(int _healing)
  {
    wallStats.health += _healing;
    InterfaceHandlercs.UpdateWallSlider();
  }

  public void GameOver()
  {
    gameManager.GetComponent<InterfaceHandler>().Defeat();
  }
}
