using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PooledMonobehaviour : MonoBehaviour
{
  [SerializeField]
  public int m_InitialPoolSize = 100;

  public event Action OnDestroyEvent;

  protected virtual void OnDisable()
  {
    if (OnDestroyEvent != null)
    {
      OnDestroyEvent();
    }
  }

  public T Get<T>(bool enable = true) where T : PooledMonobehaviour
  {
    Pool _pool = Pool.GetPool(this);
    T _pooledObject = _pool.Get<T>();

    if (enable)
    {
      _pooledObject.gameObject.SetActive(true);
    }
    return _pooledObject;
  }

  public T Get<T>(Transform parent, bool resetTransform = false) where T : PooledMonobehaviour
  {
    var pooledObject = Get<T>(true);
    pooledObject.transform.SetParent(parent);

    if (resetTransform)
    {
      pooledObject.transform.localPosition = Vector3.zero;
      pooledObject.transform.localRotation = Quaternion.identity;
    }

    return pooledObject;
  }

  public T Get<T>(Transform parent, Vector3 relativePosition, Quaternion relativeRotation) where T : PooledMonobehaviour
  {
    var pooledObject = Get<T>(true);
    pooledObject.transform.SetParent(parent);

    pooledObject.transform.localPosition = relativePosition;
    pooledObject.transform.localRotation = relativeRotation;

    return pooledObject;
  }
}