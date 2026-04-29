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
    [Header("...Stage Failed Panels!...")]
    public GameObject StagefailedPanel;
    [Header("...Stage Retry Panels!...")]
    public GameObject RetryPanel;
    public GameObject RetryBtn;
    [Header("...Coim Caintainer !...")]
    public GameObject DiamondsContainer;
    public bool IsPlayerControlsEnable = false;
    [Header("...Stage Clear Panels!...")]
    public GameObject StageClearPanel;
    public GameObject NoThanksBtn;
    public GameObject NoAdAvailableText;
    public GameObject NotenoughCoinsText;
    [Header("...Level Text!...")]
    [SerializeField] TMP_Text LevelText;
    public int LevelNum;
    Coroutine noAdCoroutine;
    Coroutine notEnoughCoinsCoroutine;
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
    public void EnableRetryPanel()
    {
        RetryPanel.SetActive(true);
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {
            playerMovementController.IsUpdateRun = false;
        }
    }
    public void DisableRetryPanel()
    {
       
        RetryPanel.SetActive(false);
        PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
        if (playerMovementController != null)
        {
            playerMovementController.IsUpdateRun = true;
        }
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
            if (GameManager.Instance.PlayerAnimation.m_Animator != null && !GameManager.Instance.IsPlayerDead)
            {
                GameManager.Instance.PlayerAnimation.m_Animator.transform.rotation = Quaternion.identity;
                GameManager.Instance.PlayerAnimation.m_Animator.Play("Run");
                Debug.Log("Play Run Animation At start");
            }
        }
    }

    public void Retry()
    {
        FadeScreen.SetActive(true);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RetryWithCoins()
    {

        long cost = 500;

        if (GameManager.Instance._gemsUpdater != null)
        {
            if (HasEnoughGems(cost))
            {
                GameManager.Instance._gemsUpdater.DeductGems(cost);
                DisableRetryPanel();
                Retry();
            }
            else
            {
                Debug.Log("Not enough gems!");
                ShowNotEnoughCoinsText();
                // Optional: Show UI message
            }
        }
    }

    bool HasEnoughGems(long amount)
    {
        // Accessing private currentGems indirectly → better to expose a getter
        return long.Parse(PlayerPrefs.GetString("PlayerGems", "0")) >= amount;
    }

    public void RetryWithAd()
    {
        AdsManagerWrapper.Instance.ShowRewardedVideo(OnAdSuccess, OnAdFailed);
    }

    void OnAdSuccess()
    {
        Retry();
    }


    void OnAdFailed()
    {
        ShowNoAdText();
    }
    public void ShowNoAdText()
    {
        if (noAdCoroutine != null)
            StopCoroutine(noAdCoroutine);

        noAdCoroutine = StartCoroutine(NoAdAnim());
    }
    public void ShowNotEnoughCoinsText()
    {
        if (notEnoughCoinsCoroutine != null)
            StopCoroutine(notEnoughCoinsCoroutine);

        notEnoughCoinsCoroutine = StartCoroutine(NotEnoughCoinsAnim());
    }

    IEnumerator NotEnoughCoinsAnim()
    {
        NotenoughCoinsText.SetActive(true);

        Transform t = NotenoughCoinsText.transform;
        t.localScale = Vector3.zero;

        float duration = 0.25f;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            t.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / duration);
            yield return null;
        }

        t.localScale = Vector3.one;

        yield return new WaitForSecondsRealtime(3f);

        NoAdAvailableText.SetActive(false);
    }
    IEnumerator NoAdAnim()
    {
        NoAdAvailableText.SetActive(true);

        Transform t = NoAdAvailableText.transform;
        t.localScale = Vector3.zero;

        float duration = 0.25f;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            t.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / duration);
            yield return null;
        }

        t.localScale = Vector3.one;

        yield return new WaitForSecondsRealtime(3f);

        NoAdAvailableText.SetActive(false);
    }
}
