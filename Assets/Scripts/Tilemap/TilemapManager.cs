using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour, IHitable
{
    [SerializeField] TileDataList tileData;

    private Dictionary<TileBase, TileBaseData> tileDataMap = new Dictionary<TileBase, TileBaseData>();
    private Dictionary<Vector3Int, int> cellHitCountMap = new Dictionary<Vector3Int, int>();
    private Dictionary<Vector3Int, UnitInfo> cellPrefabMap = new Dictionary<Vector3Int, UnitInfo>();

    [SerializeField] Transform testPrefab;
    [SerializeField] TileBase gameEndTile;
    [SerializeField] Transform gameEndTilePrefab;

    private Tilemap tilemap;

    private void Start()
    {
        tileDataMap = tileData.GetTileDataMap();

        tilemap = GetComponent<Tilemap>();

        tilemap.CompressBounds();
        var bounds = tilemap.cellBounds;
        foreach (var cellPos in bounds.allPositionsWithin)
        {
            TileBase currentTile = tilemap.GetTile(cellPos);
            if (currentTile == null) continue;

            if(currentTile == gameEndTile)
            {
                Instantiate(gameEndTilePrefab, Cell2World(cellPos), Quaternion.identity);
                tilemap.SetTile(cellPos, null);
                continue;
            }

            TileBaseData currentTileData = tileDataMap[currentTile];

            tilemap.SetTile(cellPos, currentTileData.cardBackTile);

            switch (currentTileData.tileType)
            {
                case TileType.Environment:
                case TileType.Block:
                    cellHitCountMap[cellPos] = currentTileData.hitCount;
                    cellPrefabMap[cellPos] = currentTileData.tilePrefab;
                    break;
                case TileType.Enemy:
                    InstantiateTile(cellPos, currentTileData.tilePrefab);
                    break;
                case TileType.Null:
                    // Do nothing for null tiles
                    break;
                default:
                    break;
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
        if (cellHitCountMap.ContainsKey(cellPos))
        {
            cellHitCountMap[cellPos] -= hitAmount;
            if (cellHitCountMap[cellPos] <= 0)
            {
                cellHitCountMap.Remove(cellPos);
                InstantiateTile(cellPos, cellPrefabMap[cellPos]);
            }
        }
    }

    [System.Serializable]
    public struct TileBaseData
    {
        public TileBase tileBase;
        public TileType tileType;
        public UnitInfo tilePrefab;
        public TileBase cardBackTile;
        public int hitCount;

        public TileBaseData(TileBase tileBase, TileType tileType, UnitInfo tilePrefab, TileBase cardBackTile, int hitCount)
        {
            this.tileBase = tileBase;
            this.tileType = tileType;
            this.tilePrefab = tilePrefab;
            this.cardBackTile = cardBackTile;
            this.hitCount = hitCount;
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
