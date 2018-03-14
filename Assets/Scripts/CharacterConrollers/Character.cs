using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: maybe change name to charater data and store every data from an NPC/player here
public class Character : MonoBehaviour
{
  public int currentHealth;
  public int maxHealth;

  // change this to be the opposite of hunger because hunger=100 suggest that we are hungry

  public int currentHunger;
  public int maxHunger;

  public int currentThirst;
  public int maxThirst;

  public Image healthBar;
  public Image hungerBar;
  public Image thirstBar;

  public void ChangeHealth(int amount)
  {
    if (currentHealth + amount < maxHealth)
    {
      if (currentHealth + amount > 0)
      {
        currentHealth += amount;
      }
      else
      {
        currentHealth = 0;
        // call death here
      }
    }
    else
    {
      currentHealth = maxHealth;
    }

    healthBar.fillAmount = (float)currentHealth / maxHealth;
  }

  public void ChangeHunger(int amount)
  {
    if (currentHunger + amount < maxHunger)
    {
      if (currentHunger + amount > 0)
      {
        currentHunger += amount;
      }
      else
      {
        currentHunger = 0;
        // call death here
      }
    }
    else
    {
      currentHunger = maxHunger;
    }

    hungerBar.fillAmount = (float)currentHunger / maxHunger;
  }

  public void ChangeThirst(int amount)
  {
    if (currentThirst + amount < maxThirst)
    {
      if (currentThirst + amount > 0)
      {
        currentThirst += amount;
      }
      else
      {
        currentThirst = 0;
        // call death here
      }
    }
    else
    {
      currentThirst = maxThirst;
    }

    thirstBar.fillAmount = (float)currentThirst / maxThirst;
  }
}