using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class SampleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void initialize(bool agree)
    {

        //AdsManagerWrapper.Instance.initialize(agree);

    }
    public void ShowInterstitial()
    {
        //AdsManagerWrapper.Instance.ShowInterstitial();
    }


    public void ShowRewardedVideo()
    {
        //AdsManagerWrapper.Instance.ShowRewardedVideo(null);
    }

    public void ShowSmartBanner()
    {
        //AdsManagerWrapper.Instance.ShowBanner(AdPosition.Top,AdSize.SmartBanner);
    }

    public void ShowRectBanner()
    {
        //AdsManagerWrapper.Instance.ShowRectBanner(AdPosition.BottomRight, AdSize.MediumRectangle);

    }
    public void HideRectBanner()
    {
        AdsManagerWrapper.Instance.HideRectBanner();

    }
    public void HideBanner()
    {


        AdsManagerWrapper.Instance.HideBanner();

    }


    public void ShowAppOpen()
    {
        AdsManagerWrapper.Instance.ShowAppOpen();

    }
}
