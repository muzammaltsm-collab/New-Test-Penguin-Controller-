using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;



public class PlayerFeatureScript : MonoBehaviour
{
   

    #region Variables
    [Header("Players Types!..............")]
    public GameObject[] Players;
    [Header("Player Charge Effect!.........")]
    //[SerializeField] GameObject PlayerChargeParticle;
    //[SerializeField] Transform AddParticleSpawnPosition;
    [SerializeField] int TotalParticleSpawn = 20;
    [Header("Amount Show Data!..........")]
    public int PlayerLevelAmount = 1;
    public AmountShowScript AmountShow;
    [Header("Add Particle Data!....")]
    public GameObject DiamondCollectParticle;
    [Header("Ragdol!....")]
    [SerializeField] GameObject Ragdoll;
    [SerializeField] Rigidbody Ragdoll_Rb;
    [SerializeField] float Ragdoll_UpwardForce;
    [SerializeField] float Ragdoll_ForwardForce;
    Rigidbody player_rb;
    [Header("Audio Source!....")]
    [SerializeField] AudioSource AS;
    int CurrentSwordIndex;
    #endregion



    #region Awake and Start
    private void Start()
    {   
        AmountShow.AmountShow(PlayerLevelAmount);
        player_rb = GetComponent<Rigidbody>();
        CurrentSwordIndex = PlayerPrefs.GetInt("PlayerSword", 0);
    }
    #endregion

    public void AddCharacterAmount(int Amount)
    {
        AmountAddFunction(Amount);
    }

    #region Add Particle Function

    #endregion
    public void HitByEnemy()
    {
        GameManager.Instance.SoundManager.Play_PlayerDeathSound(AS);
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {
            playerMovementController.IsUpdateRun = false;
            player_rb.useGravity = false;
            player_rb.isKinematic = true;
            playerMovementController.characterController.enabled = false;
        }
        GameManager.Instance.PlayerAnimation.m_Animator.gameObject.SetActive(false);
        AmountShow.AmountShowObject.SetActive(false);
        Ragdoll.SetActive(true);
        Ragdoll_Rb.AddForce(transform.forward * Ragdoll_ForwardForce, ForceMode.Impulse);
        Ragdoll_Rb.AddForce(Vector3.up * Ragdoll_UpwardForce, ForceMode.Impulse);
        GameManager.Instance.EnableRetry();

    }


    public void StopPlayerMovement()
    {
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {
            Time.timeScale = 1;
            playerMovementController.PlayerMovementCheck = false;
            playerMovementController.IsControlDisable = false;
            GameManager.Instance.PlayerAnimation.m_Animator.Play("Idle");

        }
    }

    #region Amount Add Function
    public void AmountAddFunction(int amount)
    {
        //CurrentAmountShowFunction();
        PlayerLevelAmount = PlayerLevelAmount + amount;

        AmountShow.AmountShow(PlayerLevelAmount);


    }
    public bool HasEnoughFish(int amount)
    {
        return PlayerLevelAmount >= amount;
    }
    public void RemoveFish(int amount)
    {
        PlayerLevelAmount -= amount;

        if (PlayerLevelAmount < 0)
            PlayerLevelAmount = 0;

        if (AmountShow != null)
        {
            AmountShow.AmountShow(PlayerLevelAmount);
        }
    }
    public void AmountSubtractFunction()
    {
        const int subtractAmount = 10;

        PlayerLevelAmount -= subtractAmount;
        if (PlayerLevelAmount < 1)
        {
            PlayerLevelAmount = 1;
        }
        AmountShow.AmountShow(PlayerLevelAmount);
    }
    #endregion

    #region Add Particle Function
    public void ShowAddParticle()
    {
        //    if (AddParticleCounter < TempAddParticleStore.Count)
        //    {
        //        TempAddParticleStore[AddParticleCounter].SetActive(true);
        //        AddParticleCounter++;
        //    }
        //    else
        //    {
        //        AddParticleCounter = 0;
        //        TempAddParticleStore[AddParticleCounter].SetActive(true);
        //        AddParticleCounter++;
        //    }
    }
    #endregion
    #region PlayerChange
    public void SelectPlayer(int Var)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].SetActive(false);
        }
        if (Var <= Players.Length)
        {
            Players[Var].SetActive(true);
        }
        else
        {
            Players[0].SetActive(true);

        }


    }
    #endregion
  

}