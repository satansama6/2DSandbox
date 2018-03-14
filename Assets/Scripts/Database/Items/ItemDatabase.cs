using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Terrain.Data;

/// <summary>
/// This database holds all of our itemData
/// </summary>
///
[System.Serializable]
public class ItemDatabase : MonoBehaviour
{
  public static ItemDatabase sharedInsatance;

  //public ItemDatabaseList databaseList;

  private string itemDataFileName = "itemsData.json";

  public List<Item> itemDatabase = new List<Item>();

  //TESTING
  //public Sprite sprite;

  private void Awake()
  {
    sharedInsatance = this;

    //databaseList.itemDatabase.Add(new Item(1, "Grass", 64, "Grass"));

    //  databaseList.itemDatabase.Add(new Item(2, "Stone", 64, "Stone"));

    //  databaseList.itemDatabase.Add(new Item(3, "Dirt", 64, "Dirt"));

    //  databaseList.itemDatabase.Add(new Item(4, "Sand", 64, "Dirt"));

    //  databaseList.itemDatabase.Add(new Item(5, "Snow", 64, "Snow"));

    //TESTING
    //sprite = database[2].sprite;
    //GameObject GO = new GameObject();
    //GO.AddComponent<SpriteRenderer>().sprite = sprite;
    //  SaveItemsToFile();
    // databaseList = LoadItemsFromFile();
  }

  public Item FetchItemByType(TileType type)
  {
    foreach (Item item in itemDatabase)
    {
      // TODO: in case of performance issues we can simply use database[id] if we imput our items in order
      if (item.type == type)
      {
        return item;
      }
    }
    return null;
  }

  #region Save/load

  // Loads the items from json
  private ItemDatabaseList LoadItemsFromFile()
  {
    string _filePath = Path.Combine(Application.streamingAssetsPath, itemDataFileName);
    if (File.Exists(_filePath))
    {
      string dataAsJson = File.ReadAllText(_filePath);
      ItemDatabaseList databaseList = JsonUtility.FromJson<ItemDatabaseList>(dataAsJson);
      return databaseList;
    }
    return null;
  }

  // Save items to json
  private void SaveItemsToFile()
  {
    string _filePath = Path.Combine(Application.streamingAssetsPath, itemDataFileName);
    string jsonData = JsonUtility.ToJson(itemDatabase);
    File.WriteAllText(_filePath, jsonData);
  }

  #endregion Save/load
}