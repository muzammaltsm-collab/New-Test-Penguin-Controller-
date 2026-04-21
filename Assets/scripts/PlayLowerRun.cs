using UnityEngine;

public class PlayLowerRun : MonoBehaviour
{
     // Adjust this value to set the jump force

    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with this one has a "Player" tag
        if (other.CompareTag("Player"))
        {
            // Call the jump function of the player
            PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();

            // Now you can call methods of PlayerMovementController instance
            if (playerMovementController != null)
            {
                playerMovementController.isRotation = false;
                playerMovementController.PlayLowerRunAnimation();
            }
        }
    }
}
