using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Terrain.Visuals;

public class MouseOverTilePosition : MonoBehaviour
{
  private Text positionText;

  // Use this for initialization
  private void Start()
  {
    positionText = GetComponent<Text>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (Time.timeSinceLevelLoad > 1)
      positionText.text = "Position: " + (WorldLoader.m_Terrain.GetTileAt((int)(InputManager.sharedInstance.GetScreenToWorldMousePosition().x + 0.5f),
                                                                       (int)(InputManager.sharedInstance.GetScreenToWorldMousePosition().y + 0.5f)).transform.position.x) + " " +
                                                                       (WorldLoader.m_Terrain.GetTileAt((int)(InputManager.sharedInstance.GetScreenToWorldMousePosition().x + 0.5f),
                                                                       (int)(InputManager.sharedInstance.GetScreenToWorldMousePosition().y + 0.5f)).transform.position.y);
  }
}