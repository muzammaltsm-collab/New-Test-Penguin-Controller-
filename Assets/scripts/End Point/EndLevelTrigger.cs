using Sound;
using System.Collections;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    [Header("Player Stop")]
    [SerializeField] private Transform playerStopPoint;
    [SerializeField] private float moveToStopSpeed = 3f;

    [Header("Player Path To Igloo")]
    [SerializeField] private Transform[] playerPathPoints;
    [SerializeField] private float playerPathMoveSpeed = 4f;

    [Header("Current Boss")]
    [SerializeField] private BossAnimalHandler currentBoss;

    private bool isTriggered = false;
    [SerializeField] AudioSource AS;
    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(EndSequence(other.gameObject));
        }
    }

    private IEnumerator EndSequence(GameObject player)
    {
        PlayerMovementController movement = PlayerMovementController.GetInstance();
        PlayerFeatureScript playerFeature = GameManager.Instance._playerFeatureScript;

        if (movement != null)
        {
            movement.PlayerMovementCheck = false;
            movement.IsControlDisable = false;
            movement.IsUpdateRun = false;
            movement.isDealingWithHurdle = true;

            if (movement.characterController != null)
            {
                movement.characterController.enabled = false;
            }
        }

        yield return StartCoroutine(MovePlayerToStopPoint(player));

        if (GameManager.Instance.PlayerAnimation.m_Animator != null)
        {
            GameManager.Instance.PlayerAnimation.m_Animator.Play("Idle");
        }

        if (currentBoss == null)
            yield break;

        yield return StartCoroutine(currentBoss.HandleBoss(playerFeature));

        if (currentBoss.WasCompleted)
        {
            if (GameManager.Instance.PlayerAnimation.m_Animator != null)
            {
                GameManager.Instance.PlayerAnimation.m_Animator.Play("Run");
            }

            yield return StartCoroutine(MovePlayerAlongPath(player));

            if (GameManager.Instance.PlayerAnimation.m_Animator != null)
            {
                GameManager.Instance.PlayerAnimation.m_Animator.Play("Idle");
            }

            GameManager.Instance.UI.RetryPanel.SetActive(true);
        }
        else
        {
            if (GameManager.Instance.PlayerAnimation.m_Animator != null)
            {
                GameManager.Instance.PlayerAnimation.m_Animator.Play("Die");
            }

            yield return new WaitForSeconds(2f);
            GameManager.Instance.SoundManager.Play_PlayerDeathSound(AS);
            GameManager.Instance.UI.Levelfailed.SetActive(true);
        }
    }

    private IEnumerator MovePlayerToStopPoint(GameObject player)
    {
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
            {
                player.transform.forward = dir.normalized;
            }

            yield return null;
        }

        player.transform.position = playerStopPoint.position;
        player.transform.rotation = playerStopPoint.rotation;
    }

    private IEnumerator MovePlayerAlongPath(GameObject player)
    {
        if (playerPathPoints == null || playerPathPoints.Length == 0)
            yield break;

        for (int i = 0; i < playerPathPoints.Length; i++)
        {
            while (Vector3.Distance(player.transform.position, playerPathPoints[i].position) > 0.05f)
            {
                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    playerPathPoints[i].position,
                    playerPathMoveSpeed * Time.deltaTime
                );

                Vector3 dir = playerPathPoints[i].position - player.transform.position;
                dir.y = 0f;

                if (dir != Vector3.zero)
                {
                    player.transform.forward = dir.normalized;
                }

                yield return null;
            }
        }
    }
}