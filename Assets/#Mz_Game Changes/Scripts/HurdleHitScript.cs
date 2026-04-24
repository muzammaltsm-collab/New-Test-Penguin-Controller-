using DG.Tweening;
using UnityEngine;

public class HurdleHitScript : MonoBehaviour
{
    public enum HurdleKind
    {
        Box,
        Barrel,
        WoodenLog
    }
    bool check = true;
    public HurdleKind hurdleKind;
    [SerializeField] private AudioSource AS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && check)
        {
            Vector3 backDirection = other.transform.position - transform.position;
            Vector3 targetPosition = other.transform.position + backDirection.normalized * 2f; // Move back by 2 units

            PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
            if (playerMovementController != null)
            {

                GameManager.Instance.PlayerAnimation.PlayerFeature.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutQuad); // Tween for smooth movement
                GetComponent<Collider>().enabled = false;
                switch (hurdleKind)
                {
                    case HurdleKind.Box:
                        GameManager.Instance.SoundManager.Play_BoxHitSound(AS);
                        break;

                    case HurdleKind.Barrel:
                        GameManager.Instance.SoundManager.Play_BarrelHitSound(AS);

                        break;

                    case HurdleKind.WoodenLog:
                        GameManager.Instance.SoundManager.Play_WoodenLogHitSound(AS);

                        break;
                }
            }

            GameManager.Instance.PlayerAnimation.PlayerFeature.AmountSubtractFunction();
            check = false;
        }
    }

}
