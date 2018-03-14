using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain.Data
{
  [System.Serializable]
  public class TileData
  {
    public TileType type;

    public TileData(TileType type)
    {
      this.type = type;
    }
  }
}