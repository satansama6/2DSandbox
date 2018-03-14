using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using Terrain.Data;

namespace Terrain.Visuals
{
  /// <summary>
  /// Store every tile in databaseList
  /// </summary>
  public class TileDatabase : MonoBehaviour
  {
    public static TileDatabase sharedInstance;

    public TileDatabaseList databaseList;

    private string tileDataFileName = "tilesData.json";

    private void Awake()
    {
      sharedInstance = this;
      //databaseList = LoadTilesFromFile();
      LoadTilesFromResources();
    }

    public TileGOData FetchTileByID(TileType type)
    {
      foreach (GameObject tile in databaseList.tileDatabase)
      {
        if (tile.GetComponent<TileGOData>().type == type)
        {
          return tile.GetComponent<TileGOData>();
        }
      }
      return null;
    }

    private void LoadTilesFromResources()
    {
      GameObject[] GOs = Resources.LoadAll<GameObject>("Tiles") as GameObject[];

      foreach (GameObject GO in GOs)
      {
        databaseList.tileDatabase.Add(GO);
      }
    }

    #region Save/Load

    private TileDatabaseList LoadTilesFromFile()
    {
      string _filePath = Path.Combine(Application.streamingAssetsPath, tileDataFileName);
      if (File.Exists(_filePath))
      {
        string dataAsJson = File.ReadAllText(_filePath);
        TileDatabaseList databaseList = JsonUtility.FromJson<TileDatabaseList>(dataAsJson);
        return databaseList;
      }
      return null;
    }

    private void SaveItemsToFile()
    {
      string _filePath = Path.Combine(Application.streamingAssetsPath, tileDataFileName);
      string jsonData = JsonUtility.ToJson(databaseList);
      File.WriteAllText(_filePath, jsonData);
    }

    #endregion Save/Load
  }
}