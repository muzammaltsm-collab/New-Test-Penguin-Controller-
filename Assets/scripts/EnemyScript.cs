using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyScript : MonoBehaviour
{
    [Header("Reference Scripts!...")]
    [SerializeField] EnemyScript enemy;
    [SerializeField] AmountShowScript AmountShow;
    [Header("Select Enemies!...")]
    [SerializeField] bool IsGamePlaySmallEnemy = false;
    [SerializeField] bool IsFinishLineSmallEnemy = false;
    [SerializeField] bool IsGamePlaySmallBoss = false;
    [SerializeField] bool IsGamePlayBigBoss = false;
    [SerializeField] bool IsFinishLineSmallBoss = false;
    [SerializeField] bool IsFinishEndBoss = false;
    bool m_Applied;
    [Header("Enemies Material!...")]
    [SerializeField] Material GoldenFishMaterial;
    [SerializeField] Material FishMaterial;
    [SerializeField] Material DeathMaterial;
    [Header("Enemies Mesh!...")]
    [SerializeField] SkinnedMeshRenderer Rend;
    [Header("Enemies Varialbes!...")]
    [SerializeField] int FinishLineEnemyLevelAmount;
    [SerializeField] int GamePlayEnemyLevelAmount;
    [SerializeField] float forwardForce = 10f; // Adjust the force values as needed

    [SerializeField] float upwardForce = 5f;

    [SerializeField] float EnemyDisableTime = 0.1f;
    [Header("Level Sprite!...")]
    [SerializeField] SpriteRenderer LevelHeaderSprrite;
    [Header("Enemies Varialbes!...")]
    [SerializeField] GameObject BossEnableParticle;
    [SerializeField] GameObject BossKillingParticle;
    [Header("Audio Source!...")]
    [SerializeField] AudioSource AS;
    [Header("End Boss Things!...")]
    [SerializeField] float EndBossforwardForce = 10f; // Adjust the force values as needed
    [SerializeField] float EndBossupwardForce = 5f;
    [SerializeField] GameObject EndBossRagdol;
    [SerializeField] Rigidbody EndBoss_RigidBody;
    const string k_PlayerTag = "Player";
    private Rigidbody rb;
    private Collider col;
    Vector3 HitPosition;
    Vector3 PlayerForwardForce;
    Vector3 hitDirection;
    Vector3 targetPosition;
    private void Start()
    {
        if (IsFinishLineSmallEnemy || IsFinishLineSmallBoss)
        {
            AmountShow.AmountShow(FinishLineEnemyLevelAmount);
            LevelHeaderSprrite.gameObject.SetActive(false);
        }
        else
        {
            AmountShow.AmountShow(GamePlayEnemyLevelAmount);

        }
        if (IsGamePlayBigBoss || IsFinishEndBoss)
        {
            BossEnableParticle.SetActive(true);
        }

        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        if (IsGamePlaySmallEnemy || IsFinishEndBoss)
        {
            AmountShow.AmountShowObject.SetActive(false);
        }
        else
        {
            AmountShow.AmountShowObject.SetActive(true);
        }
        if (IsGamePlaySmallBoss)
        {
            Rend.material = GoldenFishMaterial;
        }
        else if (IsGamePlaySmallEnemy)
        {
            Rend.material = FishMaterial;
        }

    }

    private void Update()
    {
        if (GamePlayEnemyLevelAmount > GameManager.Instance.PlayerAnimation.PlayerFeature.PlayerLevelAmount)
        {
            LevelHeaderSprrite.color = Color.red;
        }
        else
        {
            LevelHeaderSprrite.color = Color.green;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(k_PlayerTag) && !m_Applied)
        {
            HitPosition = col.transform.position;
            PlayerForwardForce = col.transform.forward;
            hitDirection = (col.transform.position - transform.position).normalized; // Calculate the direction from the enemy to the player
            Vector3 backDirection = col.transform.position - transform.position;
            targetPosition = col.transform.position + backDirection.normalized * 2f; // Move back by 2 units
            CompareLevels();
        }

    }
    void CompareLevels()
    {
        if (IsGamePlaySmallBoss || IsGamePlaySmallEnemy || IsGamePlayBigBoss)
        {
            if (GameManager.Instance.PlayerAnimation.PlayerFeature.PlayerLevelAmount >= GamePlayEnemyLevelAmount)
            {
                PlayRandomAnimations();
            }
            else
            {

                PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
                if (playerMovementController != null)
                {

                    GameManager.Instance.PlayerAnimation.PlayerFeature.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutQuad); // Tween for smooth movement
                    GetComponent<Collider>().enabled = false;
                    GameManager.Instance.SoundManager.Play_FishHitSound(AS);
                }


                GameManager.Instance.PlayerAnimation.PlayerFeature.AmountSubtractFunction();
            }
        }
        else if (IsFinishEndBoss)
        {
            EndBossAddForceFunctionAFterKill();
        }
        else
        {
            if (GameManager.Instance.PlayerAnimation.PlayerFeature.PlayerLevelAmount >= FinishLineEnemyLevelAmount)
            {
                PlayRandomAnimations();

            }
            else
            {

                // Call the HitByEnemy method on the player script
                GameManager.Instance.PlayerAnimation.PlayerFeature.HitByEnemy();


            }
        }
        m_Applied = true;
    }
    void ResetValues()
    {
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {
            // Disable movement control

            GameManager.Instance.PlayerAnimation.PlayerFeature.tag = "Player";
        }

    }
    void PlayRandomAnimations()
    {
        GameManager.Instance.PlayerAnimation.PlayerFeature.AddCharacterAmount(GamePlayEnemyLevelAmount);
        AddForceFunction();

    }



    public void AddForceFunction()
    {
        if (IsFinishLineSmallEnemy || IsGamePlaySmallEnemy)
        {
            GameManager.Instance.SoundManager.Play_CollectBlueFishSounds(AS);
        }
        else
        {
            GameManager.Instance.SoundManager.Play_GoldenFishCollectSound(AS);
        }


        if (BossEnableParticle != null)
        {
            BossEnableParticle.SetActive(false);
        }
        BossKillingParticle.SetActive(true);

        AmountShow.AmountShowObject.SetActive(false);
        col.enabled = false;
        Rend.gameObject.SetActive(false);

        StartCoroutine(DisableTimeDelay());
    }
    public void EndBossAddForceFunctionAFterKill()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.MainCamera.target.position = GameManager.Instance.MainCamera.EndPlayerCamPos.position;
            GameManager.Instance.MainCamera.target.rotation = GameManager.Instance.MainCamera.EndPlayerCamPos.rotation;
            GameManager.Instance.MainCamera.isRotation = true;
            GameManager.Instance.PlayerAnimation.gameObject.SetActive(false);
            GameManager.Instance.EndPlayer.SetActive(true);
            GameManager.Instance.PlayerAnimation.PlayerFeature.StopPlayerMovement();
            GameManager.Instance.SoundManager.Play_GoldenFishCollectSound(AS);

        }

        BossEnableParticle.SetActive(false);
        BossKillingParticle.SetActive(true);
        AmountShow.AmountShowObject.SetActive(false);
        col.enabled = false;
        Rend.material = DeathMaterial;
        Rend.gameObject.SetActive(false);
        rb.isKinematic = true;
        EndBossRagdol.SetActive(true);
        EndBoss_RigidBody.AddForce(transform.forward * EndBossforwardForce, ForceMode.Impulse);
        EndBoss_RigidBody.AddForce(Vector3.up * EndBossupwardForce, ForceMode.Impulse);

        Invoke(nameof(StopPlayerEndOfLevel), 2f);
    }

    void StopPlayerEndOfLevel()
    {
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {
            // Disable movement control
            playerMovementController.IsUpdateRun = false;
        }
        Invoke(nameof(EnableRetryPanel), 2f);
    }
    void EnableRetryPanel()
    {

        GameManager.Instance.EnableGameFailedPanel();
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(EnemyDisableTime);
        Rend.gameObject.SetActive(false);
        EndBossRagdol.SetActive(true);
        //EndBoss_Ragdoll.AddForce(transform.forward * EndBossforwardForce, ForceMode.Impulse);
        //EndBoss_Ragdoll.AddForce(Vector3.up * EndBossupwardForce, ForceMode.Impulse);
        //gameObject.SetActive(false);
    }

    IEnumerator DisableTimeDelay()
    {
        yield return new WaitForSeconds(EnemyDisableTime);
        gameObject.SetActive(false);
    }

}