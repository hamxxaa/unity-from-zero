using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image fillImage;
    private Health healthScript;
    void Awake()
    {
        healthScript = GetComponentInParent<Health>();
    }

    void OnEnable()
    {
        if (healthScript != null)
            healthScript.OnHealthChanged += UpdateBar;
    }

    void OnDisable()
    {
        if (healthScript != null)
            healthScript.OnHealthChanged -= UpdateBar;
    }

    void UpdateBar(float pct)
    {
        fillImage.fillAmount = pct;
    }
}