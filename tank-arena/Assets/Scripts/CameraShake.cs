using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Singleton
    public static CameraShake Instance;

    private Vector3 originalPos;
    private float shakeTimer;

    private Coroutine currentShakeRoutine;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        if (currentShakeRoutine != null) StopCoroutine(currentShakeRoutine);
        currentShakeRoutine = StartCoroutine(DoShake(duration, magnitude));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}