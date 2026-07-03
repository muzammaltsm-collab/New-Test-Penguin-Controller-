using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelManager : MonoBehaviour
{
    public LevelContainer _level;
    public int CurrentLevelNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        CurrentLevelNum = PlayerPrefs.GetInt("CurrentLevel", 0);
        int levelIndex;
        if (CurrentLevelNum % 6 == 0)
        {
            if (AdsManagerWrapper.Instance != null && AdsManagerWrapper.Instance.IsInterstitialAvailable()) AdsManagerWrapper.Instance.ShowInterstitial();
        }
        if (CurrentLevelNum < _level.levels.Count)
        {
            // Normal order
            levelIndex = CurrentLevelNum;
        }
        else
        {
            // After all levels → random
            levelIndex = Random.Range(0, _level.levels.Count);
        }

        _level.levels[levelIndex].BuildLevel();
    }

    public void PlaynextLevel()
    {
        CurrentLevelNum++;
        
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevelNum);
    }
    
}
