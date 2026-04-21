using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScript : MonoBehaviour
{

    public float DelayTime;
    public int NextSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        Invoke("NextSceneLoad", DelayTime);
    }

    public void NextSceneLoad()
    {
        SceneManager.LoadScene(NextSceneIndex);
    }

   
}
