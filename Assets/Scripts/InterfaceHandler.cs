using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InterfaceHandler : MonoBehaviour
{
  [Header("Wall")]
  public Slider wallSlider;
  private Stats wallStatus;

  [Header("Match Ended")]
  public GameObject matchEnded;
  public TextMeshProUGUI victoryOrDefeat;
  public TextMeshProUGUI enemiesDefeated;
  public TextMeshProUGUI matchDuration;
  public TextMeshProUGUI shotMagazine;
  public Image nextShotImg;

  private Color32 brown = new Color32(168, 103, 43, 255);
  private Color32 yellow = new Color32(192, 200, 52, 255);
  private Color32 red = new Color32(197, 44, 44, 255);
  private Color32 white = new Color32(255, 255, 255, 100);

  void Start()
  {
    wallStatus = GameObject.FindWithTag("Wall").GetComponent<Stats>();
    wallSlider.maxValue = wallStatus.health;
    UpdateWallSlider();
  }

  public void ShotMagazineUpdate(int _magazineSize)
  {
    shotMagazine.text = _magazineSize.ToString();
  }

  public void UpdateShotColor(string _shotColor)
  {
    if (_shotColor == "organic")
      nextShotImg.color = brown;
    if (_shotColor == "metal")
      nextShotImg.color = yellow;
    if (_shotColor == "plastic")
      nextShotImg.color = red;
    if (_shotColor == "empty")
      nextShotImg.color = white;
  }

  public void UpdateWallSlider()
  {
    wallSlider.value = wallStatus.health;
  }

  public void Victory()
  {
    matchEnded.SetActive(true);
    victoryOrDefeat.text = "Victory!";
    enemiesDefeated.text = GeneralInformation.totalKills.ToString();
    matchDuration.text = Mathf.Round(Time.timeSinceLevelLoad).ToString() + "s";
    Time.timeScale = 0;
  }

  public void Defeat()
  {
    matchEnded.SetActive(true);
    victoryOrDefeat.text = "Defeat";
    enemiesDefeated.text = GeneralInformation.totalKills.ToString();
    matchDuration.text = Mathf.Round(Time.timeSinceLevelLoad).ToString() + "s";
    Time.timeScale = 0;
  }

  public void ReturnToMenu()
  {
    SceneManager.LoadScene("Menu");
  }


}
