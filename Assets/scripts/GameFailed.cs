using UnityEngine;

public class GameFailed : MonoBehaviour
{
     // Adjust this value to set the jump force

    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with this one has a "Player" tag
        if (other.CompareTag("Player"))
        {
            // Call the jump function of the player
            GameManager gameManegr = GameManager.GetInstance();

            // Now you can call methods of PlayerMovementController instance
            if (gameManegr != null)
            {
                gameManegr.EnableGameFailedPanel();
            }
        }
    }
}
