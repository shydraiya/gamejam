using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Defaults")]
    [SerializeField] private float defaultDuration = 0.12f;
    [SerializeField] private float defaultAmplitude = 0.15f;
    [SerializeField] private float defaultFrequency = 25f;
    [SerializeField] private bool useUnscaledTime = true;

    private Vector3 originalLocalPos;
    private Coroutine shakeCo;

    void Awake()
    {
        originalLocalPos = transform.localPosition;
    }

    void OnDisable()
    {
        if (shakeCo != null) StopCoroutine(shakeCo);
        transform.localPosition = originalLocalPos;
        shakeCo = null;
    }

    public void Shake(float amplitude = -1f, float duration = -1f, float frequency = -1f)
    {
        Debug.Log($"shakeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
        if (amplitude < 0f) amplitude = defaultAmplitude;
        if (duration < 0f) duration = defaultDuration;
        if (frequency < 0f) frequency = defaultFrequency;

        if (shakeCo != null) StopCoroutine(shakeCo);
        shakeCo = StartCoroutine(ShakeRoutine(amplitude, duration, frequency));
    }

    private IEnumerator ShakeRoutine(float amplitude, float duration, float frequency)
    {
        float t = 0f;
        float seed = Random.value * 1000f;

        while (t < duration)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;

            // 0→1 감쇠(처음 세고 끝에 약하게)
            float k = 1f - Mathf.Clamp01(t / duration);
            float a = amplitude * k;

            float nx = (Mathf.PerlinNoise(seed, t * frequency) - 0.5f) * 2f;
            float ny = (Mathf.PerlinNoise(seed + 17.3f, t * frequency) - 0.5f) * 2f;

            transform.localPosition = originalLocalPos + new Vector3(nx, ny, 0f) * a;

            yield return null;
        }

        transform.localPosition = originalLocalPos;
        shakeCo = null;
    }
}
