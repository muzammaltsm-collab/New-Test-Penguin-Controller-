using DG.Tweening;
using UnityEngine;

// This forces Unity to automatically add the components if they are missing
[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class UI_Scale_Up : MonoBehaviour
{
    [SerializeField] private RectTransform panel;      //Use serialize field for inspector assignment, but fallback to GetComponent if not set
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animDuration = 0.3f;

 

    private void OnEnable()
    {
        // Double-check fallback in case Awake didn't run yet
        if (panel == null) panel = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        panel.localScale = Vector3.zero;

        canvasGroup.blocksRaycasts = false;

        Sequence seq = DOTween.Sequence();
      
        seq.Join(panel.DOScale(1f, animDuration).SetEase(Ease.OutBack)); 

        seq.OnComplete(() =>
        { 
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void Close()
    {
        Sequence seq = DOTween.Sequence();  // Create a new sequence for closing animation
        
        seq.Join(panel.DOScale(0f, animDuration).SetEase(Ease.InBack));
        seq.OnComplete(() => gameObject.SetActive(false));
    }
}
