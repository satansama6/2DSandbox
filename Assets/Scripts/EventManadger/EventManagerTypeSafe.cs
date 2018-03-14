using System.Collections.Generic;
using UnityEngine;
using TypeSafeEventManager.Events;

//empty class for storing the events later
public class GameEvent { }

public class EventManagerTypeSafe : MonoBehaviour
{
  private static EventManagerTypeSafe instanceInternal;

  public static EventManagerTypeSafe m_Instance
  {
    get
    {
      if (instanceInternal == null)
      {
        instanceInternal = new EventManagerTypeSafe();
      }
      return instanceInternal;
    }
  }

  public delegate void m_EventDelegate<T>(T myEvent) where T : GameEvent;

  private delegate void m_EventDelegate(GameEvent myEvent);

  private static Dictionary<System.Type, m_EventDelegate> m_Delegates = new Dictionary<System.Type, m_EventDelegate>();
  private static Dictionary<System.Delegate, m_EventDelegate> m_DelegateLookup = new Dictionary<System.Delegate, m_EventDelegate>();

  public static void AddListener<T>(m_EventDelegate<T> myDelegate) where T : GameEvent
  {
    //If we have already registered this delegate then out.
    if (m_DelegateLookup.ContainsKey(myDelegate))
    {
      return;
    }
    //Create a new non-generic delegate which calls our generic one.
    //This is the delegate we actually invoke.
    m_EventDelegate internalDelegate = (myEvent) => myDelegate((T)myEvent);
    m_DelegateLookup[myDelegate] = internalDelegate;

    m_EventDelegate tempDelegate;
    if (m_Delegates.TryGetValue(typeof(T), out tempDelegate))
    {
      m_Delegates[typeof(T)] = tempDelegate += internalDelegate;
    }
    else
    {
      m_Delegates[typeof(T)] = internalDelegate;
    }
  }

  public static void RemoveListener<T>(m_EventDelegate<T> myDelegate) where T : GameEvent
  {
    m_EventDelegate internalDelegate;
    if (m_DelegateLookup.TryGetValue(myDelegate, out internalDelegate))
    {
      m_EventDelegate tempDelegate;
      if (m_Delegates.TryGetValue(typeof(T), out tempDelegate))
      {
        tempDelegate -= internalDelegate;
        if (tempDelegate == null)
        {
          m_Delegates.Remove(typeof(T));
        }
        else
        {
          m_Delegates[typeof(T)] = tempDelegate;
        }
      }
      m_DelegateLookup.Remove(myDelegate);
    }
  }

  public static void TriggerEvent(GameEvent myEvent)
  {
    m_EventDelegate myDelegate;
    if (m_Delegates.TryGetValue(myEvent.GetType(), out myDelegate))
    {
      myDelegate.Invoke(myEvent);
    }
  }
}