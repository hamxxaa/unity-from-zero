using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float maxHealth = 100f;
    [SerializeField] bool isBase = false; 
    [SerializeField] HealthBar healthBar;

    private float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            if (isBase)
            {
                GameManager.Instance.TriggerGameOver();
            }
            else
            {
                Die();
            }
        }
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    void Die()
    {
        Destroy(gameObject);
    }


    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
}