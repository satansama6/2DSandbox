using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Data
{
  //TODO: Rewrite this to be a simple class and not a monobehaviour
  public class WorldGeneration : MonoBehaviour
  {
    // Singleton
    public static WorldGeneration m_Terrain;

    public static ChunkData[] m_ChunkMap;

    // InChunks
    public static int worldHeight = 24; //128 for   2048 tiles with size 16

    public static int worldWidth = 240;  //655 for  10480 tiles with size 16

    // Array of perlin noise data for tweakable world generation
    public PerlinNoiseData[] perlinNoiseData;

    private void Awake()
    {
      m_Terrain = this;
      m_ChunkMap = new ChunkData[worldWidth * worldHeight];
      for (int _x = 0; _x < worldWidth; _x++)
      {
        for (int _y = 0; _y < worldHeight; _y++)
        {
          m_ChunkMap[_x + _y * worldWidth] = new ChunkData(_x, _y);
        }
      }
    }

    private void Start()
    {
      GenerateWorld();
    }

    private void Update()
    {
      if (generateWorld)
      {
        GenerateWorld();
        generateWorld = false;
        Debug.Log("World Generated");
      }
    }

    public bool generateWorld = true;

    //-----------------------------------------------------------------------------------------------------------//

    private void GenerateWorld()
    {
      CreateOutline();
      FillMap();
      CreateSnow();
      //  CreateIronOre();
      //   CreateGoldOre();
      //  CreateCopperOre();
    }

    //-----------------------------------------------------------------------------------------------------------//

    private float PerlinCalculation3D(float x, float y, float scaleX = 1, float scaleY = 1, float offset = 0)
    {
      return Mathf.PerlinNoise(x * scaleX + offset, y * scaleY + offset);
    }

    private float PerlinCalculation2D(float x, float terrainHeight, float offset = 0)
    {
      return Mathf.PerlinNoise(x * terrainHeight + offset, 0);
    }

    //-----------------------------------------------------------------------------------------------------------//
    //-----------------------------------------------------------------------------------------------------------//
    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Creates the outline for the map with the perlinNoiseData
    /// </summary>
    private void CreateOutline()
    {
      for (int x = 0; x < worldWidth * ChunkData.m_Size; x++)
      {
        float _elevation = 0;
        for (int i = 0; i < perlinNoiseData.Length; i++)
        {
          _elevation += perlinNoiseData[i].m_Scale * PerlinCalculation2D(x, perlinNoiseData[i].m_TerrainHeight, perlinNoiseData[i].m_Offset);
        }
        // Scale up the elevation to world size
        _elevation *= worldHeight * ChunkData.m_Size;
        SetTileAt(x, (int)_elevation, TileType.Dirt);
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// 2. STEP
    /// Fill the map under the map outline with a layer of grass and stone below
    /// </summary>
    private void FillMap()
    {
      for (int x = 0; x < worldWidth * ChunkData.m_Size; x++)
      {
        int y = 0;
        bool _finished = false;
        while (!_finished && y < worldHeight * ChunkData.m_Size)
        {
          // Get the first not empty tile
          if (GetTileAt(x, y) != 0)
          {
            // From that tile go till the bottom of the world
            for (int i = y - 1; i > 0; i--)
            {
              TileType _tileToPlaceId;

              // We want to place grass in 50 tiles from the map outline
              int _elevation = (i + Random.Range(-40, 40));
              if (y - _elevation < 10f)
              {
                _tileToPlaceId = TileType.Dirt;
              }
              else
              {
                _tileToPlaceId = TileType.Stone;
              }
              SetTileAt(x, i, _tileToPlaceId);
            }
            _finished = true;
          }
          y++;
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// 3. STEP
    /// Create snow biomes above certain heights
    /// </summary>
    /// TODO: Create snow layer that is bigger than the fog of war, on the snow biome, because mountains are made of stone and not snow
    private void CreateSnow()
    {
      int _minHeight = 150;
      int _maxHeight = 170;
      int _randomDeviation = 20;

      for (int x = 0; x < worldWidth * ChunkData.m_Size; x++)
      {
        int y = worldHeight * ChunkData.m_Size - 1;
        bool _finished = false;
        while (!_finished && y > 0)
        {
          if (GetTileAt(x, y) != 0)
          {
            for (int i = y; i > _minHeight; i--)
            {
              int _elevation = (i + Random.Range(-_randomDeviation, _randomDeviation));

              if (_elevation > _maxHeight)
              {
                TileType _tileToPlaceId = TileType.Snow;
                SetTileAt(x, i, _tileToPlaceId);
              }
            }
            _finished = true;
          }
          y--;
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// 4. STEP
    /// </summary>
    private void CreateIronOre()
    {
      // Higher scale results in more ore pathes, and lower threshold results in bigger pathes
      float _scaleX = 0.2f;
      float _scaleY = 0.2f;
      float _spawnThreshold = 0.75f;
      float _seed = 1;

      for (int x = 0; x < worldWidth * ChunkData.m_Size; x++)
      {
        int y = 0;

        while (y < worldHeight * ChunkData.m_Size)
        {
          if (GetTileAt(x, y) != 0)
          {
            float _value = PerlinCalculation3D(x, y, _scaleX, _scaleY, _seed);
            if (_value > _spawnThreshold)
            {
              TileType _tileToPlaceId = TileType.Iron;
              SetTileAt(x, y, _tileToPlaceId);
            }
          }
          y++;
        }
      }
    }

    private void CreateGoldOre()
    {
      // Higher scale results in more ore pathes, and lower threshold results in bigger pathes
      float _scaleX = 0.2f;
      float _scaleY = 0.2f;
      float _spawnThreshold = 0.80f;
      float _seed = 10;

      for (int x = 0; x < worldWidth * ChunkData.m_Size; x++)
      {
        int y = 0;

        while (y < worldHeight * ChunkData.m_Size)
        {
          if (GetTileAt(x, y) != 0)
          {
            float _value = PerlinCalculation3D(x, y, _scaleX, _scaleY, _seed);
            if (_value > _spawnThreshold)
            {
              TileType _tileToPlaceId = TileType.Gold;
              SetTileAt(x, y, _tileToPlaceId);
            }
          }
          y++;
        }
      }
    }

    public void CreateCopperOre()
    {
      // Higher scale results in more ore pathes, and lower threshold results in bigger pathes
      float _scaleX = 0.2f;
      float _scaleY = 0.2f;
      float _spawnThreshold = 0.80f;
      float _seed = 100;

      for (int x = 0; x < worldWidth * ChunkData.m_Size; x++)
      {
        int y = 0;

        while (y < worldHeight * ChunkData.m_Size)
        {
          if (GetTileAt(x, y) != 0)
          {
            float _value = PerlinCalculation3D(x, y, _scaleX, _scaleY, _seed);
            if (_value > _spawnThreshold)
            {
              TileType _tileToPlaceId = TileType.Copper;
              SetTileAt(x, y, _tileToPlaceId);
            }
          }
          y++;
        }
      }
    }

    //-----------------------------------------------------------------------------------------------------------//
    //-----------------------------------------------------------------------------------------------------------//
    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="_type"></param>
    public void SetTileAt(int x, int y, TileType type)
    {
      int chunkX = x / ChunkData.m_Size;
      int chunkY = y / ChunkData.m_Size;

      int tileX = x % ChunkData.m_Size;
      int tileY = y % ChunkData.m_Size;

      m_ChunkMap[chunkX + chunkY * worldWidth].SetTileAt(tileX, tileY, type);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileType GetTileAt(int x, int y)
    {
      int _chunkX = x / ChunkData.m_Size;
      int _chunkY = y / ChunkData.m_Size;
      // we need to determine where should we place our tile in the chunk
      int _tileX = x % ChunkData.m_Size;
      int _tileY = y % ChunkData.m_Size;
      return m_ChunkMap[_chunkX + _chunkY * worldWidth].GetTileAt(_tileX, _tileY);
    }

    //-----------------------------------------------------------------------------------------------------------//
  }
}