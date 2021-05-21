using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
  [Header("Prefabs")]
  public GameObject enemyPrefab;
  public GameObject bossPrefab;

  [Header("Stats")]
  public float spawnDelay = 3;
  public int[] enemiesPerRound = { 1, 1, 1, 1, 1 };
  //   public int[] enemiesPerRound = { 5, 7, 9, 11, 15 };
  [Header("Read Only")]
  public int spawnsPerRound = 0;
  public int spawnsTotal = 0;
  public int spawnedBoss = 0;
  public int currentRound = 1;

  [Header("Enemy Materials")]
  public Material plasticMat;
  public Material organicMat;
  public Material metalMat;

  private float timeCounter = 0;
  private GameObject player;
  private GameObject wall;
  private GameObject gameManager;

  private void Start()
  {
    player = GameObject.FindWithTag("Player");
    wall = GameObject.FindWithTag("Wall");
    gameManager = GameObject.FindWithTag("GameManager");

    GeneralInformation.bossKilled = 0;
    GeneralInformation.roundKills = 0;
    GeneralInformation.totalKills = 0;
    Time.timeScale = 1;
  }

  void Update()
  {
    timeCounter += Time.deltaTime;

    if (spawnedBoss == 0) //Boss Spawned?
    {
      if (spawnsPerRound != enemiesPerRound[currentRound - 1]) //Se não criou o número certo de inimigos para o round
      {
        if (timeCounter >= spawnDelay)
        {
          GenerateNewEnemy(enemyPrefab);
          timeCounter = 0;
        }
      }
      else //Se ja criou todos os inimigos
      {
        if (GeneralInformation.roundKills == enemiesPerRound[currentRound - 1]) //Todos morreram
        {
          if (currentRound == enemiesPerRound.Length) //último round?
          {
            GenerateNewEnemy(bossPrefab);
          }
          else
          {
            spawnsPerRound = 0;
            GeneralInformation.roundKills = 0;
            currentRound++;
            spawnDelay *= 0.9f;
            int healingEachHorde = 10;
            wall.GetComponent<WallHandler>().Heal(healingEachHorde);
          }
        }
      }
    }
    else
    {
      if (GeneralInformation.bossKilled == 1)
      {
        Victory();
      }
    }
  }

  public void GenerateNewEnemy(GameObject _enemyPrefab)
  {
    Vector3 creationPosition = RandomizePosition();

    var newEnemy = Instantiate(_enemyPrefab, creationPosition, Quaternion.identity);
    var objectHeight = newEnemy.transform.GetComponent<CapsuleCollider>().bounds.extents.y;
    newEnemy.transform.position = newEnemy.transform.position + (Vector3.up * objectHeight);

    if (_enemyPrefab.tag == "Enemy")
    {
      spawnsPerRound += 1;
      spawnsTotal += 1;
      RandomizeEnemyType(newEnemy);
    }
    else if (_enemyPrefab.tag == "Boss")
    {
      spawnedBoss += 1;
    }
  }

  private void RandomizeEnemyType(GameObject newEnemy)
  {
    var tagRandomizer = Random.Range(0, 10);
    if (tagRandomizer <= 4)
    {
      newEnemy.tag = "OrganicEnemy"; //45%
      newEnemy.name = "Organic Enemy"; //45%
      newEnemy.GetComponent<MeshRenderer>().material = organicMat;
    }
    else if (tagRandomizer <= 7)
    {
      newEnemy.tag = "MetalEnemy"; //27%
      newEnemy.name = "Metal Enemy"; //45%
      newEnemy.GetComponent<MeshRenderer>().material = metalMat;
    }
    else if (tagRandomizer <= 10)
    {
      newEnemy.tag = "PlasticEnemy"; //27%
      newEnemy.name = "Plastic Enemy"; //45%
      newEnemy.GetComponent<MeshRenderer>().material = plasticMat;
    }
  }

  public void Victory()
  {
    gameManager.GetComponent<InterfaceHandler>().Victory();
  }

  private Vector3 RandomizePosition()
  {
    Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.y));
    Vector3 posicao = new Vector3();

    posicao.x = Random.Range(-screenBounds.x, screenBounds.x);
    posicao += transform.position;
    posicao.y = 0;

    return posicao;
  }

}