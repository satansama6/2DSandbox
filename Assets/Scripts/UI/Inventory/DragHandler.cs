using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
  public static GameObject itemBeingDragged;

  public static InventorySlot startingSlot;
  public static InventorySlot finishingSlot;

  public bool isDragable = true;

  //-----------------------------------------------------------------------------------------------------------//

  public void OnBeginDrag(PointerEventData eventData)
  {
    // Store the starting position so we know where to snap back in if we drag outside of the inventory
    startingSlot = GetComponentInParent<InventorySlot>();
    // Store the dragged item
    itemBeingDragged = gameObject;
    GetComponent<CanvasGroup>().blocksRaycasts = false;
    // Set the parent to be the root Canvas so we always render on top of everything
    itemBeingDragged.transform.SetParent(transform.root);
  }

  //-----------------------------------------------------------------------------------------------------------//

  public void OnDrag(PointerEventData eventData)
  {
    // Change the position based on the mouseposition
    transform.position = eventData.position;
  }

  //-----------------------------------------------------------------------------------------------------------//

  public void OnEndDrag(PointerEventData eventData)
  {
    GetComponent<CanvasGroup>().blocksRaycasts = true;

    // Check if we snapped back to our original slot
    if (transform.parent == startingSlot.transform)
    {
      transform.position = startingSlot.transform.position;
    }
    else
    {
      // If we did not drag on an actual slot
      if (itemBeingDragged.transform.parent == transform.root)
      {
        transform.position = startingSlot.transform.position;
        itemBeingDragged.transform.SetParent(startingSlot.transform);
      }
    }
    itemBeingDragged = null;
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    CraftingPanel.sharedInstance.CraftItem(gameObject.GetComponent<SlotData>().item.type);
  }
}