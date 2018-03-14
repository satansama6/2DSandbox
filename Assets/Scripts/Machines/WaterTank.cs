using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Visuals;
using Terrain.Data;

public class WaterTank : MachineBlock
{
  private float currentWaterAmount = 0;
  private float maxWaterAmount = 100;

  public GameObject waterFill;

  // TODO: do we want more than 1 core for 1 water tank

  public bool AddWater(float amount)
  {
    if (currentWaterAmount + amount < maxWaterAmount)
    {
      currentWaterAmount += amount;
      waterFill.transform.localScale = new Vector3(1, currentWaterAmount / maxWaterAmount, 1);
      return true;
    }
    return false;
  }
}