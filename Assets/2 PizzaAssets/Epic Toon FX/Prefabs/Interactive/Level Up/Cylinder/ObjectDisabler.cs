using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour
{
    public float Time;
    private void OnEnable()
    {
        StartCoroutine(delayin_Disable());
    }

    IEnumerator delayin_Disable()
    {
        yield return new WaitForSeconds(Time);  
        this.gameObject.SetActive(false);
    }
}
