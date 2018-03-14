using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TypeSafeEventManager.Events;
using System.Linq;

[RequireComponent(typeof(RecipeDatabase))]
public class CraftingPanel : Inventory
{
  public static CraftingPanel sharedInstance;
  private bool recalculateCraftableItems = false;

  #region Events

  private void OnEnable()
  {
    EventManagerTypeSafe.AddListener<UIEvents.OnItemAddedOrRemoved>(CheckCraftableItems);
  }

  private void OnDisable()
  {
    EventManagerTypeSafe.RemoveListener<UIEvents.OnItemAddedOrRemoved>(CheckCraftableItems);
  }

  #endregion Events

  private InventorySlot[] slots = new InventorySlot[8];
  private Recipe[] recipes = new Recipe[8];

  private void Awake()
  {
    sharedInstance = this;
    int i = 0;
    foreach (Transform child in transform)
    {
      slots[i] = (child.gameObject.GetComponent<InventorySlot>());
      i++;
    }
  }

  private void CheckCraftableItems(UIEvents.OnItemAddedOrRemoved arg)
  {
    EmptySlots(slots);
    ClearRecipes();
    int i = 0;
    foreach (Recipe recipe in RecipeDatabase.sharedInstance.recipeDatabase)
    {
      if (!recipe.inputItems.Except(InventoryPanel.sharedInstance.GetItemList()).Any())
      {
        AddItem(recipe.outputItem.type, 1, slots);
        recipes[i] = recipe;
        i++;
      }
    }
  }

  private void ClearRecipes()
  {
    for (int i = 0; i < recipes.Length; i++)
    {
      recipes[i] = null;
    }
  }

  public void CraftItem(TileType type)
  {
    int i = WhereIsItemInInventory(type, slots);
    if (i != -1)
    {
      AddItem(recipes[i].outputItem.type, 1, InventoryPanel.sharedInstance.slots);
      foreach (Item item in recipes[i].inputItems)
      {
        RemoveItem(item.type, -1, InventoryPanel.sharedInstance.slots);
        recalculateCraftableItems = true;
      }
    }
    else
    {
      Debug.LogError("Item with type: " + type + " does not exist!");
    }
  }

  private void Update()
  {
    if (recalculateCraftableItems)
    {
      CheckCraftableItems(new UIEvents.OnItemAddedOrRemoved());
      recalculateCraftableItems = false;
    }
  }
}