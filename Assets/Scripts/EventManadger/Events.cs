using UnityEngine;

namespace TypeSafeEventManager.Events
{
  //this class contains all events for our typesafe eventmanager.
  public static class Events
  {
  }

  public static class UIEvents
  {
    // Maybe we want to have a separete event for add, and one for remove
    public class OnItemAddedOrRemoved : GameEvent { }
  }

  public static class InputEvents
  {
    public class OnMouseButton1DownEvent : GameEvent { }

    public class OnMouseButton2DownEvent : GameEvent { }

    public class OnMouseScrollEvent : GameEvent
    {
      public float delta;

      public OnMouseScrollEvent(float delta)
      {
        this.delta = delta;
      }
    }

    public class OnControllPressedEvent : GameEvent
    {
      public float delta;

      public OnControllPressedEvent(float delta)
      {
        this.delta = delta;
      }
    }
  }
}