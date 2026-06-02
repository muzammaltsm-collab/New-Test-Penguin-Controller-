using DG.Tweening;
using UnityEngine;

// This forces Unity to automatically add the components if they are missing
[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class Pop_Up : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animDuration = 0.3f;

    private void Awake()
    {
        // Safe, early detection before any OnEnable calls
        if (panel == null) panel = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        // Double-check fallback in case Awake didn't run yet
        if (panel == null) panel = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        panel.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, animDuration * 0.5f));
        seq.Join(panel.DOScale(1f, animDuration).SetEase(Ease.OutBack));

        seq.OnComplete(() => {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void Close()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(0f, animDuration * 0.5f));
        seq.Join(panel.DOScale(0f, animDuration).SetEase(Ease.InBack));
        seq.OnComplete(() => gameObject.SetActive(false));
    }
}
