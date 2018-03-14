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

  public override void Place()
  {
    base.Place();
    if (!CheckForCore())
      TileCheck(this);
  }

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

      TileCheck(_tile.Up().GetComponent<MachineBlock>());

      TileCheck(_tile.Right().GetComponent<MachineBlock>());

      TileCheck(_tile.Down().GetComponent<MachineBlock>());

      TileCheck(_tile.Left().GetComponent<MachineBlock>());
    }
  }

  private bool CheckForCore()
  {
    return HasCore(this.Up().GetComponent<MachineBlock>()) ||
          HasCore(this.Right().GetComponent<MachineBlock>()) ||
          HasCore(this.Down().GetComponent<MachineBlock>()) ||
          HasCore(this.Left().GetComponent<MachineBlock>());
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