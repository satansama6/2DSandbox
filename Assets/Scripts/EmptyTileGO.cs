using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Visuals;

public class EmptyTileGO : TileGOData
{
  public override bool Mine(int amount)
  {
    return true;
  }
}