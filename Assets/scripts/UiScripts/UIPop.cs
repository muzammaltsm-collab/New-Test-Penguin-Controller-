using UnityEngine;
using UnityEngine.UI;

public class UIPop : MonoBehaviour
{
    public RectTransform coinIcon; // the UI Image GameObject for the coin icon
    public float popScale = 1.3f;
    public float popDuration = 0.15f;

    private Vector3 iconOriginalScale;
    private Coroutine popRoutine;

    void Awake()
    {
        if (coinIcon) iconOriginalScale = coinIcon.localScale;
    }

    public void Pop()
    {
        if (coinIcon == null) return;
        if (popRoutine != null) StopCoroutine(popRoutine);
        popRoutine = StartCoroutine(PopEffect());
    }

    private System.Collections.IEnumerator PopEffect()
    {
        float t = 0f;
        Vector3 iconTarget = iconOriginalScale * popScale;

        while (t < popDuration)
        {
            t += Time.deltaTime;
            coinIcon.localScale = Vector3.Lerp(iconOriginalScale, iconTarget, t / popDuration);
            yield return null;
        }

        t = 0f;
        while (t < popDuration)
        {
            t += Time.deltaTime;
            coinIcon.localScale = Vector3.Lerp(iconTarget, iconOriginalScale, t / popDuration);
            yield return null;
        }

        coinIcon.localScale = iconOriginalScale;
    }
}