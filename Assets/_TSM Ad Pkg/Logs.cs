using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logs
{
    public static void Print(string message, Color? c = null)
    {
#if UNITY_EDITOR
        string coloredMessage =
            $"<color=#{ColorUtility.ToHtmlStringRGBA(c ?? Color.yellow)}>{message}</color>";
        Debug.LogError(coloredMessage);
#endif
    }
}
