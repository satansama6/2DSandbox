using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Terrain.Visuals;

public class MouseOverTileType : MonoBehaviour
{
  private Text typeText;

  // Use this for initialization
  private void Start()
  {
    typeText = GetComponent<Text>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (Time.timeSinceLevelLoad > 1)
      typeText.text = "Type: " + (WorldLoader.m_Terrain.GetTileAt((int)(InputManager.sharedInstance.GetScreenToWorldMousePosition().x + 0.5f),
                                                                       (int)(InputManager.sharedInstance.GetScreenToWorldMousePosition().y + 0.5f)).GetComponent<TileGOData>().type);
  }
}