using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public LevelManager lm;

    // Start is called before the first frame update
    void Start()
    {
        if (lm == null) lm = GameObject.FindObjectOfType<LevelManager>();
        currentHealth = startingHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int dmg)
    {
        if (currentHealth > 0) currentHealth -= dmg;

        if (currentHealth <= 0) PlayerDies();

        healthBar.value = currentHealth;
    }

    public void TakeHealth(int health)
    {
        if (currentHealth < 100)
        {
            currentHealth += health;
            healthBar.value = Mathf.Clamp(currentHealth, 0, startingHealth);
        }
    }

    void PlayerDies()
    {
        lm.LevelLost();
    }
}
