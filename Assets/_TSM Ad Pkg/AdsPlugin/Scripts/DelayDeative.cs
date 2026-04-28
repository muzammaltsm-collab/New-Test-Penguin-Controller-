using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayDeative : MonoBehaviour
{
    public float DelayTime = 2f;


    public void OnEnable()
    {
        StartCoroutine(WFS_Deative());
    }



    IEnumerator WFS_Deative()
    {
        yield return new WaitForSecondsRealtime(DelayTime);
        this.gameObject.SetActive(false);
    }
}
