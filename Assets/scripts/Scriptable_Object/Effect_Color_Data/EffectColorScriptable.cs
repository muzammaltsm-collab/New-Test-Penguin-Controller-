using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect Data", menuName = "Scriptable Object/Effect Data")]
public class EffectColorScriptable : ScriptableObject
{
    public EffectData[] effectdata;
}

[System.Serializable]
public class EffectData
{
    [Header("Game Slash Effect!..................")]
    [Header("Slash Effects Data!.........")]
    public EndSlashData endSlashData;
    public SlashData slashData;
    //public Color Sparks;
    //public Color Hit;
    //public Color FlareEffect;
    //public Color SparKsCore;
    //public Material FlareMaterial;
    //public Material MainMaterial;
    [Header("Characte Data!.......")]
    public Color CharacterColor;
    public float CharacterMaxSize;
    [Header("Effect Range!........")]
    public int EffectStartValue;
    public int EffectMaxValue;
}

[System.Serializable]
public class EndSlashData
{
    public Material EndSlash;
    public Material GroundCrack;
    public Material Trail;
    public Color Fire;
    public Color Sparks;
    public Color Flash;
    public Color Derbis;
    public Color FlashColor;
    public Color Wind;
    public Color Leaves;
    public Color SparksCore;
    public Color Glow;
}

[System.Serializable]
public class SlashData
{
    public Color Sparks;
    public Color Hit;
    public Color FlareEffect;
    public Color SparKsCore;
    public Material FlareMaterial;
    public Material MainMaterial;
}