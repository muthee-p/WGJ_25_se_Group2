using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public static HealthScript instance;
    [SerializeField] private Slider healthBar;
    public float maxHealth;
    float healthLoss = 10;
    float currentHealth;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        SetHealth(currentHealth);

        StartCoroutine(DrainHealth());
    }

    void SetHealth(float health)
    {
        healthBar.value = health;
    }

    IEnumerator DrainHealth()
    {
        while (currentHealth > 0)
        {
            currentHealth -= healthLoss;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            healthBar.value = currentHealth;

            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("Player is fainted!");
    }
    
    public void RefillHealth()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

}
