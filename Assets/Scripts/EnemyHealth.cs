using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public AudioClip gotHitSFX;

    private void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthBar.maxValue = startingHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int dmg)
    {
        if (currentHealth > 0) currentHealth -= dmg;

        healthBar.value = currentHealth;
    }

    public void TakeHealth(int health)
    {
        if (currentHealth < startingHealth)
        {
            currentHealth += health;
            healthBar.value = Mathf.Clamp(currentHealth, 0, startingHealth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(LevelManager.playerAxeDamage);
            AudioSource.PlayClipAtPoint(gotHitSFX, Camera.main.transform.position);
        }
    }
}

