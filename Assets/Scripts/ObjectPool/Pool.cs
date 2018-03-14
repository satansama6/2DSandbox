using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
  private static Dictionary<PooledMonobehaviour, Pool> m_Pools = new Dictionary<PooledMonobehaviour, Pool>();
  private Queue<PooledMonobehaviour> m_Objects = new Queue<PooledMonobehaviour>();
  private List<PooledMonobehaviour> m_DisabledObjects = new List<PooledMonobehaviour>();

  private PooledMonobehaviour m_Prefab;

  public static Pool m_SharedInstance;

  private void Awake()
  {
    m_SharedInstance = this;
  }

  public static Pool GetPool(PooledMonobehaviour prefab)
  {
    if (m_Pools.ContainsKey(prefab))
    {
      return m_Pools[prefab];
    }
    Pool _pool = new GameObject("Pool-" + (prefab as Component).name).AddComponent<Pool>();
    _pool.m_Prefab = prefab;
    _pool.GrowPool();
    _pool.transform.parent = GameObject.Find("Pool").transform;

    m_Pools.Add(prefab, _pool);
    return _pool;
  }

  private int totalPooledObjects = 0;

  public void GrowPool()
  {
    for (int i = 0; i < m_Prefab.m_InitialPoolSize; i++)
    {
      PooledMonobehaviour _pooledObject = Instantiate(this.m_Prefab) as PooledMonobehaviour;

      (_pooledObject as Component).gameObject.name += " " + totalPooledObjects++;
      _pooledObject.OnDestroyEvent += () => AddObjectToAvalaible(_pooledObject);

      (_pooledObject as Component).gameObject.SetActive(false);
    }
  }

  public void AddObjectToAvalaible(PooledMonobehaviour pooledObject)
  {
    m_DisabledObjects.Add(pooledObject);
    m_Objects.Enqueue(pooledObject);
  }

  public T Get<T>() where T : PooledMonobehaviour
  {
    MakeDisabledObjectsChildren();
    if (m_Objects.Count == 0)
    {
      GrowPool();
    }

    PooledMonobehaviour _pooledObject = m_Objects.Dequeue();
    return _pooledObject as T;
  }

  private void MakeDisabledObjectsChildren()
  {
    if (m_DisabledObjects.Count > 0)
    {
      foreach (PooledMonobehaviour pooledObject in m_DisabledObjects)
      {
        if (pooledObject.gameObject.activeInHierarchy == false)
        {
          (pooledObject as Component).transform.SetParent(transform);
        }
      }

      m_DisabledObjects.Clear();
    }
  }

  private void Update()
  {
    MakeDisabledObjectsChildren();
  }
}