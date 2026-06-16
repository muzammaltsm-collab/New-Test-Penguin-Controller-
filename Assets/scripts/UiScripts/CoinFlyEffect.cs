using System.Collections;
using System.Collections.Generic;
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

    [Header("Pooling")]
    [SerializeField] int poolSize = 12; // should be >= coinCount, a little headroom is fine

    readonly Queue<RectTransform> pool = new Queue<RectTransform>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject coinGO = Instantiate(coinPrefab, canvas.transform);
            coinGO.SetActive(false);
            pool.Enqueue(coinGO.GetComponent<RectTransform>());
        }
    }

    RectTransform GetCoinFromPool()
    {
        RectTransform coinRT;
        if (pool.Count > 0)
        {
            coinRT = pool.Dequeue();
        }
        else
        {
            // pool exhausted (shouldn't normally happen) - grow it
            GameObject coinGO = Instantiate(coinPrefab, canvas.transform);
            coinRT = coinGO.GetComponent<RectTransform>();
        }

        coinRT.gameObject.SetActive(true);
        return coinRT;
    }

    void ReturnCoinToPool(RectTransform coinRT)
    {
        coinRT.DOKill();
        coinRT.gameObject.SetActive(false);
        pool.Enqueue(coinRT);
    }

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
        RectTransform coinRT = GetCoinFromPool();
        coinRT.localScale = Vector3.one;
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
                    ReturnCoinToPool(coinRT);
                });

            coinRT.DOScale(0.6f, flyDuration).SetEase(Ease.InQuad);
        });
    }
}