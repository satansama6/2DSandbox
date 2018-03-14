using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenClosePanel : MonoBehaviour
{
  public KeyCode openCloseButton;

  private bool isOpen = false;

  // Update is called once per frame
  private void Update()
  {
    if (Input.GetKeyDown(openCloseButton))
    {
      if (isOpen)
      {
        isOpen = false;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(-1000, 0, 0);
      }
      else
      {
        isOpen = true;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
      }
    }
  }
}