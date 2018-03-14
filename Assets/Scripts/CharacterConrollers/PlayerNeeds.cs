using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNeeds : Character
{
  // Use this for initialization
  private void Start()
  {
    currentHealth = 100;
    currentHunger = 100;
    currentThirst = 100;

    healthBar.fillAmount = currentHealth;
  }

  // Update is called once per frame
  private void Update()
  {
    //timer += Time.deltaTime;
    //if (timer > 0.1f)
    //{
    //  timer = 0;
    //  ChangeHealth(-1);
    //  ChangeHunger(-1);
    //  ChangeThirst(-1);
    //}
  }
}