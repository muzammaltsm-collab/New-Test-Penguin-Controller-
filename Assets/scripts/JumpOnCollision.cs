using UnityEngine;

public class JumpOnCollision : MonoBehaviour
{
    [SerializeField] AudioSource AS;
    [SerializeField] bool isEndPointJump;
    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with this one has a "Player" tag
        if (other.CompareTag("Player"))
        {
            // Call the jump function of the player
            PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
            GameManager.Instance.SoundManager.Play_JumpSound(AS);
            // Now you can call methods of PlayerMovementController instance
            if (playerMovementController != null)
            {
                playerMovementController.Jump();
                if (GameManager.Instance.PlayerAnimation.m_Animator != null)
                {
                    GameManager.Instance.PlayerAnimation.m_Animator.Play("Jump");

                    playerMovementController.isRotation = true;
                }
                playerMovementController.isGrounded = false;

            }
        }

        {

        }
    }
}
