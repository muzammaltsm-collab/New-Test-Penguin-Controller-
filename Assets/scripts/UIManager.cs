using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [Header("...UI Panels!...")]
    public GameObject FadeScreen;
    public GameObject GameStartPanel;
  
    public GameObject Levelfailed;
    public GameObject DiamondsContainer;
    public GameObject RetryBtn;
    public bool IsPlayerControlsEnable = false;
    [Header("...Retry Panels!...")]
    public GameObject RetryPanel;
    public GameObject NoThanksBtn;
    [Header("...Level Text!...")]
    [SerializeField] TMP_Text LevelText;
      public  int LevelNum;

    // Start is called before the first frame update
    void Start()
    {
       
        LevelText.text = LevelNum.ToString();
        EnableGameStartControlls();
    }
    private void Awake()
    {
        LevelNum = PlayerPrefs.GetInt("CurrentLevel", 0);
        LevelNum += 1;
    }
    public void EnableGameStartControlls()
    {
       
        RetryBtn.SetActive(false);
        DiamondsContainer.SetActive(true);
        GameStartPanel.SetActive(true);
        if (GameManager.Instance)
        {
            GameManager.Instance.OrignalPlayer.SetActive(false);
            GameManager.Instance.MovementController.SetActive(false);
            GameManager.Instance.StartingPlayer.SetActive(true);
        }
    }
    public void StartGame()
    {
        RetryBtn.SetActive(true);
        GameStartPanel.SetActive(false);
        if (GameManager.Instance)
        {
            GameManager.Instance.MovementController.SetActive(true);
            GameManager.Instance.OrignalPlayer.SetActive(true);
            GameManager.Instance.StartingPlayer.SetActive(false);
            IsPlayerControlsEnable = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Retry()
    {
        FadeScreen.SetActive(true);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
