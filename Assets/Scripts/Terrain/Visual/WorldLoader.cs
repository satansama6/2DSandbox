using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Data;

namespace Terrain.Visuals
{
  public class WorldLoader : MonoBehaviour
  {
    public static WorldLoader m_Terrain;

    public GameObject m_Character;
    public GameObject m_ChunkGO;

    // The rendering distance in chunks
    public int m_RenderDisatance;

    // Stores the chunks and the corresponding positions in actual world position so chunk_2_2 position is 32,32
    private Dictionary<Vector2, ChunkVisual> m_ChunkMap;

    // We store the dirty tiles in this que and in each frame we recalculate the visuals for the dirt tiles
    public Queue<TileGOData> dirtyTiles = new Queue<TileGOData>();

    private float timer = 0;

    private void Awake()
    {
      m_Terrain = this;
      m_ChunkMap = new Dictionary<Vector2, ChunkVisual>();
    }

    private void Start()
    {
    }

    private void Update()
    {
      timer += Time.deltaTime;
      if (timer > 0.1f)
      {
        timer = 0;
        FindChunksToLoad();
        FindChunksToUnload();
      }
      RedrawDirtyTiles();
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// We load all the chunks that is inside of renderDistance
    /// Distance is in chunk size
    /// </summary>
    private void FindChunksToLoad()
    {
      int _x;
      int _y;

      _x = (int)m_Character.transform.position.x / ChunkData.m_Size;
      _y = (int)m_Character.transform.position.y / ChunkData.m_Size;

      for (int x = _x - m_RenderDisatance; x < _x + m_RenderDisatance; x++)
      {
        for (int y = _y - m_RenderDisatance; y < _y + m_RenderDisatance; y++)
        {
          LoadChunkAt(x, y);
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    // Chunk(5,5)= 5*chunk.size, 5*chunk.size in world

    /// <summary>
    /// Draws the chunk visuals at x and y
    /// </summary>
    /// <param name="x"> x position in chunk space </param>
    /// <param name="y"> y position in chunk space </param>
    private void LoadChunkAt(int x, int y)
    {
      int _chunkX = x * ChunkData.m_Size;    // world positions
      int _chunkY = y * ChunkData.m_Size;
      // Check if the chunk is not loaded yet
      if (m_ChunkMap.ContainsKey(new Vector2(_chunkX, _chunkY)) == false)
      {
        GameObject _chunk = null;
        // Request new chunkGO from pool
        _chunk = m_ChunkGO.GetComponent<PooledMonobehaviour>().Get<PooledMonobehaviour>().gameObject;
        _chunk.transform.position = new Vector3(_chunkX, _chunkY, 0);

        _chunk.transform.name = "Chunk_" + x + "_" + y;
        _chunk.transform.parent = transform;

        // Add the new chunk to the chunkMap
        m_ChunkMap.Add(new Vector2(_chunkX, _chunkY), _chunk.GetComponent<ChunkVisual>());

        // Check if the chunk is outside of our generated world
        if (x > 0 && x < WorldGeneration.m_ChunkMap.GetLength(0) && y > 0 && y < WorldGeneration.m_ChunkMap.GetLength(0))
        {
          // Draw the actual tiles for the chuk
          _chunk.GetComponent<ChunkVisual>().DrawChunk(WorldGeneration.m_ChunkMap[x + y * WorldGeneration.worldWidth].GetTiles());

          // Call dirty edge tiles for the neighbour chunks
          if (m_ChunkMap.ContainsKey(new Vector2(_chunkX - ChunkData.m_Size, _chunkY)))
          {
            m_ChunkMap[new Vector2(_chunkX - ChunkData.m_Size, _chunkY)].DirtyEdgeTiles();
          }

          if (m_ChunkMap.ContainsKey(new Vector2(_chunkX + ChunkData.m_Size, _chunkY)))
          {
            m_ChunkMap[new Vector2(_chunkX + ChunkData.m_Size, _chunkY)].DirtyEdgeTiles();
          }

          if (m_ChunkMap.ContainsKey(new Vector2(_chunkX, _chunkY - ChunkData.m_Size)))
          {
            m_ChunkMap[new Vector2(_chunkX, _chunkY - ChunkData.m_Size)].DirtyEdgeTiles();
          }

          if (m_ChunkMap.ContainsKey(new Vector2(_chunkX, _chunkY + ChunkData.m_Size)))
          {
            m_ChunkMap[new Vector2(_chunkX, _chunkY + ChunkData.m_Size)].DirtyEdgeTiles();
          }
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Finds the chunks that the distace to the player is larger than drawDistance
    /// </summary>
    private void FindChunksToUnload()
    {
      List<ChunkVisual> _unloadChunks = new List<ChunkVisual>(m_ChunkMap.Values);
      Queue<ChunkVisual> _unloadQueue = new Queue<ChunkVisual>();

      // Check for all of the loaded chunks
      for (int i = 0; i < _unloadChunks.Count; i++)
      {
        float _distance = Vector3.Distance(m_Character.transform.position, _unloadChunks[i].transform.position);

        // There was an issue where we kept loading and unloading the chunks
        // thats why there is a +2 here so we only unload if we passed that number
        if (_distance > (m_RenderDisatance + 2) * ChunkData.m_Size)
        {
          // Queue the chunks that needs to be deleted. If we unload them here that will fuck up the deleteChunks order
          _unloadQueue.Enqueue(_unloadChunks[i]);
        }
      }
      // Unload all the chunks that needs to be unloaded
      while (_unloadQueue.Count > 0)
      {
        ChunkVisual _chunk = _unloadQueue.Dequeue();
        m_ChunkMap.Remove(_chunk.transform.position);
        UnLoadChunk(_chunk.gameObject);
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Unloads the chunk
    /// </summary>
    /// <param name="_chunk"> Chunk to unload </param>
    private void UnLoadChunk(GameObject _chunk)
    {
      foreach (Transform child in _chunk.transform)
      {
        child.gameObject.SetActive(false);
      }
      _chunk.SetActive(false);
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Returns the tileGO at x and y
    /// </summary>
    /// <param name="x"> x position </param>
    /// <param name="y"> y position </param>
    /// <returns></returns>
    public GameObject GetTileAt(int x, int y)
    {
      //  x = Mathf.Round(x);
      // y = Mathf.Round(y);

      // No need to devide with 16
      //  int _chunkX = (int)(x / 16) * 16;
      //  int _chunkY = (int)(y / 16) * 16;

      int _tileX = x % ChunkData.m_Size;
      int _tileY = y % ChunkData.m_Size;

      int _chunkX = x - _tileX;
      int _chunkY = y - _tileY;

      if (m_ChunkMap.ContainsKey(new Vector2(_chunkX, _chunkY)))
      {
        return m_ChunkMap[new Vector2(_chunkX, _chunkY)].GetTileAt(_tileX, _tileY);
      }
      return null;
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Returns the Tile type at x and y
    /// </summary>
    /// <param name="x"> x position </param>
    /// <param name="y"> y position </param>
    /// <returns> TileType </returns>
    public TileType GetTileTypeAt(int x, int y)
    {
      int _tileX = x % ChunkData.m_Size;
      int _tileY = y % ChunkData.m_Size;

      int _chunkX = x - _tileX;
      int _chunkY = y - _tileY;
      if (m_ChunkMap.ContainsKey(new Vector2(_chunkX, _chunkY)))
      {
        return m_ChunkMap[new Vector2(_chunkX, _chunkY)].GetTileAt(_tileX, _tileY).GetComponent<TileGOData>().type;
      }
      return 0;
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Sets the tile to tileType at x and y
    /// </summary>
    /// <param name="x"> x position</param>
    /// <param name="y"> y position </param>
    /// <param name="type"> Tile type  </param>
    public void SetTileAt(int x, int y, TileType type)
    {
      // We need this division and multiplication in the if below
      //int _chunkX = x / ChunkData.m_Size;
      //int _chunkY = y / ChunkData.m_Size;

      //int _tileX = x % ChunkData.m_Size;
      //int _tileY = y % ChunkData.m_Size;

      int _tileX = x % ChunkData.m_Size;
      int _tileY = y % ChunkData.m_Size;

      int _chunkX = x - _tileX;
      int _chunkY = y - _tileY;
      if (m_ChunkMap.ContainsKey(new Vector2(_chunkX, _chunkY)) == true)
      {
        m_ChunkMap[new Vector2(_chunkX, _chunkY)].SetTileAt(_tileX, _tileY, type);
      }
      else
      {
        Debug.LogError("Could not find chunk at " + _chunkX + "_" + _chunkY + " position!");
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Recalculates the dirty tiles corners for the shader
    /// </summary>
    private void RedrawDirtyTiles()
    {
      while (dirtyTiles.Count != 0)
      {
        TileGOData _tileToRedraw = dirtyTiles.Dequeue();

        // Create the bitMask with the neighbours
        byte bitmaskValue = 0;

        if (_tileToRedraw.UpLeft() != null && _tileToRedraw.UpLeft().type != _tileToRedraw.type)
        {
          bitmaskValue += 1;
        }

        if (_tileToRedraw.Up() != null && _tileToRedraw.Up().type != _tileToRedraw.type)
        {
          bitmaskValue += 2;
        }

        if (_tileToRedraw.UpRight() != null && _tileToRedraw.UpRight().type != _tileToRedraw.type)
        {
          bitmaskValue += 4;
        }

        if (_tileToRedraw.Left() != null && _tileToRedraw.Left().type != _tileToRedraw.type)
        {
          bitmaskValue += 8;
        }

        if (_tileToRedraw.Right() != null && _tileToRedraw.Right().type != _tileToRedraw.type)
        {
          bitmaskValue += 16;
        }

        if (_tileToRedraw.DownLeft() != null && _tileToRedraw.DownLeft().type != _tileToRedraw.type)
        {
          bitmaskValue += 32;
        }

        if (_tileToRedraw.Down() != null && _tileToRedraw.Down().type != _tileToRedraw.type)
        {
          bitmaskValue += 64;
        }

        if (_tileToRedraw.DownRight() != null && _tileToRedraw.DownRight().type != _tileToRedraw.type)
        {
          bitmaskValue += 128;
        }

        // Cache the material
        Material _tileMaterial = _tileToRedraw.GetComponentInChildren<SpriteRenderer>().material;

        // Get the mask index
        byte _value = GetMaskIntTL(bitmaskValue & Bits.TOP_LEFT);

        // Set the value in the shader
        _tileMaterial.SetFloat("_TopLeftMaskTile", _value);

        _value = GetMaskIntTR(bitmaskValue & Bits.TOP_RIGHT);

        _tileMaterial.SetFloat("_TopRightMaskTile", _value);

        _value = GetMaskIntBL(bitmaskValue & Bits.BOTTOM_LEFT);

        _tileMaterial.SetFloat("_BottomLeftMaskTile", _value);

        _value = GetMaskIntBR(bitmaskValue & Bits.BOTTOL_RIGHT);

        _tileMaterial.SetFloat("_BottomRightMaskTile", _value);

        float _x = _tileToRedraw.transform.position.x;
        float _y = _tileToRedraw.transform.position.y;

        // Change the offset of the shader based on the tiles position
        _tileMaterial.SetVector("_UVOffset", GetUVOffset(_x, _y));
      }
    }

    #region GetMaskForCorners

    public byte GetMaskIntTL(int maskValue)
    {
      switch (maskValue)
      {
        case 0: return 12;
        case 1: return 7;
        case 2: return 6;
        case 3: return 9;
        case 8: return 5;
        case 9: return 11;
        case 10: return 14;
        case 11: return 0;
        default:
          Debug.LogWarning("Bitmask value not implemented in TL: " + maskValue);
          return 255;
      }
    }

    public byte GetMaskIntTR(int maskValue)
    {
      switch (maskValue)
      {
        case 0: return 12;
        case 2: return 7;
        case 4: return 6;
        case 6: return 9;
        case 16: return 4;
        case 18: return 13;
        case 20: return 10;
        case 22: return 1;
        default:
          Debug.LogWarning("Bitmask value not implemented in TR: " + maskValue);
          return 255;
      }
    }

    public byte GetMaskIntBL(int maskValue)
    {
      switch (maskValue)
      {
        case 0: return 12;
        case 8: return 7;
        case 32: return 5;
        case 40: return 11;
        case 64: return 4;
        case 72: return 13;
        case 96: return 8;
        case 104: return 2;

        default:
          Debug.LogWarning("Bitmask value not implemented in BL: " + maskValue);
          return 255;
      }
    }

    public byte GetMaskIntBR(int maskValue)
    {
      switch (maskValue)
      {
        case 0: return 12;
        case 16: return 6;
        case 64: return 5;
        case 80: return 14;
        case 128: return 4;
        case 144: return 10;
        case 192: return 8;
        case 208: return 3;
        default:
          Debug.LogWarning("Bitmask value not implemented: " + maskValue);
          return 255;
      }
    }

    #endregion GetMaskForCorners

    /// <summary>
    /// Returns the offset of the shaders UV based on x and y
    /// </summary>
    /// <param name="_x"> x position </param>
    /// <param name="_y"> y position </param>
    /// <returns> Mask index in the mask texture </returns>
    private Vector4 GetUVOffset(float _x, float _y)
    {
      Vector4 _v4 = new Vector4(0, 0, 0, 0);
      _x %= 8;
      _y %= 8;

      switch ((int)_x)
      {
        case 0:
          _v4.x = 0.05f;
          break;

        case 1:
          _v4.x = 0.15f;
          break;

        case 2:
          _v4.x = 0.25f;
          break;

        case 3:
          _v4.x = 0.35f;
          break;

        case 4:
          _v4.x = 0.45f;
          break;

        case 5:
          _v4.x = 0.55f;
          break;

        case 6:
          _v4.x = 0.65f;
          break;

        case 7:
          _v4.x = 0.75f;
          break;

        default:
          _v4.x = 0;
          break;
      }

      switch ((int)_y)
      {
        case 0:
          _v4.y = 0.05f;
          break;

        case 1:
          _v4.y = 0.15f;
          break;

        case 2:
          _v4.y = 0.25f;
          break;

        case 3:
          _v4.y = 0.35f;
          break;

        case 4:
          _v4.y = 0.45f;
          break;

        case 5:
          _v4.y = 0.55f;
          break;

        case 6:
          _v4.y = 0.65f;
          break;

        case 7:
          _v4.y = 0.75f;
          break;

        default:
          _v4.y = 0;
          break;
      }
      return _v4;
    }
  }
}