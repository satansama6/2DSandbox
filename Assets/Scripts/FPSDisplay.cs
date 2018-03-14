using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add this script on Camera
//Press F1 to toggle on/off the FPS Display

public class FPSDisplay : MonoBehaviour
{
  public int test;
  public static FPSDisplay fpsDisplay;

  private float deltaTime;

  private bool showFPS = true;

  private void Awake()
  {
    UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
    if (fpsDisplay == null)
    {
      fpsDisplay = this;
    }
    else
    {
      UnityEngine.Object.Destroy(fpsDisplay);
    }
  }

  private void Update()
  {
    this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
    if (Input.GetKeyDown(KeyCode.F1))
    {
      this.showFPS = !this.showFPS;
    }
  }

  private void OnGUI()
  {
    if (!this.showFPS)
    {
      return;
    }
    int width = Screen.width;
    int height = Screen.height;
    GUIStyle gUIStyle = new GUIStyle();
    Rect position = new Rect(0f, 0f, (float)width, (float)(height * 2 / 100));
    gUIStyle.alignment = TextAnchor.UpperLeft;
    gUIStyle.fontSize = height * 2 / 70;
    gUIStyle.normal.textColor = Color.white;
    float num = this.deltaTime * 1000f;
    float num2 = 1f / this.deltaTime;
    string text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
    GUI.Label(position, text, gUIStyle);
  }
}