using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, ColorUsage(true, true)] Color flashColor = Color.white;
    [SerializeField] float flashDuration = 0.1f;
    [SerializeField] float flashIntensity = 5f;

    [Header("References")]
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] Health healthScript;

    private MaterialPropertyBlock propBlock;
    private Coroutine flashRoutine;

    // Cache the Emission ID in the shader for performance
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
    private static readonly int MainColorID = Shader.PropertyToID("_Color");

    void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        if (healthScript == null) healthScript = GetComponent<Health>();
        if (meshRenderers == null || meshRenderers.Length == 0)
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    void OnEnable()
    {
        if (healthScript) healthScript.OnHealthChanged += Flash;
    }

    void OnDisable()
    {
        if (healthScript) healthScript.OnHealthChanged -= Flash;
    }

    void Flash(float healthPercent)
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        Color finalFlashColor = flashColor * flashIntensity;

        for (int i = 0; i < 3; i++)
        {
            foreach (var r in meshRenderers)
            {
                r.GetPropertyBlock(propBlock);

                propBlock.SetColor(BaseColorID, flashColor);
                propBlock.SetColor(MainColorID, flashColor);
                propBlock.SetColor(EmissionColorID, finalFlashColor);

                r.material.EnableKeyword("_EMISSION");

                r.SetPropertyBlock(propBlock);
            }

            yield return new WaitForSeconds(flashDuration);

            foreach (var r in meshRenderers)
            {
                r.GetPropertyBlock(propBlock);
                propBlock.Clear();
                r.SetPropertyBlock(propBlock);
            }

            yield return new WaitForSeconds(flashDuration / 2);
        }
    }
}