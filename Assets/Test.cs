using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
  private byte a = 11;

  private void Start()
  {
    byte b = 2;

    if ((a & b) != 0)
    {
      Debug.Log((a & b));
    }
  }

  private void Update()
  {
  }
}