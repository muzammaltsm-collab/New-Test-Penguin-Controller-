using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class PlayerAnimationScript : MonoBehaviour
{
    public PlayerAnimationScript PlayerAnimation;
    public Animator m_Animator;

    public PlayerFeatureScript PlayerFeature;
    [HideInInspector]
    public EnemyScript enemyscript;
    private void Start()
    {

    }
    #region Play Animation Function
    public Animator GetAnimator()
    {
        return m_Animator;
    }

    #endregion


    void ResetValues()
    {
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {
            // Disable movement control
            playerMovementController.isDealingWithHurdle = false;
            PlayerFeature.tag = "Player";
        }
       
    }
    public void FinishLineEnemiesLevelComparision(int amount)
    {
        if (PlayerFeature.PlayerLevelAmount < amount)
        {
            //PlayerFeature.PlayerMovementCheck = false;
        }
    }
    public void PlayIdleAnimation()
    {
        //This animation is player on Player animations by events 
        m_Animator.Play("Idle");
    }
   

    public void EnemyDeath(EnemyScript enemy)
    {
        enemyscript = enemy;

    }
    

    IEnumerator DieTimeDelay()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.EnableGameFailedPanel();
    }


    #region Upper Body Animation

    public void BackDeath()
    {
       
        StartCoroutine(DieTimeDelay());
    }



   
    public void EndSlashAnimation()
    {
        m_Animator.Play("EndSlash");
       
    }


    void AddForceOtherCharacter()
    {
        //enemyscript.AddForceFunction(AttackID);
    }
    #endregion
}