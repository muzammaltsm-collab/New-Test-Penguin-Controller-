using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Spike_HurdleScript : MonoBehaviour
{
    bool check = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && check)
        {
            Vector3 backDirection = other.transform.position - transform.position;
            Vector3 targetPosition = other.transform.position + backDirection.normalized * 2f; // Move back by 2 units
           
            PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
            if (playerMovementController != null)
            {

                GameManager.Instance.PlayerAnimation.PlayerFeature.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutQuad); // Tween for smooth movement
                GetComponent<Collider>().enabled = false;
            }

            GameManager.Instance.PlayerAnimation.PlayerFeature.AmountSubtractFunction();
            check = false;
        }
    }
}
