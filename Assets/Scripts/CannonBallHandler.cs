using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CannonBallHandler : MonoBehaviour
{
  public GameObject cannonBallPrefab;
  public Transform cannonShootingPos;
  public List<string> shotMagazine = new List<string>();

  public Material organicMaterial;
  public Material metalMaterial;
  public Material plasticMaterial;

  public void HandleCannonBall()
  {
    string updateColor;

    if (isMagazineEmpty() == false)
    {
      ShootCannonBall();
      transform.GetComponent<InterfaceHandler>().ShotMagazineUpdate(shotMagazine.Count);
      if (isMagazineEmpty())
        updateColor = "empty";
      else
        updateColor = shotMagazine.First();
    }
    else
    {
      Debug.Log("Magazine is empty!");
      updateColor = "empty";
    }

    transform.GetComponent<InterfaceHandler>().UpdateShotColor(updateColor);
  }

  private void ShootCannonBall()
  {
    GameObject clone = Instantiate(cannonBallPrefab, cannonShootingPos.position, cannonShootingPos.rotation);

    string tag = SetShotType(clone);
    if (tag != null)
    {
      clone.tag = tag;
    }

    shotMagazine.RemoveAt(0);
  }

  private string SetShotType(GameObject _shot)
  {
    string firstShot = shotMagazine.First();
    MeshRenderer shotRenderer = _shot.transform.GetComponent<MeshRenderer>();
    string _tag = null;

    if (firstShot == "organic")
    {
      shotRenderer.material = organicMaterial;
      _tag = "OrganicShot";
    }
    if (firstShot == "metal")
    {
      shotRenderer.material = metalMaterial;
      _tag = "MetalShot";
    }
    if (firstShot == "plastic")
    {
      shotRenderer.material = plasticMaterial;
      _tag = "PlasticShot";
    }

    return _tag;
  }

  public void AddToMagazine(string _type, string _size)
  {
    if (isMagazineFull() == false)
    {
      if (isMagazineEmpty())
        transform.GetComponent<InterfaceHandler>().UpdateShotColor(_type);

      if (_size == "small")
        shotMagazine.Add(_type);
      else if (_size == "big")
        LoadThreeTimes(_type);
      else
        Debug.LogError("Garbage Size in garbage specs is incorrect");

      transform.GetComponent<InterfaceHandler>().ShotMagazineUpdate(shotMagazine.Count);


    }
  }

  public void LoadThreeTimes(string _type)
  {
    for (int i = 0; i < 3; i++)
    {
      if (isMagazineFull() == true)
        return;

      shotMagazine.Add(_type);
    }
  }

  public bool isMagazineFull()
  {
    if (shotMagazine.Count == 5)
      return true;
    else
      return false;
  }

  public bool isMagazineEmpty()
  {
    if (shotMagazine.Count == 0)
      return true;
    else
      return false;
  }

}
