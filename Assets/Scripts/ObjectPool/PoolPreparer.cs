using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terrain.Visuals;

/// <summary>
/// Creates 100 gameObjects from every tile in "Resources/Tiles" and disables them so we can use it if we need
/// </summary>
public class PoolPreparer : MonoBehaviour
{
  [SerializeField]
  public GameObject[] prefabs;

  private void Awake()
  {
    prefabs = TileDatabase.sharedInstance.databaseList.tileDatabase.ToArray();

    foreach (GameObject _prefab in prefabs)
    {
      if (_prefab == null)
      {
        Debug.LogError("Null prefab in PoolPreparer");
      }
      else
      {
        PooledMonobehaviour _poolablePrefab = _prefab.GetComponent<PooledMonobehaviour>();

        if (_poolablePrefab == null)
        {
          Debug.LogError("Prefab does not contain an IPoolable and can't be pooled");
        }
        else
        {
          Pool.GetPool(_poolablePrefab);
        }
      }
    }
  }
}