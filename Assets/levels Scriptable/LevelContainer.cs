using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelContainer", menuName = "ScriptableObjects/Level/Level Container")]
public class LevelContainer : ScriptableObject
{
    public List<LevelPathContainer> levels;
}
