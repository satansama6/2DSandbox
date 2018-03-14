using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TypeSafeEventManager.Events;

public class CameraZoomController : MonoBehaviour
{
  public int zoomLevel = 1;
  private int zoomIncrease = 1;
  private float cameraSize;

  private void Start()
  {
    cameraSize = Screen.height / (float)256;
    Camera.main.orthographicSize = cameraSize * (float)zoomLevel;
  }

  private void OnEnable()
  {
    EventManagerTypeSafe.AddListener<InputEvents.OnControllPressedEvent>(Zoom);
  }

  private void OnDisable()
  {
    EventManagerTypeSafe.RemoveListener<InputEvents.OnControllPressedEvent>(Zoom);
  }

  private void Zoom(InputEvents.OnControllPressedEvent arg)
  {
    if (arg.delta < 0)
    {
      zoomLevel += zoomIncrease;
      zoomIncrease = 1 + (int)Camera.main.orthographicSize / 10;
      Camera.main.orthographicSize = cameraSize * (float)zoomLevel;
    }
    else
    {
      if (zoomLevel > 1)
      {
        zoomLevel -= zoomIncrease;
        zoomIncrease = 1 + (int)Camera.main.orthographicSize / 20;
        Camera.main.orthographicSize = cameraSize * (float)zoomLevel;
      }
    }
  }

  // Update is called once per frame
  private void Update()
  {
  }
}