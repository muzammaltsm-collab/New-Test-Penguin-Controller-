using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDeath : MonoBehaviour
{
    const string k_PlayerTag = "Player";
    bool isFalling = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(k_PlayerTag) || isFalling)
            return;

        isFalling = true;

        // Freeze player movement
        PlayerMovementController movement = PlayerMovementController.GetInstance();

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

        // Play death animation
        if (GameManager.Instance != null &&
            GameManager.Instance.PlayerAnimation != null &&
            GameManager.Instance.PlayerAnimation.m_Animator != null)
        {
            GameManager.Instance.PlayerAnimation.m_Animator.Play("Die");
            // mark player as dead so other scripts won't force Run again
            GameManager.Instance.IsPlayerDead = true;
        }

        // Disable level / gameplay objects if you have a level parent
        // Replace this with your actual level object if needed.
        // Example:
        // GameManager.Instance.CurrentLevel.SetActive(false);

        // Show failed panel
        if (GameManager.Instance != null &&
            GameManager.Instance.UI != null &&
            GameManager.Instance.UI.StagefailedPanel != null)
        {
            GameManager.Instance.UI.StagefailedPanel.SetActive(true);
        }

    }
}
