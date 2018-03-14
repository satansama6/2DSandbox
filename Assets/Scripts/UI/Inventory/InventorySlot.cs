using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
  public GameObject emptyItem;

  // Return the item in the inventorySlot, if we don't have one return null
  public GameObject ItemGO
  {
    get
    {
      if (transform.childCount > 0)
      {
        foreach (Transform child in transform)
        {
          if (child.tag == "Item")
          {
            return child.gameObject;
          }
        }
      }
      return null;
    }
  }

  //-----------------------------------------------------------------------------------------------------------//

  public SlotData Item
  {
    get
    {
      return ItemGO.GetComponent<SlotData>();
    }
  }

  //-----------------------------------------------------------------------------------------------------------//

  // If an item is being dropped in this inventorySlot
  public void OnDrop(PointerEventData eventData)
  {
    // Check if there is an item in this inventory slot and if there is move this one to the other ones slot
    if (ItemGO)
    {
      // Item in this slot
      GameObject GO = ItemGO;

      GO.transform.SetParent(DragHandler.startingSlot.transform);
      GO.transform.localPosition = Vector2.zero;
    }
    // Item dragged to this slot
    DragHandler.itemBeingDragged.transform.SetParent(transform);
    DragHandler.itemBeingDragged.transform.localPosition = Vector2.zero;
  }

  //-----------------------------------------------------------------------------------------------------------//

  // If we move above this slot with the pointer we change the finisingSlot to be this slot
  public void OnPointerEnter(PointerEventData eventData)
  {
    DragHandler.finishingSlot = this;
  }

  //-----------------------------------------------------------------------------------------------------------//

  public void DestroyItem()
  {
    foreach (Transform child in transform)
    {
      if (child.tag == "Item")
      {
        Destroy(child.gameObject);
      }
    }
  }

  //-----------------------------------------------------------------------------------------------------------//

  public void UpdateSlotVisual()
  {
    if (Item.itemCount == 0)
    {
      DestroyImmediate(ItemGO);
    }
    else
    {
      ItemGO.GetComponent<Image>().sprite = Item.item.sprite;
      ItemGO.GetComponentInChildren<Text>().text = Item.itemCount.ToString();
    }
  }

  //-----------------------------------------------------------------------------------------------------------//

  public void UpdateItemCount(int _amount)
  {
    Item.itemCount += _amount;
    UpdateSlotVisual();
  }

  //-----------------------------------------------------------------------------------------------------------//

  public void CreateNewItem(Item itemToAdd)
  {
    GameObject GO = Instantiate(emptyItem);
    GO.transform.SetParent(transform);
    GO.transform.localPosition = Vector2.zero;
    Item.item = itemToAdd;
    Item.itemCount = 0;
  }

  //-----------------------------------------------------------------------------------------------------------//

  public void AddItem(TileType type, int amount)
  {
    if (ItemGO == null)
    {
      Item _itemToAdd = ItemDatabase.sharedInsatance.FetchItemByType(type);
      CreateNewItem(_itemToAdd);
    }
    UpdateItemCount(amount);
  }
}