using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmountShowScript : MonoBehaviour
{
    public GameObject AmountShowObject;
    public TextMeshPro AmountText;

    public void AmountShow(int Amount)
    {
        AmountText.text = Amount.ToString(); 
        //currencyConvert(Amount);
       
    }

    #region Currency Convert
    public static string currencyConvert(float Score)
    {
        string result;
        string[] ScoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
        int i;

        for (i = 0; i < ScoreNames.Length; i++)
            if (Score < 900)
                break;
            else Score = Mathf.Floor(Score / 100f) / 10f;

        if (Score == Mathf.Floor(Score))
            result = Score.ToString() + ScoreNames[i];
        else result = Score.ToString() + ScoreNames[i];
        return result;
    }
    #endregion
}

public enum AmountEnum
{
    Add,
    Multiply,
    Sub
}

