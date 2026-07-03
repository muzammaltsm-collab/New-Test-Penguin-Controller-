using System.Collections;
using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float moveSpeed = 3f;

    private bool isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(MoveHorizontalToCenter(other.gameObject));
        }
    }

    private IEnumerator MoveHorizontalToCenter(GameObject player)
    {
        PlayerMovementController movement = PlayerMovementController.GetInstance();

        if (movement != null)
        {
            movement.IsControlDisable = true;
            movement.PlayerMovementCheck = false;

            // ❌ Do NOT disable CharacterController
            // because jump / gravity needs it
        }

        while (true)
        {
            Vector3 currentPos = player.transform.position;

            Vector3 targetPos = new Vector3(
                centerPoint.position.x,
                currentPos.y,              // keep current Y
                centerPoint.position.z
            );

            if (Vector3.Distance(
                    new Vector3(currentPos.x, 0f, currentPos.z),
                    new Vector3(centerPoint.position.x, 0f, centerPoint.position.z)
                ) <= 0.05f)
            {
                break;
            }

            player.transform.position = Vector3.MoveTowards(
                currentPos,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        Vector3 finalPos = player.transform.position;
        player.transform.position = new Vector3(
            centerPoint.position.x,
            finalPos.y,                 // keep jump/fall Y
            centerPoint.position.z
        );
        // Enable movement again
        if (movement != null)
        {
            movement.IsControlDisable = false;
            movement.PlayerMovementCheck = true;
        }
    }
}