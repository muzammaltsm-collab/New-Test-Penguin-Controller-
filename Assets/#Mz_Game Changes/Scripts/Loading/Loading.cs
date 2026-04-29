using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingPercentageText;
    [SerializeField] private Image progressBar;

    [SerializeField] private string gameplaySceneName = "GameplayScene";
    [SerializeField] private float loadingDuration = 5f; // Total duration for both bar and scene loading
    [SerializeField] private float smoothSpeed = 2f;

    private float displayedPercentage = 0f;
    private float targetPercentage = 0f;
    // private AsyncOperation sceneLoadOperation;

    private void Start()
    {
        StartCoroutine(LoadGameplayScene());
    }

    private IEnumerator LoadGameplayScene()
    {
        // Start loading the scene asynchronously
        //sceneLoadOperation = SceneManager.LoadSceneAsync(gameplaySceneName);
        // sceneLoadOperation.allowSceneActivation = false;

        float elapsedTime = 0f;

        // Progress over the total loadingDuration
        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate target percentage based on elapsed time (0-100% over loadingDuration)
            targetPercentage = (elapsedTime / loadingDuration) * 100f;

            // Smoothly interpolate displayed percentage
            displayedPercentage = Mathf.Lerp(displayedPercentage, targetPercentage, Time.deltaTime * smoothSpeed);

            // Update UI
            if (loadingPercentageText != null)
            {
                loadingPercentageText.text = Mathf.RoundToInt(displayedPercentage) + "%";
            }

            if (progressBar != null)
            {
                progressBar.fillAmount = displayedPercentage / 100f;
            }

            yield return null;
        }

        // Ensure we reach exactly 100%
        displayedPercentage = 100f;
        if (loadingPercentageText != null)
        {
            loadingPercentageText.text = "100%";
        }
        if (progressBar != null)
        {
            progressBar.fillAmount = 1f;
        }

        if (AdsManagerWrapper.Instance != null && AdsManagerWrapper.Instance.isAppOpenAvailable()) { AdsManagerWrapper.Instance.ShowAppOpen(); }

        SceneManager.LoadScene(gameplaySceneName);

        // Wait a frame then activate the scene
        yield return null;
        //sceneLoadOperation.allowSceneActivation = true;
    }
}