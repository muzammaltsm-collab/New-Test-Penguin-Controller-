using System.Collections;
using UnityEngine;

public class BossAnimalHandler : MonoBehaviour
{
    [Header("Requirement")]
    [SerializeField] private int requiredFish = 5;

    [Header("Fish Visual")]
    [SerializeField] private GameObject fishVisualPrefab;
    [SerializeField] private Transform fishSpawnPoint;
    [SerializeField] private Transform fishBoxTarget;
    [SerializeField] private float fishThrowDuration = 0.5f;
    [SerializeField] private float fishArcHeight = 2f;
    [SerializeField] private float delayBetweenFish = 0.08f;

    [Header("Boss Path")]
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    public IEnumerator HandleBoss(PlayerFeatureScript playerFeature)
    {
        if (playerFeature == null) yield break;

        if (playerFeature.PlayerLevelAmount < requiredFish)
        {
            Debug.Log(gameObject.name + " not enough fish.");
            yield break;
        }

        yield return StartCoroutine(GiveFishToBox(playerFeature));

        if (animator != null)
        {
            animator.SetTrigger("Move");
        }

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
                    transform.forward = dir.normalized;

                yield return null;
            }
        }

        gameObject.SetActive(false);
    }

    private IEnumerator GiveFishToBox(PlayerFeatureScript playerFeature)
    {
        for (int i = 0; i < requiredFish; i++)
        {
            if (playerFeature.PlayerLevelAmount <= 0)
                yield break;

            GameObject fish = Instantiate(fishVisualPrefab, fishSpawnPoint.position, Quaternion.identity);
            StartCoroutine(MoveFishArc(fish, fishSpawnPoint.position, fishBoxTarget.position, fishThrowDuration));

            playerFeature.PlayerLevelAmount--;

            if (playerFeature.AmountShow != null)
            {
                playerFeature.AmountShow.AmountText.text = playerFeature.PlayerLevelAmount.ToString();
            }

            yield return new WaitForSeconds(delayBetweenFish);
        }

        yield return new WaitForSeconds(fishThrowDuration);
    }

    private IEnumerator MoveFishArc(GameObject fish, Vector3 start, Vector3 end, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * fishArcHeight;

            fish.transform.position = pos;

            time += Time.deltaTime;
            yield return null;
        }

        fish.transform.position = end;
        Destroy(fish);
    }
}