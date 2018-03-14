//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TileSpriteManager : MonoBehaviour
//{
//  public static TileSpriteManager sharedInstance;

//  private void Awake()
//  {
//    sharedInstance = this;
//  }

//  public Sprite[] dirtTiles = new Sprite[64];
//  public Sprite[] stoneTiles = new Sprite[64];
//  public Sprite[] snowTiles = new Sprite[64];

//  public Sprite snow;

//  public Sprite sand;

//  public Sprite iron;

//  public Sprite gold;

//  public Sprite copper;

//  public Sprite waterMachineCore;

//  public Sprite waterTank;

//  public Sprite quarryCore;

//  public Sprite manualLever;

//  public Sprite plantIncubatorCore;

//  public Sprite plantHolder;

//  public Sprite furnace;

//  public List<Sprite> mask = new List<Sprite>();
//  public List<Sprite> outline = new List<Sprite>();

//  public Sprite GetTileForPosition(int x, int y, TileType type)
//  {
//    x = x % 8;
//    y = y % 8;

//    if (type == TileType.Dirt)
//    {
//      return dirtTiles[(56 - y * 8) + x];
//    }

//    if (type == TileType.Stone)
//    {
//      return stoneTiles[(56 - y * 8) + x];
//    }
//    if (type == TileType.Sand)
//    {
//      return sand;
//    }
//    if (type == TileType.Snow)
//    {
//      return snowTiles[(56 - y * 8) + x];
//    }
//    if (type == TileType.Iron)
//    {
//      return iron;
//    }

//    if (type == TileType.Gold)
//    {
//      return gold;
//    }

//    if (type == TileType.Copper)
//    {
//      return copper;
//    }
//    if (type == TileType.WaterMachineCore)
//    {
//      return waterMachineCore;
//    }
//    if (type == TileType.WaterTank)
//    {
//      return waterTank;
//    }

//    if (type == TileType.QuarryCore)
//    {
//      return quarryCore;
//    }

//    if (type == TileType.ManualLever)
//    {
//      return manualLever;
//    }

//    if (type == TileType.PlantIncubatorCore)
//    {
//      return plantIncubatorCore;
//    }

//    if (type == TileType.PlantHolder)
//    {
//      return plantHolder;
//    }

//    if (type == TileType.Furnace)
//    {
//      return furnace;
//    }

//    if (type == TileType.Empty)
//    {
//      return null;
//    }

//    Debug.LogWarning("Can not find sprite for tile: " + type);
//    return null;
//  }
//}