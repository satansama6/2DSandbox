using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TypeSafeEventManager.Events;

/// <summary>
/// Helper class for inventory management
/// </summary>
public class Inventory : MonoBehaviour
{
  public GameObject emptyItem;

  /// <summary>
  /// Adds an item into the inventory
  /// </summary>
  /// <param name="type"> Item to add type </param>
  /// <param name="amount"> Amount to add </param>
  /// <param name="slots"> The inventory slots that we should add to </param>
  protected void AddItem(TileType type, int amount, InventorySlot[] slots)
  {
    Item _itemToAdd = ItemDatabase.sharedInsatance.FetchItemByType(type);
    int _position = WhereIsItemInInventory(type, slots);
    if (_position != -1)
    {
      UpdateItemCount(_position, amount, slots);
      return;
    }
    else
    {
      //TODO: use while loop
      for (int i = 0; i < slots.Length; i++)
      {
        // If the slot is empty
        if (slots[i].ItemGO == null)
        {
          CreateNewItem(i, _itemToAdd, slots);
          UpdateItemCount(i, amount, slots);
          return;
        }
      }
    }
  }

  /// <summary>
  /// Removes the first item that we can find from the inventory
  /// </summary>
  /// <param name="type"> Item to remove type </param>
  /// <param name="amount"> Amount to remove </param>
  /// <param name="slots"> Slots to remove from </param>
  public void RemoveItem(TileType type, int amount, InventorySlot[] slots)
  {
    int _position = WhereIsItemInInventory(type, slots);
    if (_position != -1)
    {
      UpdateItemCount(_position, amount, slots);
      return;
    }
    else
    {
      Debug.LogWarning("Item you try to remove does not exist in the inventory (maybe we don't need this warning)");
    }
  }

  /// <summary>
  /// Check if item with type is in inventory and return its position
  /// </summary>
  /// <param name="type"> The item type that we are looking for </param>
  /// <returns> A posion where we found the item, -1 if there is no item with type in inventory </returns>

  protected int WhereIsItemInInventory(TileType type, InventorySlot[] slots)
  {
    for (int i = 0; i < slots.Length; i++)
    {
      if (slots[i].ItemGO != null)
      {
        if (slots[i].Item.item.type == type && slots[i].Item.itemCount < slots[i].Item.item.stackSize)
        {
          return i;
        }
      }
    }
    return -1;
  }

  /// <summary>
  /// Changes the itemCount in _position with _amount
  /// </summary>
  /// <param name="position"> Slot number </param>
  ///  <param name="number"> Change amount </param>
  protected void UpdateItemCount(int _position, int _amount, InventorySlot[] slots)
  {
    slots[_position].UpdateItemCount(_amount);
  }

  /// <summary>
  /// Updates the item sprite at position and updates the item count
  /// </summary>
  /// <param name="position"> Posion for item that we want to update the visuals </param>
  protected void UpdateInventoryVisuals(int _position, InventorySlot[] slots)
  {
    slots[_position].UpdateSlotVisual();

    // REMOVE
    //// If the itemCount is 0 we destroy the item
    //if (slots[_position].Item.itemCount == 0)
    //{
    //  DestroyImmediate(slots[_position].ItemGO);
    //}
    //else
    //{
    //  slots[_position].ItemGO.GetComponent<Image>().sprite = slots[_position].Item.item.sprite;

    //  slots[_position].ItemGO.GetComponentInChildren<Text>().text = slots[_position].Item.itemCount.ToString();
    //}

    // ENDREMOVE
  }

  /// <summary>
  /// When we want to add an item that does not yet exist in the inventory we create a new empty item
  /// </summary>
  /// <param name="slotNumber"> Position to put the item in the inventory </param>
  /// <param name="itemToAdd"></param>
  /// <param name="slots"></param>
  protected void CreateNewItem(int slotNumber, Item itemToAdd, InventorySlot[] slots)
  {
    GameObject GO = Instantiate(emptyItem);
    GO.transform.SetParent(slots[slotNumber].transform);
    GO.transform.localPosition = Vector2.zero;
    slots[slotNumber].Item.item = itemToAdd;
    slots[slotNumber].Item.itemCount = 0;
  }

  /// <summary>
  ///  Empty all the slots in the inventory
  /// </summary>
  /// <param name="slots"></param>
  public void EmptySlots(InventorySlot[] slots)
  {
    for (int i = 0; i < slots.Length; i++)
    {
      DestroyImmediate(slots[i].ItemGO);
    }
  }
}