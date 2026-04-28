using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UnlockCharacterPanel : MonoBehaviour
{
    public Text msg;
    int i = 5;
    public Action Reward;
    void OnEnable()
    {
        StartCoroutine(StartTimer());
    }
    public IEnumerator StartTimer()
    {

        while (i > 0)
        {
            msg.text = "Ads Starting in " + i + " sec";
            yield return new WaitForSecondsRealtime(1f);
            i--;
        }
        //AdsManagerWrapper.Instance.ShowRewardedVideo(Reward);
        i = 5;
        gameObject.SetActive(false);
    }
    public void Nothanks()
    {

        StopCoroutine(StartTimer());
        i = 5;
        
        gameObject.SetActive(false);
    }
}
