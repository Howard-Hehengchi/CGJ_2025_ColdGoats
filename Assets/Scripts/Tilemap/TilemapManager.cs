using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour, IHitable
{
    [SerializeField] List<TileBase> tileBaseList = new List<TileBase>();
    [SerializeField] List<TileType> tileTypeList = new List<TileType>();
    [SerializeField] List<UnitInfo> tilePrefabList = new List<UnitInfo>();

    private Dictionary<TileBase, TileType> tileTypeMap = new Dictionary<TileBase, TileType>();
    private Dictionary<TileBase, UnitInfo> tilePrefabMap = new Dictionary<TileBase, UnitInfo>();

    private Dictionary<Vector3Int, int> healthMap = new Dictionary<Vector3Int, int>();

    [SerializeField] Transform testPrefab;

    private Tilemap tilemap;

    private void Start()
    {
        for(int i = 0; i < tileBaseList.Count; i++)
        {
            tileTypeMap.Add(tileBaseList[i], tileTypeList[i]);
            tilePrefabMap.Add(tileBaseList[i], tilePrefabList[i]);
        }

        tilemap = GetComponent<Tilemap>();

        tilemap.CompressBounds();
        var bounds = tilemap.cellBounds;
        foreach (var cellPos in bounds.allPositionsWithin)
        {
            TileBase currentTile = tilemap.GetTile(cellPos);
            if(currentTile != null)
            {
                switch (tileTypeMap[currentTile])
                {
                    case TileType.Environment:
                        healthMap[cellPos] = 5;
                        break;
                    case TileType.Block:
                        healthMap[cellPos] = 10;
                        break;
                    case TileType.Enemy:
                        InstantiateTile(cellPos, tilePrefabMap[currentTile]);
                        break;
                    case TileType.Null:
                        // Do nothing for null tiles
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void InstantiateTile(Vector3Int cellPos, UnitInfo prefab)
    {
        var unitInfo = Instantiate(prefab, Cell2World(cellPos), Quaternion.identity);
        unitInfo.BackFlip();
        tilemap.SetTile(cellPos, null);
    }

    private Vector3 Cell2World(Vector3Int cellPos)
    {
        Vector3 position = tilemap.CellToWorld(cellPos) + tilemap.cellSize * 0.5f;
        position.z = 0f;
        return position;
    }

    public void OnHit(Vector2 position, int hitAmount = 1)
    {
        Vector3Int cellPos = tilemap.WorldToCell(position);
        if (healthMap.ContainsKey(cellPos))
        {
            healthMap[cellPos] -= hitAmount;
            if (healthMap[cellPos] <= 0)
            {
                TileBase tile = tilemap.GetTile(cellPos);
                if (tile != null)
                {
                    healthMap.Remove(cellPos);
                    InstantiateTile(cellPos, tilePrefabMap[tile]);
                }
            }
        }
    }

    public enum TileType
    {
        Null,
        Environment,
        Block,
        Enemy
    }
}
