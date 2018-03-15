using System.Collections;
using System.Collections.Generic;
using Terrain.Visuals;
using UnityEngine;

public class MachineBlock : TileGOData
{
  public MachineBlock core;

  public List<TileType> allowedTypes = new List<TileType>();

  public List<MachineBlock> machineBlocks = new List<MachineBlock>();

  public virtual void Interact()
  {
  }

  //-----------------------------------------------------------------------------------------------------------//

  public override void Place()
  {
    base.Place();
    if (!CheckForCore())
      TileCheck(this);
  }

  //-----------------------------------------------------------------------------------------------------------//

  /// <summary>
  /// We check if the neighbour tiles are allowed in this machine and if it does
  /// then we add that to the machineBlocks list and call this function for the neighbours
  /// </summary>
  /// <param name="_tile"></param>
  private void TileCheck(MachineBlock _tile)
  {
    if (_tile == null || _tile.type == TileType.Empty || machineBlocks.Contains(_tile))
    {
      return;
    }

    if (allowedTypes.Contains(_tile.type) || _tile == this)
    {
      machineBlocks.Add(_tile);
      _tile.core = machineBlocks[0];

      TileCheckNeighbours<MachineBlock>(_tile, TileCheck);
    }
  }

  //-----------------------------------------------------------------------------------------------------------//

  /// <summary>
  /// Generic class for calling an action for all the neighbours
  /// </summary>
  /// <typeparam name="T"> Generic tileGoData type </typeparam>
  /// <param name="_tileToCheck"> The tile we want to check the neighbours around </param>
  /// <param name="_action"> The action we want to call on the neighbours </param>
  private void TileCheckNeighbours<T>(TileGOData _tileToCheck, System.Action<T> _action) where T : TileGOData
  {
    _action(_tileToCheck.Up<T>());
    _action(_tileToCheck.Right<T>());
    _action(_tileToCheck.Down<T>());
    _action(_tileToCheck.Left<T>());
  }

  //-----------------------------------------------------------------------------------------------------------//

  /// <summary>
  /// Check for the neighbours to see if any of them has a core
  /// If they do then set that core to ours aswell and add this tile to the cores machineBlocks
  /// </summary>
  /// <returns></returns>
  private bool CheckForCore()
  {
    bool hasCore = false;
    TileCheckNeighbours<MachineBlock>(this, tileNeighbour => hasCore |= HasCore(tileNeighbour));
    return hasCore;
  }

  private bool HasCore(MachineBlock machineBlock)
  {
    if (machineBlock == null)
    {
      return false;
    }

    MachineBlock _core = machineBlock.core;
    if (_core.allowedTypes.Contains(this.type))
    {
      this.core = _core;
      _core.machineBlocks.Add(this);
      return true;
    }
    return false;
  }
}