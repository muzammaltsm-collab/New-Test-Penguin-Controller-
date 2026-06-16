using System.Collections;
using UnityEngine;
using DG.Tweening;
public class CoinFlyEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject coinPrefab;       // coin UI prefab asset (Image-based)
    [SerializeField] RectTransform spawnPoint;    // No Thanks button RectTransform
    [SerializeField] RectTransform targetPoint;   // coin counter icon RectTransform
    [SerializeField] Canvas canvas;               // the single scene Canvas
    [SerializeField] UIPop coinPopEffect;         // pops the counter icon when a coin lands

    [Header("Burst Settings")]
    [SerializeField] int coinCount = 8;
    [SerializeField] float burstRadius = 120f;
    [SerializeField] float burstDuration = 0.3f;
    [SerializeField] float holdDuration = 0.5f;
    [SerializeField] float staggerDelay = 0.05f;

    [Header("Fly-To-Target Settings")]
    [SerializeField] float flyDuration = 0.6f;
    [SerializeField] float arcHeight = 80f;

    public void PlayCoinFly(System.Action onComplete = null)
    {
        StartCoroutine(SpawnCoinsRoutine(onComplete));
    }

    IEnumerator SpawnCoinsRoutine(System.Action onComplete)
    {
        int remaining = coinCount;

        for (int i = 0; i < coinCount; i++)
        {
            float angle = (360f / coinCount) * i;
            SpawnSingleCoin(angle, () =>
            {
                remaining--;
                if (remaining <= 0)
                    onComplete?.Invoke();
            });

            yield return new WaitForSeconds(staggerDelay);
        }
    }

    void SpawnSingleCoin(float angleDegrees, System.Action onCoinComplete)
    {
        GameObject coinGO = Instantiate(coinPrefab, canvas.transform);
        RectTransform coinRT = coinGO.GetComponent<RectTransform>();
        coinRT.position = spawnPoint.position;

        float rad = angleDegrees * Mathf.Deg2Rad;
        Vector3 burstOffset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * burstRadius;
        Vector3 burstPos = spawnPoint.position + burstOffset;

        Sequence seq = DOTween.Sequence();

        seq.Append(coinRT.DOMove(burstPos, burstDuration).SetEase(Ease.OutQuad));
        seq.AppendInterval(holdDuration);

        seq.AppendCallback(() =>
        {
            Vector3 startPos = coinRT.position;
            Vector3 endPos = targetPoint.position;
            Vector3 midPos = Vector3.Lerp(startPos, endPos, 0.5f) + Vector3.up * arcHeight;
            Vector3[] path = new Vector3[] { startPos, midPos, endPos };

            coinRT.DOPath(path, flyDuration, PathType.CatmullRom)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    if (coinPopEffect != null) coinPopEffect.Pop();
                    onCoinComplete?.Invoke();
                    Destroy(coinGO);
                });

            coinRT.DOScale(0.6f, flyDuration).SetEase(Ease.InQuad);
        });
    }
}