using UnityEngine;
using DG.Tweening;

public class AnimationLevelFailed : MonoBehaviour
{
    [SerializeField] private RectTransform panelContainer;
    [SerializeField] private CanvasGroup backgroundDim;

    [Header("Timings & easing")]
    [SerializeField] private float openDuration = 0.5f;
    [SerializeField] private float closeDuration = 0.3f;
    [SerializeField] private float backgroundFadeDuration = 0.3f;
    [SerializeField] private Ease openEase = Ease.OutBack;
    [SerializeField] private Ease closeEase = Ease.InBack;

    private bool _isOpen;

    private void Awake()
    {
        if (panelContainer == null)
            panelContainer = GetComponent<RectTransform>();

        if (backgroundDim == null)
            backgroundDim = GetComponentInChildren<CanvasGroup>();

        if (panelContainer == null)
            Debug.LogWarning($"{nameof(AnimationLevelFailed)}: no RectTransform assigned or found on '{name}'.");

        if (backgroundDim == null)
            Debug.LogWarning($"{nameof(AnimationLevelFailed)}: no CanvasGroup assigned or found in children of '{name}'.");
    }

    public void Open()
    {
        if (_isOpen) return;
        if (panelContainer == null || backgroundDim == null)
        {
            gameObject.SetActive(true);
            _isOpen = true;
            return;
        }

        _isOpen = true;
        gameObject.SetActive(true);

        // Prepare start state
        panelContainer.localScale = Vector3.zero;
        backgroundDim.alpha = 0f;
        backgroundDim.interactable = true;
        backgroundDim.blocksRaycasts = true;

        // Kill any existing tweens for these targets
        DOTween.Kill(panelContainer);
        DOTween.Kill(backgroundDim);

        // Background fade in
        backgroundDim.DOFade(1f, backgroundFadeDuration).SetUpdate(true);

        // Panel pop-in
        panelContainer.DOScale(Vector3.one, openDuration)
                      .SetEase(openEase)
                      .OnComplete(() => panelContainer.localScale = Vector3.one);
    }

    public void Close()
    {
        if (!_isOpen) return;
        if (panelContainer == null || backgroundDim == null)
        {
            gameObject.SetActive(false);
            _isOpen = false;
            return;
        }

        _isOpen = false;

        // Prevent interaction while closing
        backgroundDim.interactable = false;
        backgroundDim.blocksRaycasts = false;

        // Kill any existing tweens for these targets
        DOTween.Kill(panelContainer);
        DOTween.Kill(backgroundDim);

        panelContainer.DOScale(Vector3.zero, closeDuration)
                      .SetEase(closeEase)
                      .OnComplete(() =>
                      {
                          panelContainer.localScale = Vector3.zero;
                          gameObject.SetActive(false);
                      });

        backgroundDim.DOFade(0f, backgroundFadeDuration).SetUpdate(true);
    }
}