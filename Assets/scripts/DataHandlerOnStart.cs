using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandlerOnStart : MonoBehaviour
{
    [SerializeField] Transform PlayerStartingPosition;
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {

            playerMovementController.characterController.transform.position = PlayerStartingPosition.position;
        }
    }

}
