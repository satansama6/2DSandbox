using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Data;

namespace Terrain.Visuals
{
  public class ChunkVisual : MonoBehaviour
  {
    public GameObject[] tileGO = new GameObject[ChunkData.m_Size * ChunkData.m_Size];

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Requests new gameObjects from the pool and places them in the correct positions
    /// </summary>
    /// <param name="_tiles"> The data for the tiles </param>
    public void DrawChunk(TileData[] _tiles)
    {
      GameObject _TileType = null;
      for (int _x = 0; _x < ChunkData.m_Size; _x++)
      {
        for (int _y = 0; _y < ChunkData.m_Size; _y++)
        {
          _TileType = TileDatabase.sharedInstance.FetchTileByID(_tiles[_x + _y * ChunkData.m_Size].type).gameObject;

          _TileType = _TileType.GetComponent<PooledMonobehaviour>().Get<PooledMonobehaviour>().gameObject;

          _TileType.transform.position = new Vector3(transform.position.x + _x, transform.position.y + _y, 0);

          _TileType.GetComponent<TileGOData>().UpdateVisual(_x, _y);

          _TileType.transform.parent = transform;

          tileGO[_x + _y * ChunkData.m_Size] = _TileType;

          WorldLoader.m_Terrain.dirtyTiles.Enqueue(_TileType.GetComponent<TileGOData>());
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Marks as dirty all the tiles that are on the edge of this chunk
    /// </summary>
    public void DirtyEdgeTiles()
    {
      //Top side
      int _y = 0;
      int _x = 0;
      for (_x = 0; _x < ChunkData.m_Size; _x++)
      {
        WorldLoader.m_Terrain.dirtyTiles.Enqueue(GetTileAt(_x, _y).GetComponent<TileGOData>());
      }

      // Bottom
      _y = ChunkData.m_Size - 1;
      for (_x = 0; _x < ChunkData.m_Size; _x++)
      {
        WorldLoader.m_Terrain.dirtyTiles.Enqueue(GetTileAt(_x, _y).GetComponent<TileGOData>());
      }

      // Left
      _x = 0;
      for (_y = 0; _y < ChunkData.m_Size; _y++)
      {
        WorldLoader.m_Terrain.dirtyTiles.Enqueue(GetTileAt(_x, _y).GetComponent<TileGOData>());
      }

      // Right
      _x = ChunkData.m_Size - 1;
      for (_y = 0; _y < ChunkData.m_Size; _y++)
      {
        WorldLoader.m_Terrain.dirtyTiles.Enqueue(GetTileAt(_x, _y).GetComponent<TileGOData>());
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Returns the tile from this chunk at x and y
    /// </summary>
    /// <param name="x"> x position </param>
    /// <param name="y"> y position </param>
    /// <returns></returns>
    public GameObject GetTileAt(int x, int y)
    {
      return tileGO[x + y * ChunkData.m_Size];
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Sets a tile with type <type> at x and y
    /// </summary>
    /// <param name="x"> x position </param>
    /// <param name="y"> y position</param>
    /// <param name="type"> Tile type</param>
    public void SetTileAt(int x, int y, TileType type)
    {
      GameObject _TileType = null;
      _TileType = TileDatabase.sharedInstance.FetchTileByID(type).gameObject;
      _TileType = _TileType.GetComponent<PooledMonobehaviour>().Get<PooledMonobehaviour>().gameObject;
      _TileType.transform.position = new Vector3(x + transform.position.x, y + transform.position.y, 0);
      tileGO[x + y * ChunkData.m_Size] = _TileType;
      _TileType.transform.parent = transform;

      _TileType.GetComponent<TileGOData>().Place();
    }
  }
}