using UnityEngine;
using TMPro;

public class NeonPulse : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 2f;
    [SerializeField] float minGlow = 0.2f;
    [SerializeField] float maxGlow = 1f;

    [Header("References")]
    [SerializeField] TextMeshProUGUI targetText;

    private Material fontMat;

    void Awake()
    {
        if (targetText == null) targetText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (targetText != null)
        {
            fontMat = targetText.fontMaterial;
        }
    }

    void Update()
    {
        if (fontMat != null)
        {
            float sineWave = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

            float currentGlow = Mathf.Lerp(minGlow, maxGlow, sineWave);

            fontMat.SetFloat(ShaderUtilities.ID_GlowPower, currentGlow);
        }
    }
}