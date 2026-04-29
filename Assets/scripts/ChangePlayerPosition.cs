using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPosition : MonoBehaviour
{
    [SerializeField] Transform NewPosition;
    [SerializeField] bool isPlayerControllesDisable;
    [SerializeField] AudioSource AS;
    [SerializeField] bool isPortalIn = false;
    void OnTriggerEnter(Collider other)
    {

        // Check if the object colliding with this one has a "Player" tag
        if (other.CompareTag("Player"))
        {
            if (isPortalIn)
            {

                GameManager.Instance.SoundManager.Play__PortalInSound(AS);
            }
            else
            {
                GameManager.Instance.SoundManager.Play__PortalOutSound(AS);
            }
            other.transform.position = NewPosition.position;
        }
        if (isPlayerControllesDisable)
        {
            PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();

            // Check if the instance is not null
            if (playerMovementController != null)
            {
                // Disable movement control
                playerMovementController.IsControlDisable = false;
            }
        }
        // Update is called once per frame

    }
}