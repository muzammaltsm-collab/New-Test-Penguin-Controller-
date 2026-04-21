using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;
public class FillBar : MonoBehaviour
{
    [SerializeField] Image fillImage = null;
   
    void Start()
    {
        Invoke(nameof(StartTransition), 2f);
    }
    public void StartTransition()
    {
        fillImage.DOPause();
        fillImage.fillAmount = 0;
        fillImage.DOFillAmount(1f, 5f).SetEase(Ease.Linear);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
