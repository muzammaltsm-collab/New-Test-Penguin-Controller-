using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript
{
    public static void PlayAnimation(Animator Anim, string StateName, int LayerID, float LayerWait)
    {
        Anim.SetLayerWeight(LayerID, LayerWait);
        if (!Anim.GetCurrentAnimatorStateInfo(LayerID).IsName(StateName))
        {
            Anim.Play(StateName, LayerID);
        }
    }

    public static void AnimationWaitAgainReset(Animator m_Animator)
    {
        m_Animator.SetLayerWeight(1, 0);
    }
}