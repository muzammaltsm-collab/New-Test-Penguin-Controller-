using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossAnimalHandler : MonoBehaviour
{
    [Header("Requirement")]
    [SerializeField] private int requiredFish = 5;

    [Header("Boss Header Text")]
    [SerializeField] private TMP_Text bossRequiredText;
    [SerializeField] private GameObject amountTextOBJ;

    [Header("Fish Visual Prefabs")]
    [SerializeField] private GameObject blueFishPrefab;
    [SerializeField] private GameObject goldenFishPrefab;

    [Header("Fish Transfer Points")]
    [SerializeField] private Transform fishSpawnPoint;
    [SerializeField] private Transform fishBoxTarget;

    [Header("Fish Transfer Visual Settings")]
    [SerializeField] private float smallAmountThrowDuration = 0.45f;
    [SerializeField] private float largeAmountThrowDuration = 0.22f;
    [SerializeField] private float smallAmountDelay = 0.09f;
    [SerializeField] private float largeAmountDelay = 0.03f;
    [SerializeField] private float fishArcHeight = 2.5f;

    [Header("Golden Fish Visual Settings")]
    [SerializeField] private float goldenArcMultiplier = 1.5f;
    [SerializeField] private float goldenScaleMultiplier = 1.25f;

    [Header("Audio")]
    [SerializeField] private AudioSource fishAudioSource;
    [SerializeField] private AudioClip blueFishClip;
    [SerializeField] private AudioClip goldenFishClip;

    [Header("Impact Effects")]
    [SerializeField] private Transform fishImpactTarget;
    [SerializeField] private GameObject goldenImpactEffectPrefab;
    [SerializeField] private float targetPunchScale = 0.15f;
    [SerializeField] private float targetPunchDuration = 0.12f;

    [Header("Boss Path")]
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float moveSpeed = 8f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string moveAnimationName = "Walk";

    public bool WasCompleted { get; private set; }
    public bool HadEnoughFish { get; private set; }

    private int currentBossRemaining;


    private void Start()
    {
        bossRequiredText.text = requiredFish.ToString();
    }

    [Serializable]
    private class FishTransferStep
    {
        public GameObject prefab;
        public int value;
        public bool isGolden;

        public FishTransferStep(GameObject prefab, int value, bool isGolden)
        {
            this.prefab = prefab;
            this.value = value;
            this.isGolden = isGolden;
        }
    }

    public IEnumerator HandleBoss(PlayerFeatureScript playerFeature)
    {
        WasCompleted = false;
        HadEnoughFish = false;

        if (playerFeature == null)
            yield break;

        int playerFish = playerFeature.PlayerLevelAmount;
        HadEnoughFish = playerFish >= requiredFish;

        currentBossRemaining = requiredFish;
        UpdateBossRequirementText();

        int transferAmount = Mathf.Min(playerFish, requiredFish);

        if (transferAmount > 0)
        {
            yield return StartCoroutine(GiveFishToBoxMixed(playerFeature, transferAmount));
        }

        if (!HadEnoughFish)
            yield break;

        if (animator != null)
        {
            animator.Play(moveAnimationName);
        }

        yield return StartCoroutine(MoveBossAlongPath());

        WasCompleted = true;
        gameObject.SetActive(false);
    }

    private IEnumerator GiveFishToBoxMixed(PlayerFeatureScript playerFeature, int transferAmount)
    {
        List<FishTransferStep> steps = BuildFishTransferSteps(transferAmount);

        if (steps == null || steps.Count == 0)
            yield break;

        int landedCount = 0;

        float t = Mathf.InverseLerp(10f, 500f, transferAmount);
        float throwDuration = Mathf.Lerp(smallAmountThrowDuration, largeAmountThrowDuration, t);
        float delayBetweenSteps = Mathf.Lerp(smallAmountDelay, largeAmountDelay, t);

        for (int i = 0; i < steps.Count; i++)
        {
            FishTransferStep step = steps[i];

            if (step.prefab != null && fishSpawnPoint != null && fishBoxTarget != null)
            {
                GameObject fish = Instantiate(step.prefab, fishSpawnPoint.position, Quaternion.identity);

                if (step.isGolden)
                {
                    fish.transform.localScale *= goldenScaleMultiplier;
                }

                StartCoroutine(MoveFishArc(
                    fish,
                    fishSpawnPoint.position,
                    fishBoxTarget.position,
                    throwDuration,
                    step.isGolden,
                    () =>
                    {
                        ApplyTransferStep(playerFeature, step.value);
                        PlayFishLandingSound(step.isGolden);
                        PlayGoldenImpact(step.isGolden);
                        landedCount++;
                    }));
            }
            else
            {
                ApplyTransferStep(playerFeature, step.value);
                PlayFishLandingSound(step.isGolden);
                PlayGoldenImpact(step.isGolden);
                landedCount++;
            }

            yield return new WaitForSeconds(delayBetweenSteps);
        }

        while (landedCount < steps.Count)
        {
            yield return null;
        }
    }

    private List<FishTransferStep> BuildFishTransferSteps(int transferAmount)
    {
        List<FishTransferStep> steps = new List<FishTransferStep>();

        if (transferAmount <= 0)
            return steps;

        int remainingAmount = transferAmount;
        GameObject goldenPrefabToUse = goldenFishPrefab != null ? goldenFishPrefab : blueFishPrefab;

        while (remainingAmount > 0)
        {
            int blueCount = Mathf.Min(5, remainingAmount);

            if (remainingAmount <= 5)
            {
                for (int i = 0; i < remainingAmount; i++)
                {
                    steps.Add(new FishTransferStep(blueFishPrefab, 1, false));
                }

                remainingAmount = 0;
                break;
            }

            for (int i = 0; i < blueCount; i++)
            {
                steps.Add(new FishTransferStep(blueFishPrefab, 1, false));
            }

            remainingAmount -= blueCount;

            if (remainingAmount <= 0)
                break;

            int goldenValue = Mathf.Min(20, remainingAmount);
            steps.Add(new FishTransferStep(goldenPrefabToUse, goldenValue, true));

            remainingAmount -= goldenValue;
        }

        return steps;
    }

    private void ApplyTransferStep(PlayerFeatureScript playerFeature, int stepValue)
    {
        playerFeature.RemoveFish(stepValue);

        currentBossRemaining -= stepValue;
        if (currentBossRemaining < 0)
            currentBossRemaining = 0;

        UpdateBossRequirementText();
    }

    private void UpdateBossRequirementText()
    {
        if (bossRequiredText == null)
            return;

        if (currentBossRemaining <= 0)
        {
            bossRequiredText.text = "";
            amountTextOBJ.SetActive(false);
        }
        else
        {
            bossRequiredText.text = currentBossRemaining.ToString();
        }
    }

    private void PlayFishLandingSound(bool isGolden)
    {
        if (fishAudioSource == null)
            return;

        AudioClip clipToPlay = isGolden ? goldenFishClip : blueFishClip;

        if (clipToPlay != null)
        {
            fishAudioSource.PlayOneShot(clipToPlay);
        }
    }

    private void PlayGoldenImpact(bool isGolden)
    {
        if (!isGolden)
            return;

        Transform target = fishImpactTarget != null ? fishImpactTarget : fishBoxTarget;

        if (target == null)
            return;

        if (goldenImpactEffectPrefab != null)
        {
            GameObject fx = Instantiate(goldenImpactEffectPrefab, target.position, Quaternion.identity);
            Destroy(fx, 1.5f);
        }

        StartCoroutine(PunchTargetScale(target));
    }

    private IEnumerator PunchTargetScale(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 punchScale = originalScale + Vector3.one * targetPunchScale;

        float halfDuration = targetPunchDuration * 0.5f;
        float time = 0f;

        while (time < halfDuration)
        {
            time += Time.deltaTime;
            float t = time / halfDuration;
            target.localScale = Vector3.Lerp(originalScale, punchScale, t);
            yield return null;
        }

        time = 0f;

        while (time < halfDuration)
        {
            time += Time.deltaTime;
            float t = time / halfDuration;
            target.localScale = Vector3.Lerp(punchScale, originalScale, t);
            yield return null;
        }

        target.localScale = originalScale;
    }

    private IEnumerator MoveBossAlongPath()
    {
        if (movePoints == null || movePoints.Length == 0)
            yield break;

        for (int i = 0; i < movePoints.Length; i++)
        {
            while (Vector3.Distance(transform.position, movePoints[i].position) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    movePoints[i].position,
                    moveSpeed * Time.deltaTime
                );

                Vector3 dir = movePoints[i].position - transform.position;
                dir.y = 0f;

                if (dir != Vector3.zero)
                {
                    transform.forward = dir.normalized;
                }

                yield return null;
            }
        }
    }

    private IEnumerator MoveFishArc(
        GameObject fish,
        Vector3 start,
        Vector3 end,
        float duration,
        bool isGolden,
        Action onReached)
    {
        float time = 0f;
        float arcHeight = isGolden ? fishArcHeight * goldenArcMultiplier : fishArcHeight;

        while (time < duration)
        {
            float normalized = time / duration;

            Vector3 pos = Vector3.Lerp(start, end, normalized);
            pos.y += Mathf.Sin(normalized * Mathf.PI) * arcHeight;

            fish.transform.position = pos;

            time += Time.deltaTime;
            yield return null;
        }

        fish.transform.position = end;
        onReached?.Invoke();
        Destroy(fish);
    }
}