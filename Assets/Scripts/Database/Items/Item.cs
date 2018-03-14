using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Data;

public enum TileType
{
  Empty,
  /*Basic Tiles*/
  Dirt,
  Grass,
  Sand,
  Snow,
  Stone,
  /*Ores*/
  Copper,
  Iron,
  Gold,
  /*Machines*/
  WaterMachineCore,
  WaterTank,
  QuarryCore,
  ManualLever,
  PlantIncubatorCore,
  PlantHolder,
  Furnace
}

[System.Serializable]
public class Item
{
  public TileType type = 0;
  public string itemName = "Empty";
  public int stackSize = 64;
  public string slug = "Empty";
  public Sprite sprite = null;
  public bool placable = true;

  public Item()
  {
    this.type = 0;
  }

  public Item(TileType type, string name, int stackSize, string slug)
  {
    this.type = type;
    this.itemName = name;
    this.stackSize = stackSize;
    this.slug = slug;
    this.sprite = Resources.Load<Sprite>("Sprites/" + slug);
  }
}