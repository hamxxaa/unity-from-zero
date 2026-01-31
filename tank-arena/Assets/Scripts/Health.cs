using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Ayarlar")]
    [SerializeField] int maxHealth = 100;

    [Header("Efektler")]
    [SerializeField] GameObject deathEffect;

    public int CurrentHealth { get; private set; }

    // Events
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        float healthPercent = (float)CurrentHealth / maxHealth;
        OnHealthChanged?.Invoke(healthPercent);
        SoundManager.Instance.PlaySound(SoundManager.Instance.hitSound, 0.6f);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();

        SoundManager.Instance.PlaySound(SoundManager.Instance.explosionSound, 1f);

        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);

            Destroy(effect, 2f);
        }

        if (gameObject.CompareTag("Player"))
        {
            CameraShake.Instance?.Shake(0.5f, 1f);
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (var r in renderers) r.enabled = false;

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (var c in colliders) c.enabled = false;

            MonoBehaviour[] scripts = GetComponentsInChildren<MonoBehaviour>();
            foreach (var s in scripts) s.enabled = false;
        }
        else
        {
            CameraShake.Instance?.Shake(0.1f, 0.3f);
            Destroy(gameObject);
        }
    }

    public void HealFull()
    {
        CurrentHealth = maxHealth;

        OnHealthChanged?.Invoke(1f);
    }
}