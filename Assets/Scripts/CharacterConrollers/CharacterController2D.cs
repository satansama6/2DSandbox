using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Data;
using Terrain.Visuals;
using TypeSafeEventManager.Events;

public class CharacterController2D : PhysicsObject
{
  public float maxSpeed = 7;
  public float jumpTakeOffSpeed = 7;

  public int miningStregth = 1;
  public int reachDistance = 3;

  // private SpriteRenderer spriteRenderer;
  //private Animator animator;

  //TODO: if we want to use events we need to override the physicsObject class OnEnable function
  //-----------------------------------------------------------------------------------------------------------//

  #region UnityFunctions

  private void Awake()
  {
    //spriteRenderer = GetComponent<SpriteRenderer>();
    //animator = GetComponent<Animator>();
  }

  //-----------------------------------------------------------------------------------------------------------//

  protected override void Update()
  {
    // Debug.DrawLine(transform.position, transform.position + new Vector3(0, 0.1f, 0));
    base.Update();
    if (Input.GetMouseButton(0))
    {
      Mining();
    }
    if (Input.GetMouseButtonDown(1))
    {
      PlaceBlockFromInventory();
    }
    if (Input.GetKey(KeyCode.E))
    {
      Interact();
    }
  }

  #endregion UnityFunctions

  //-----------------------------------------------------------------------------------------------------------//

  #region Movement

  protected override void ComputeVelocity()
  {
    Vector2 move = Vector2.zero;

    move.x = Input.GetAxis("Horizontal");

    if (Input.GetButtonDown("Jump") && grounded)
    {
      velocity.y = jumpTakeOffSpeed;
    }
    else if (Input.GetButtonUp("Jump"))
    {
      if (velocity.y > 0)
      {
        velocity.y = velocity.y * 0.5f;
      }
    }

    //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
    //if (flipSprite)
    //{
    //  spriteRenderer.flipX = !spriteRenderer.flipX;
    //}

    //animator.SetBool("grounded", grounded);
    //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

    targetVelocity = move * maxSpeed;
  }

  #endregion Movement

  //-----------------------------------------------------------------------------------------------------------//

  /// <summary>
  /// Mining
  /// </summary>
  private void Mining()
  {
    Vector2 _mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    Vector2 _characterPosition = new Vector2(transform.position.x, transform.position.y);
    // If the distance between the character and the place where we clicked is smaller than the reach distance then we call the mine fucntion for that tile
    if (Vector2.Distance(_mousePosition, _characterPosition) < reachDistance)
    {
      WorldLoader.m_Terrain.GetTileAt((int)(_mousePosition.x + 0.5f), (int)(_mousePosition.y + 0.5f)).GetComponent<TileGOData>().Mine(miningStregth);
    }
  }

  //-----------------------------------------------------------------------------------------------------------//

  private void PlaceBlockFromInventory()
  {
    Vector2 _mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    Vector2 _characterPosition = new Vector2(transform.position.x, transform.position.y);
    Vector2 _spawnTilePosition = new Vector3(Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + 0.5f), Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y + 0.5f));

    // Check if there is already a tile here
    if (WorldGeneration.m_Terrain.GetTileAt((int)_spawnTilePosition.x, (int)_spawnTilePosition.y) == 0)
    {
      // change 3 to reachDistance
      if (Vector2.Distance(_mousePosition, _characterPosition) < reachDistance)
      {
        TileType _itemID = InventoryPanel.sharedInstance.GetItemInSelectedInventorySlot(false);
        // Check if there is an item in the selected slot
        if (_itemID != 0)
        {
          // Set the tile
          WorldLoader.m_Terrain.SetTileAt((int)_spawnTilePosition.x, (int)_spawnTilePosition.y, _itemID);
          WorldGeneration.m_Terrain.SetTileAt((int)_spawnTilePosition.x, (int)_spawnTilePosition.y, _itemID);

          // Remove the item from the inventory
          InventoryPanel.sharedInstance.GetItemInSelectedInventorySlot(true);
        }
      }
    }
    else
    {
      GameObject _clickedTile = WorldLoader.m_Terrain.GetTileAt((int)_spawnTilePosition.x, (int)_spawnTilePosition.y);
      _clickedTile.GetComponent<TileGOData>().ShowInventory();
    }
  }

  //-----------------------------------------------------------------------------------------------------------//

  /// <summary>
  /// Interacting with the tiles
  /// </summary>
  private void Interact()
  {
    // If we have a tile with IInteract interface at the characters position then we call the interact function on it.
    Vector2 _characterPosition = new Vector2(transform.position.x, transform.position.y);
    IInteractable _tileToInteract = WorldLoader.m_Terrain.GetTileAt((int)(_characterPosition.x + 0.5f), (int)(_characterPosition.y + 0.5f)).GetComponent<IInteractable>();

    if (_tileToInteract != null)
    {
      _tileToInteract.Interact();
    }
    _tileToInteract = WorldLoader.m_Terrain.GetTileAt((int)(_characterPosition.x + 0.5f), (int)(_characterPosition.y + 1.5f)).GetComponent<IInteractable>();
    if (_tileToInteract != null)
    {
      _tileToInteract.Interact();
    }
  }
}