using UnityEngine;
using TypeSafeEventManager.Events;

/// <summary>
/// We trigger events based on what we do with the mouse
/// Usage:  EventManagerTypeSafe.AddListener<MouseEvents.OnMouseDownEvent>(OnMouseDownEvent);
/// private void OnMouseDownEvent(MouseEvents.OnMouseDownEvent arg)
/// </summary>
public class InputManager : MonoBehaviour
{
  public static InputManager sharedInstance;

  public KeyCode mouseButton1 = KeyCode.Mouse0;
  public KeyCode mouseButton2 = KeyCode.Mouse1;

  public KeyCode controlKey = KeyCode.LeftControl;

  // There is no actual way to change the mouse scroll wheel,
  // what we can do is add secondary buttons for scrollwheel up and down and change those
  public string mouseScrollWheel = "Mouse ScrollWheel";

  public Vector2 mousePosition;

  private void Awake()
  {
    sharedInstance = this;
  }

  // Use this for initialization
  private void Start()
  {
  }

  // Update is called once per frame
  private void Update()
  {
    if (Input.GetKeyDown(mouseButton1))
    {
      EventManagerTypeSafe.TriggerEvent(new InputEvents.OnMouseButton1DownEvent());
    }

    if (Input.GetKeyDown(mouseButton2))
    {
      EventManagerTypeSafe.TriggerEvent(new InputEvents.OnMouseButton2DownEvent());
    }

    // Mouse Scroll
    float scrollAmount = Input.GetAxis(mouseScrollWheel);

    if (scrollAmount != 0)
    {
      EventManagerTypeSafe.TriggerEvent(new InputEvents.OnMouseScrollEvent(scrollAmount));
      if (Input.GetKey(controlKey))
      {
        EventManagerTypeSafe.TriggerEvent(new InputEvents.OnControllPressedEvent(scrollAmount));
      }
    }
  }

  public Vector2 GetScreenToWorldMousePosition()
  {
    return Camera.main.ScreenToWorldPoint(Input.mousePosition);
  }
}