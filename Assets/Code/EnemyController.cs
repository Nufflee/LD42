using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public static bool shouldStop;

  private static Dictionary<Enemy, int> enemyIds = new Dictionary<Enemy, int>();
  private GameObject armyPrefab;

  private void Start()
  {
    armyPrefab = Resources.Load<GameObject>("Prefabs/EnemyArmy");

    Spawn();
  }

  private void Update()
  {
    shouldStop = Random.Range(0.0f, 1.0f) <= 0.15f;
  }

  private void Spawn()
  {
    Instantiate(armyPrefab, new Vector3(13, 0, 13), Quaternion.identity);
  }

  public static int GetId(Enemy enemy)
  {
    int id = 0;

    if (!enemyIds.TryGetValue(enemy, out id))
    {
      enemyIds.Add(enemy, id = enemyIds.Count);
    }

    return id;
  }
}