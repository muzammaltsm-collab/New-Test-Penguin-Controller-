using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinishLineTrigger : MonoBehaviour
{
    // Adjust this value to set the jump force
    public float newXPositionOffset = 0.03f; // Offset for the new x position
    public float newYPositionOffset = 0.13f; // Offset for the new Y position
    [SerializeField] GameObject L_Flag;
    [SerializeField] GameObject R_Flag;
    [SerializeField] AudioSource AS;
  
    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with this one has a "Player" tag
        if (other.CompareTag("Player"))
        {
            // Get the instance of the PlayerMovementController
            PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
            GameManager.Instance.MainCamera.target.position = GameManager.Instance.MainCamera.FinishLineCamPosition.position;
            GameManager.Instance.MainCamera.target.rotation = GameManager.Instance.MainCamera.FinishLineCamPosition.rotation;
            GameManager.Instance.MainCamera.isRotation = true;
            // Check if the instance is not null
            if (playerMovementController != null)
            {
                GameManager.Instance.SoundManager.Play__WinSound(AS);
                // Disable movement control
                playerMovementController.IsControlDisable = false;

                Vector3 currentPosition = other.transform.position;

                // Modify the x position by adding 0.03 units
                currentPosition.x = newXPositionOffset;
                currentPosition.y = newYPositionOffset;

                // Update the player's position
                other.transform.position = currentPosition;
                L_Flag.SetActive(true);
                R_Flag.SetActive(true);
               
            }
        }
    }

}
