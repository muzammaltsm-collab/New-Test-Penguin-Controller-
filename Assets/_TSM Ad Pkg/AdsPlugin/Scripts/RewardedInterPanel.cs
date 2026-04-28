using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedInterPanel : MonoBehaviour
{
    // Start is called before the first frame update
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
        //AdsManagerWrapper.Instance.ShowRewardedInter(Reward);
        i = 5;
        gameObject.SetActive(false);
    }
    public void Nothanks()
    {
        StopCoroutine(StartTimer());
        i = 5;
        AdsManagerWrapper.Instance.ShowInterstitial();
        gameObject.SetActive(false);
    }

}
