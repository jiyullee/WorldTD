using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private Tilemap _tilemap;
    public GameObject tempObject;

    [SerializeField] private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;
    private List<TileBase> towerTiles = new List<TileBase>();
    private List<TileBase> placed_Tiles = new List<TileBase>();
    private List<TileBase> unPlaced_Tiles = new List<TileBase>();
    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        dataFromTiles = new Dictionary<TileBase, TileData>();
        
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                if(!dataFromTiles.ContainsKey(tile))
                    dataFromTiles.Add(tile, tileData);
            }
        }

        for (int i = -10; i < _tilemap.size.y; i++)
        {
            for (int j = -10; j < _tilemap.size.x; j++)
            {
                Vector3Int vInt = new Vector3Int(j,i,0);
                Vector3 pos = _tilemap.CellToWorld(vInt) + new Vector3(0.32f,0.32f,0f);
                TileBase tile = _tilemap.GetTile(vInt);
           
                if (tile && dataFromTiles.ContainsKey(tile))
                {
                    if (dataFromTiles[tile].tile_data == TILE_DATA.TOWER)
                    {
                        GameObject obj = Instantiate(tempObject, pos, Quaternion.identity);
                        obj.transform.parent = transform;
                        towerTiles.Add(tile);
                    }
                }
                    
            }
        }
    }
    
}