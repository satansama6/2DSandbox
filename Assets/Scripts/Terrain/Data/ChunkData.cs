using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Data
{
  /// <summary>
  /// Stores the tile types
  /// </summary>
  public class ChunkData
  {
    public static int m_Size = 16;
    private TileData[] m_Tiles;

    public int x;
    public int y;

    // TODO COMMENTING
    // Creates an empty chunk
    public ChunkData(int x, int y)
    {
      this.x = x;
      this.y = y;
      m_Tiles = new TileData[m_Size * m_Size];
      for (int _x = 0; _x < m_Size; _x++)
      {
        for (int _y = 0; _y < m_Size; _y++)
        {
          m_Tiles[_x + _y * ChunkData.m_Size] = new TileData(0);
        }
      }
    }

    /// <summary>
    /// Creates a chunk with tiles data
    /// </summary>
    /// <param name="x"> x position of the chunk </param>
    /// <param name="y"> y position of the chunk </param>
    /// <param name="tiles"> the actual data </param>
    public ChunkData(int x, int y, TileData[,] tiles)
    {
      this.x = x * ChunkData.m_Size;
      this.y = y * ChunkData.m_Size;
      m_Tiles = new TileData[m_Size * m_Size];

      for (int _x = 0; _x < m_Size; _x++)
      {
        for (int _y = 0; _y < m_Size; _y++)
        {
          m_Tiles[_x + _y * ChunkData.m_Size] = new TileData(tiles[_x, _y].type);
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    public ChunkData(int x, int y, TileType[,] tiles)
    {
      this.x = x * ChunkData.m_Size;
      this.y = y * ChunkData.m_Size;
      m_Tiles = new TileData[m_Size * m_Size];

      for (int _x = 0; _x < m_Size; _x++)
      {
        for (int _y = 0; _y < m_Size; _y++)
        {
          m_Tiles[_x + _y * ChunkData.m_Size] = new TileData(tiles[_x, _y]);
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Returns all tiles in this chunk
    /// </summary>
    /// <returns></returns>
    public TileData[] GetTiles()
    {
      return m_Tiles;
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Change tile type at x,y
    /// </summary>
    /// <param name="x"> x-position in chunk. Must be lower than chunk.size </param>
    /// <param name="y"> y-position in chunk. Must be lower than chunk.size </param>
    /// <param name="type"> Type to change the tile </param>
    public void SetTileAt(int x, int y, TileType type)
    {
      m_Tiles[x + y * ChunkData.m_Size].type = type;
    }

    public TileType GetTileAt(int x, int y)
    {
      return m_Tiles[x + y * ChunkData.m_Size].type;
    }
  }
}