using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileDataList", menuName = "ScriptableObjects/TileDataList", order = 1)]
public class TileDataList : ScriptableObject
{
    public List<TilemapManager.TileBaseData> tileDataList = new List<TilemapManager.TileBaseData>();

    public Dictionary<TileBase, TilemapManager.TileBaseData> GetTileDataMap()
    {
        var tileTypeMap = new Dictionary<TileBase, TilemapManager.TileBaseData>();
        for (int i = 0; i < tileDataList.Count; i++)
        {
            tileTypeMap[tileDataList[i].tileBase] = tileDataList[i];
        }
        return tileTypeMap;
    }
}
