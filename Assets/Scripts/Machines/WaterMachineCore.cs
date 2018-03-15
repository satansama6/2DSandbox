using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Visuals;
using Terrain.Data;

public class WaterMachineCore : MachineBlock
{
  private int dirtCount = 0;
  private float dirtPercentage = 0;

  private InventorySlot dirtSlot;
  private InventorySlot sandSlot;

  public List<WaterTank> waterTanks = new List<WaterTank>();

  public override void Interact()
  {
    if (dirtSlot != null)
      // TODO: Do it when you put items in the machine slot
      if (dirtSlot.ItemGO != null)
      {
        dirtCount = dirtSlot.Item.itemCount;

        dirtPercentage += 0.5f;

        if (dirtPercentage >= 100)
        {
          dirtPercentage = 0;
          dirtCount--;
          dirtSlot.UpdateItemCount(-1);
          sandSlot.AddItem(TileType.Sand, 1);
        }

        foreach (MachineBlock block in machineBlocks)
        {
          WaterTank _waterTank = block.GetComponent<WaterTank>();
          if (_waterTank != null)
          {
            if (_waterTank.AddWater(0.1f))
            {
              return;
            }
          }
        }
        Debug.LogError("No water tank found!");
      }
  }

  public override void Place()
  {
    base.Place();

    //SetNeighboursToIsCoreless();
    //CheckForCoreInNeighbours();
  }

  protected override void Start()
  {
    core = this;
  }

  public override bool Mine(int amount)
  {
    base.Mine(amount);
    return true;
    // TODO this is wrong
  }

  public override void ShowInventory()
  {
    if (inventory == null)
    {
      inventory = InventoryUtility.sharedInstance.CreateGenericInventory(2);
      dirtSlot = inventory.transform.GetChild(1).transform.GetChild(0).GetComponent<InventorySlot>();
      sandSlot = inventory.transform.GetChild(1).transform.GetChild(1).GetComponent<InventorySlot>();
      inventory.SetActive(false);
    }
    base.ShowInventory();
  }
}