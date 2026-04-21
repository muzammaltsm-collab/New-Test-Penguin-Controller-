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
        if (CurrentLevelNum >= _level.levels.Count)
        {
            CurrentLevelNum = 0;
        }
        _level.levels[CurrentLevelNum].BuildLevel();
    }

    public void PlaynextLevel()
    {
        CurrentLevelNum++;
        
        PlayerPrefs.SetInt("CurrentLevel", CurrentLevelNum);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
