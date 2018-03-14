using System.Collections;
using System.Collections.Generic;
using Terrain.Visuals;
using UnityEngine;

public class ManualLever : MachineBlock
{
  public override void Interact()
  {
    core.Interact();
  }
}