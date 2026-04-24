using Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("...Player!...")]
    public GameObject OrignalPlayer;
    public GameObject StartingPlayer;
    public GameObject EndPlayer;
    [Header("...Movement Controller!...")]
    public GameObject MovementController;
    [Header("...PlayerFeatureScript!...")]
    public PlayerFeatureScript _playerFeatureScript;

    [Header("...Main Camera!...")]
    public CameraFollow MainCamera;
    [Header("...Player Animation Script!...")]
    public PlayerAnimationScript PlayerAnimation;
    [Header("...SoundManager!...")]
    public Sound_ScriptableObject SoundManager;
    [Header("...AudioSource!...")]
    public AudioSource AS;
    [Header("...GemsUpdater!...")]
    public GemsUpdater _gemsUpdater;
    //UImanager
    [Header("...UI Manager!...")]
    public UIManager UI;
    [Header("...Level Manager!...")]
    public levelManager _levelManager;

    // Flag to indicate player death so other systems don't force animations back to Run
    [HideInInspector]
    public bool IsPlayerDead = false;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
    public static GameManager GetInstance()
    {
        return Instance;
    }
    private void Awake()
    {
       
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Debug.Log("Start");
        UI.FadeScreen.SetActive(true);
        UI.RetryPanel.SetActive(false);
        _levelManager.PlaynextLevel();
        // reset death flag
        IsPlayerDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        Debug.Log("Start");
        UI.FadeScreen.SetActive(true);
        UI.Levelfailed.SetActive(false);
        // reset death flag
        IsPlayerDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RewardAfterRV()
    {
        Time.timeScale = 1f;
        
        UI.FadeScreen.SetActive(true);
        UI.RetryPanel.SetActive(false);
        _levelManager.PlaynextLevel();
        // reset death flag
        IsPlayerDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnableRetry()
    {

        StartCoroutine(DieTimeDelay());
    }
    IEnumerator DieTimeDelay()
    {
        yield return new WaitForSeconds(3f);
        EnableGameFailedPanel();
    }
    public void EnableGameFailedPanel()
    {
        UI.GameStartPanel.SetActive(false);
        UI.RetryBtn.SetActive(false);
        UI.RetryPanel.SetActive(true);
        Invoke(nameof(EnableNoThanksBtn), 5f);
        Time.timeScale = 1f;
    }
    void EnableNoThanksBtn()
    {
        UI.NoThanksBtn.SetActive(true);
    }
}

