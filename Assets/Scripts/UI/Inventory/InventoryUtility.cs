using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUtility : MonoBehaviour
{
  public static InventoryUtility sharedInstance;

  public GameObject genericInventoryPanel;
  public GameObject slot;

  private void Awake()
  {
    sharedInstance = this;
  }

  public GameObject CreateGenericInventory(int slotCount)
  {
    GameObject _panel = Instantiate(genericInventoryPanel, transform);

    for (int i = 0; i < slotCount; i++)
    {
      Instantiate(slot, _panel.transform.GetChild(1));
    }
    return _panel;
  }
}