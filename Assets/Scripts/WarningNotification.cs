using UnityEngine;

public class WarningNotification : MonoBehaviour
{
    public CanvasGroup warningCanvasGroup;
    public Transform warningTransform;

    public float blinkInterval = 0.5f;   // Time between ON and OFF
    public float pulseScale = 1.2f;       // Maximum scale multiplier
    public float pulseSpeed = 2f;          // Speed of pulsing

    private Vector3 originalScale;
    private float blinkTimer = 0f;
    private bool isBlinking = false;

    void Start()
    {
        originalScale = warningTransform.localScale;
        StartBlinkingEffect();
    }

    public void StartBlinkingEffect()
    {
        isBlinking = true;
    }

    public void StopBlinkingEffect()
    {
        isBlinking = false;

        // Reset to normal
        warningCanvasGroup.alpha = 1f;
        warningTransform.localScale = originalScale;
    }

    void Update()
    {
        if (!isBlinking) return;

        // Blinking Effect
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            warningCanvasGroup.alpha = (warningCanvasGroup.alpha == 1f) ? 0f : 1f; // Toggle ON/OFF
            blinkTimer = 0f;
        }

        // Pulsing Effect
        float scale = 1f + Mathf.PingPong(Time.time * pulseSpeed, pulseScale - 1f);
        warningTransform.localScale = originalScale * scale;
    }
}
