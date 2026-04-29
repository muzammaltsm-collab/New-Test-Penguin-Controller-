using System;
using TMPro;
using UnityEngine;
using System.Collections;
public class rewardSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rewardToShow;
    [SerializeField] private Transform Hand;
    [SerializeField] private Animator handAnim;
    Coroutine noAdCoroutine;
    int currentGems;
    void Start()
    {
        handAnim = GetComponent<Animator>();
        if (PlayerPrefs.HasKey("PlayerGems"))
        {
            currentGems = (int)long.Parse(PlayerPrefs.GetString("PlayerGems"));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("rewardNo"))
        {
            var multiplier = other.gameObject.name;

            rewardToShow.text = (currentGems * float.Parse(multiplier)).ToString();
            PlayerPrefs.SetInt("reward", int.Parse(rewardToShow.text));
        }
    }
    public void DoubleDiamondRV()
    {
        AdsManagerWrapper.Instance.ShowRewardedVideo(GetTheReward, RewardNotAvailable);
    }
    void GetTheReward()
    {
        currentGems = PlayerPrefs.GetInt("reward");
        if (GameManager.Instance)
        {

            GameManager.Instance._gemsUpdater.AddGems(currentGems);
            GameManager.Instance.RewardAfterRV();
        }
        handAnim.enabled = false;
    }
    void RewardNotAvailable()
    {
        GameManager.Instance.UI.ShowNoAdText();

    }
}
