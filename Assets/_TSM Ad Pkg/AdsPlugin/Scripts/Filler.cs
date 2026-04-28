using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Filler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject FillBar;
    void OnEnable()
    {
        FillBar.transform.localScale = new Vector3(0, 1, 1);
        //LeanTween.scaleX(FillBar, 1, 1.25f).setIgnoreTimeScale(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
