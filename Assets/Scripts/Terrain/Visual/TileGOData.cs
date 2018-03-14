using System.Collections;
using System.Collections.Generic;
using Terrain.Data;
using UnityEngine;

namespace Terrain.Visuals
{
  public class TileGOData : MonoBehaviour
  {
    public TileType type;

    public Sprite[] tileVisual;

    public int durability;

    public bool isBreakable = true;

    public bool isMaskable = true;

    public bool isConnectable = true;

    public List<TileType> itemDrop = new List<TileType>();

    [HideInInspector]
    public GameObject inventory;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
      spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// We call this function whenever a block gets clickedOn(mined)
    /// </summary>
    /// <param name="amount"> Mining strenght, reduce durability by this amount </param>
    /// <returns> True if tile got mined </returns>
    public virtual bool Mine(int amount)
    {
      if (isBreakable)
      {
        durability -= amount;
        if (durability <= 0)
        {
          WorldGeneration.m_Terrain.SetTileAt((int)transform.position.x, (int)transform.position.y, 0);
          WorldLoader.m_Terrain.SetTileAt((int)transform.position.x, (int)transform.position.y, 0);
          foreach (TileType type in TileDatabase.sharedInstance.FetchTileByID(type).itemDrop)
          {
            InventoryPanel.sharedInstance.AddItem(type, 1);
          }
          MarkTilesDirtyAround();
          gameObject.SetActive(false);
          return true;
        }
      }
      return false;
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Adds every tile around this one to the dirty queue
    /// </summary>
    private void MarkTilesDirtyAround()
    {
      AddToDirtyQueue(Up());
      AddToDirtyQueue(UpLeft());
      AddToDirtyQueue(UpRight());
      AddToDirtyQueue(Left());
      AddToDirtyQueue(Right());
      AddToDirtyQueue(Down());
      AddToDirtyQueue(DownLeft());
      AddToDirtyQueue(DownRight());
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Adds _tileToAdd tile into the dirty queue
    /// </summary>
    /// <param name="_tileToAdd"> The tile to be added to the dirty queue</param>
    public void AddToDirtyQueue(TileGOData _tileToAdd)
    {
      if (_tileToAdd.isMaskable)
      {
        WorldLoader.m_Terrain.dirtyTiles.Enqueue(_tileToAdd);
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Gets called when we place a block
    /// </summary>
    public virtual void Place()
    {
      // Set the sprite the correct sprite for the tile based on its position
      UpdateVisual((int)transform.position.x, (int)transform.position.y);
      AddToDirtyQueue(this);
      MarkTilesDirtyAround();
    }

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// If the inventory is active we deactive it
    /// If the inventory is inactive we active it
    /// </summary>
    public virtual void ShowInventory()
    {
      if (inventory != null)
      {
        if (inventory.activeInHierarchy)
        {
          inventory.SetActive(false);
          return;
        }
        inventory.SetActive(true);
      }
    }

    //-----------------------------------------------------------------------------------------------------------//

    #region NeighbourTiles

    public TileGOData CheckNeighbour(int _x, int _y)
    {
      GameObject GO = WorldLoader.m_Terrain.GetTileAt(_x, _y);

      return GO != null ? GO.GetComponent<TileGOData>() : null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public TileGOData UpLeft()
    {
      return CheckNeighbour((int)transform.position.x - 1, (int)transform.position.y + 1);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileGOData Up()
    {
      return CheckNeighbour((int)transform.position.x, (int)transform.position.y + 1);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileGOData UpRight()
    {
      return CheckNeighbour((int)transform.position.x + 1, (int)transform.position.y + 1);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileGOData Left()
    {
      return CheckNeighbour((int)transform.position.x - 1, (int)transform.position.y);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileGOData Right()
    {
      return CheckNeighbour((int)transform.position.x + 1, (int)transform.position.y);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileGOData DownLeft()
    {
      return CheckNeighbour((int)transform.position.x - 1, (int)transform.position.y - 1);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileGOData Down()
    {
      return CheckNeighbour((int)transform.position.x, (int)transform.position.y - 1);
    }

    //-----------------------------------------------------------------------------------------------------------//

    public TileGOData DownRight()
    {
      return CheckNeighbour((int)transform.position.x + 1, (int)transform.position.y - 1);
    }

    #endregion NeighbourTiles

    //-----------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Changes the tiles visual based on its position
    /// </summary>
    /// <param name="_x"> x position </param>
    /// <param name="_y"> y position </param>
    public void UpdateVisual(int _x, int _y)
    {
      if (isConnectable)
      {
        _x = (int)transform.position.x % 8;
        _y = (int)transform.position.y % 8;
        spriteRenderer.sprite = tileVisual[(56 - _y * 8) + _x];
      }
      else
      {
        spriteRenderer.sprite = tileVisual[0];
      }
    }
  }
}