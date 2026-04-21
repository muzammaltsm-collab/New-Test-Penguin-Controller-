using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisableScript : MonoBehaviour
{
    [SerializeField] float ObjectDisableTime = 3f;

    private void OnEnable()
    {
        StartCoroutine(ObjectDisable());
    }

    IEnumerator ObjectDisable()
    {
        yield return new WaitForSeconds(ObjectDisableTime);
        gameObject.SetActive(false);
    }
}
