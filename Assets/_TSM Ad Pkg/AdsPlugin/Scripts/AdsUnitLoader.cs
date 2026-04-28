using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdsUnitLoader : MonoBehaviour
{
    public AdsUnitModel _model;
    public string AdsIdsData;
    // Start is called before the first frame update

    private void Start()
    {
        //Invoke("ReteriveAdIds", 5f);
    }
    public void ReteriveAdIds()
    {
        //_model = JsonUtility.FromJson<AdsUnitModel>(RemoteValues.Instance.ReturnAdIds());

        //for (int i = 0; i < 3; i++)
        //{
        //    AdsManagerWrapper.Instance._inter.AdUnitID[i] = _model._inter[i];
        //    AdsManagerWrapper.Instance._rewarded.AdUnitID[i] = _model._rewarded[i];
        //    AdsManagerWrapper.Instance._appOpen.AdUnitID[i] = _model._appOpen[i];
        //    AdsManagerWrapper.Instance._banner.AdUnitID[i] = _model._banner[i];
        //    AdsManagerWrapper.Instance._rectBanner.AdUnitID[i] = _model._rectBanner[i];
        //    AdsManagerWrapper.Instance._rewardInter.AdUnitID[i] = _model._rewardInter[i];
        //}

    }
}
