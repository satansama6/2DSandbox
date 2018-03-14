using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
  public Item outputItem;
  public List<Item> inputItems = new List<Item>();

  public Recipe(Item outputItem, List<Item> inputItems)
  {
    this.outputItem = outputItem;
    this.inputItems = inputItems;
  }
}