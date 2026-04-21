using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class First_Scene : MonoBehaviour
{
    [SerializeField] float Delay = 0;
   [SerializeField] string SceneName = "SplashScene";
    private void Start()
    {
        LoadScene(Delay);
    }
    Coroutine SceneCor;
    void StopSceneCor()
    {
        if (SceneCor != null)
            this.StopCoroutine(SceneCor);
    }
    void LoadScene(float time)
    {
        StopSceneCor();
        if (time == 0)
            SceneManager.LoadScene(SceneName);
        else
        {
            
            SceneCor = StartCoroutine(delaySceneLoad(time));
        }
    }
    IEnumerator delaySceneLoad(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneName);
    }
}
