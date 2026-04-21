using System.Collections;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform playerStopPoint;
    [SerializeField] private float moveToStopSpeed = 3f;

    [Header("Bosses")]
    [SerializeField] private BossAnimalHandler[] bossAnimals;

    [Header("UI")]
    [SerializeField] private GameObject nextLevelScreen;

    private bool isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        if (!other.CompareTag("Player")) return;

        isTriggered = true;
        StartCoroutine(EndSequence(other.gameObject));
    }

    private IEnumerator EndSequence(GameObject player)
    {
        PlayerMovementController movement = player.GetComponent<PlayerMovementController>();

        if (movement != null)
        {
            movement.PlayerMovementCheck = false;
            movement.IsControlDisable = false;
            movement.IsUpdateRun = false;
        }

        while (Vector3.Distance(player.transform.position, playerStopPoint.position) > 0.05f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                playerStopPoint.position,
                moveToStopSpeed * Time.deltaTime
            );

            Vector3 dir = playerStopPoint.position - player.transform.position;
            dir.y = 0f;

            if (dir != Vector3.zero)
                player.transform.forward = dir.normalized;

            yield return null;
        }

        if (GameManager.Instance.PlayerAnimation.m_Animator != null)
        {
            GameManager.Instance.PlayerAnimation.m_Animator.Play("Idle");
        }

        PlayerFeatureScript playerFeature = player.GetComponent<PlayerFeatureScript>();

        for (int i = 0; i < bossAnimals.Length; i++)
        {
            yield return StartCoroutine(bossAnimals[i].HandleBoss(playerFeature));
        }

        if (nextLevelScreen != null)
            nextLevelScreen.SetActive(true);
    }
}