using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Level_", menuName = "ScriptableObjects/Level/Level Path Container")]
public class LevelPathContainer : ScriptableObject
{
    public List<GameObject> Patches;
    public GameObject StartingPointPrefeb;
    public GameObject EndingPointPrefeb;
    public GameObject BackgroundEnv;
    Vector3 position;
    int _patchesCount;
    GameObject NewLevel;
    int CurrentLevelNum;

    public void BuildLevel()
    {
        CurrentLevelNum = PlayerPrefs.GetInt("CurrentLevel", 0);
        NewLevel = new GameObject("Level:  " + CurrentLevelNum);
        if (BackgroundEnv != null)
        {
            Instantiate(BackgroundEnv, NewLevel.transform);
        }
        Instantiate(StartingPointPrefeb, NewLevel.transform);
        for (_patchesCount = 0; _patchesCount < Patches.Count; _patchesCount++)
        {
            if (_patchesCount == 0)
            {
                position = StartingPointPrefeb.GetComponent<Track>().EndSpawnPoint.position;
                Instantiate(Patches[_patchesCount], position, Quaternion.identity, NewLevel.transform);
            }
            else
            {
                position += Patches[_patchesCount - 1].GetComponent<Track>().EndSpawnPoint.position;
                Instantiate(Patches[_patchesCount], position, Quaternion.identity, NewLevel.transform);
            }
        }
        position = position + Patches[_patchesCount - 1].GetComponent<Track>().EndSpawnPoint.position;
        position.y += 1.08f;
        Instantiate(EndingPointPrefeb, position, Quaternion.identity, NewLevel.transform);
    }
}
