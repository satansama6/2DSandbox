using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RecipeDatabase : MonoBehaviour
{
  public static RecipeDatabase sharedInstance;

  public List<Recipe> recipeDatabase = new List<Recipe>();

  private void Awake()
  {
    sharedInstance = this;
  }

  private void Start()
  {
    List<Item> _inputItems = new List<Item>
    {
      ItemDatabase.sharedInsatance.FetchItemByType(TileType.Dirt),
      ItemDatabase.sharedInsatance.FetchItemByType(TileType.Snow)
    };
    recipeDatabase.Add(new Recipe(ItemDatabase.sharedInsatance.FetchItemByType(TileType.WaterMachineCore), _inputItems));

    _inputItems = new List<Item>();
    _inputItems.Add(ItemDatabase.sharedInsatance.FetchItemByType(TileType.Snow));

    recipeDatabase.Add(new Recipe(ItemDatabase.sharedInsatance.FetchItemByType(TileType.WaterTank), _inputItems));
  }
}