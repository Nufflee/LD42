using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tile
{
  public Vector2Int position;
  public GameObject gameObject;
  public List<Solider> soliders = new List<Solider>();
  public List<Enemy> enemies = new List<Enemy>();
  public float soliderProgress;
  public float enemyProgress;
  public bool isBlue;
}

public class Territory : MonoBehaviour
{
  public static Territory Instance { get; private set; }

  public static bool isPlayerConquering;
  public static bool isEnemyConquering;

  private GameObject redHexagonPrefab;
  private GameObject blueHexagonPrefab;
  private GameObject whiteHexagonPrefab;
  private List<Tile> tiles = new List<Tile>();
  private Tile lerpToBlue;
  private Tile lerpToRed;
  private Slider playerConquerProgress;
  private Image playerConquerProgressImage;
  private Tile playerConquering;
  private Color red = new Color(255 / 255, 1 / 255, 5 / 255);
  private Color blue = new Color(0 / 255, 49 / 255, 255 / 255);
  private int size = 6;

  private void Start()
  {
    Instance = this;

    redHexagonPrefab = Resources.Load<GameObject>("Prefabs/RedHexagon");
    blueHexagonPrefab = Resources.Load<GameObject>("Prefabs/BlueHexagon");
    whiteHexagonPrefab = Resources.Load<GameObject>("Prefabs/WhiteHexagon");
    playerConquerProgress = GameObject.Find("UICanvas").transform.Find("PlayerConquerProgress").GetComponent<Slider>();
    playerConquerProgressImage = GameObject.Find("UICanvas").transform.Find("PlayerConquerProgress/FillArea/Fill").GetComponent<Image>();

    float outerRadius = 4.45f;

    for (int x = -size; x < size; x++)
    {
      for (int z = -size; z < size; z++)
      {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (outerRadius * (float) Math.Sqrt(3) / 2 * 2f);
        position.y = -1.0f;
        position.z = z * (outerRadius * 1.5f);

        GameObject tile = Instantiate(whiteHexagonPrefab, position, whiteHexagonPrefab.transform.rotation, transform);
        tile.name = "MinimapTile";

        tiles.Add(new Tile
        {
          gameObject = tile,
          position = new Vector2Int(x, z)
        });
      }
    }

    SetTile(Vector2Int.zero, false);

    GetTileAtPosition(Vector2Int.zero).enemyProgress = 100;
  }

  private void Update()
  {
    foreach (Tile tile in tiles)
    {
      tile.soliders.Clear();
      tile.enemies.Clear();
    }

    if (Random.Range(0.0f, 1.0f) < 0.1f)
    {
    }

    foreach (Solider solider in SoliderController.soliders)
    {
      RaycastHit hit;
      Tile tile = null;

      if (Physics.Raycast(solider.transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
      {
        tile = GetTileAtWorldPosition(hit.collider.transform.position);
        tile.soliders.Add(solider);
      }

      if (!Input.GetKey(KeyCode.Space))
      {
        isPlayerConquering = false;
        playerConquerProgress.gameObject.SetActive(false);

        return;
      }

      if (tile == null)
      {
        return;
      }

      if (playerConquering != null)
      {
        if (playerConquering.soliders.Count < 3)
        {
          playerConquerProgress.gameObject.SetActive(false);

          playerConquering = null;
        }
        else
        {
          playerConquering = tile;
        }
      }

      if (tile.soliders.Count > 3)
      {
        playerConquering = tile;

        playerConquerProgress.gameObject.SetActive(true);

        if (!tile.isBlue)
        {
          isPlayerConquering = true;
        }
        else
        {
          playerConquerProgress.value = 100;
          isPlayerConquering = false;

          return;
        }

        if (tile.enemyProgress > 0)
        {
          tile.enemyProgress -= 8.0f * Time.deltaTime;

          playerConquerProgressImage.color = red;
          playerConquerProgress.value = tile.enemyProgress;

          return;
        }

        tile.soliderProgress += 20.0f * Time.deltaTime;

        playerConquerProgressImage.color = blue;
        playerConquerProgress.value = tile.soliderProgress;

        if (tile.soliderProgress >= 100)
        {
          SetTile(tile.position, true, tile);
        }
      }
    }

    foreach (Enemy enemy in EnemyController.enemies)
    {
      RaycastHit hit;

      if (Physics.Raycast(enemy.transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
      {
        GetTileAtWorldPosition(hit.collider.transform.position).enemies.Add(enemy);
      }
    }
  }

  public Tile GetTileAtWorldPosition(Vector3 position)
  {
    return tiles.FirstOrDefault((tile) => tile.gameObject.transform.position.x == position.x && tile.gameObject.transform.position.z == position.z);
  }

  public Tile GetTileAtPosition(Vector2Int position)
  {
    return tiles.FirstOrDefault((tile) => tile.position == position);
  }

  public void SetTile(Vector2Int position, bool blue, Tile inherit = null)
  {
    Tile tile = GetTileAtPosition(position);
    GameObject newTileGO;
    int index = tiles.IndexOf(tile);
    Vector2Int oldPosition = tile.position;

    if (blue)
    {
      Destroy(tile.gameObject);

      newTileGO = Instantiate(blueHexagonPrefab, tile.gameObject.transform.position, blueHexagonPrefab.transform.rotation, transform);
    }
    else
    {
      Destroy(tile.gameObject);

      newTileGO = Instantiate(redHexagonPrefab, tile.gameObject.transform.position, redHexagonPrefab.transform.rotation, transform);
    }

    if (inherit == null)
    {
      inherit = new Tile();
    }

    Tile newTile = inherit;

    newTile.isBlue = blue;
    newTile.gameObject = newTileGO;

    tiles[index] = newTile;

    newTileGO.name = "WorldTile";
  }
}