using System.Collections;
using System.Collections.Generic;
using Terrain.Visuals;
using UnityEngine;

public class ManualLever : MachineBlock, IInteractable
{
  public GameObject lever;

  public override void Interact()
  {
    core.Interact();

    lever.transform.Rotate(transform.forward);
  }
}