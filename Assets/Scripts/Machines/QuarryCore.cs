using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Visuals;
using Terrain.Data;

public class QuarryCore : MachineBlock
{
  // The amount of tiles we are mining from the quarry
  public int range = 10;

  public int direction = -1;

  private Vector3 startingPosition;

  private int x;
  private int y;

  private int progressPercentage = 0;
  private int progressSpeed = 10;

  // The streght the quarry mines the tiles
  private int miningStrenght = 1;

  protected override void Start()
  {
    core = this;

    startingPosition = transform.position;

    x = (int)transform.position.x;
    y = (int)transform.position.y;
    //x++;
    x -= direction;
  }

  public override void Interact()
  {
    progressPercentage += progressSpeed;
    if (progressPercentage > 100)
    {
      progressPercentage -= 100;
      if (WorldLoader.m_Terrain.GetTileAt(x, y).GetComponent<TileGOData>().Mine(miningStrenght))
      {
        MoveToNextPosition();
      }
    }
  }

  //TODO: boolean to move backward and change position accordingly
  private void MoveToNextPosition()
  {
    //x++;
    x -= direction;
    if (Mathf.Abs(x - startingPosition.x) == range)
    {
      x = (int)startingPosition.x;
      y--;
    }
  }
}