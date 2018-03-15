using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Data
{
  [System.Serializable]
  public class TileData
  {
    public TileType type;

    public Sprite[] tileVisual;

    public int durability;

    public bool isBreakable;

    public bool isMaskable;

    public bool isConnectable;

    public List<TileType> itemDrop = new List<TileType>();

    public TileData(TileType type)
    {
      this.type = type;
    }
  }
}