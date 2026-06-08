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
    [SerializeField] private float loadingDuration = 5f;
    [SerializeField] private float smoothSpeed = 2f;

    private float displayedPercentage = 0f;
    private float targetPercentage = 0f;

    private void Start()
    {
        StartCoroutine(LoadGameplayScene());
    }

    private IEnumerator LoadGameplayScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration)
        {
            elapsedTime += Time.deltaTime;

            targetPercentage = (elapsedTime / loadingDuration) * 100f;
            displayedPercentage = Mathf.Lerp(
                displayedPercentage,
                targetPercentage,
                Time.deltaTime * smoothSpeed
            );

            if (loadingPercentageText != null)
                loadingPercentageText.text = Mathf.RoundToInt(displayedPercentage) + "%";

            if (progressBar != null)
                progressBar.fillAmount = displayedPercentage / 100f;

            yield return null;
        }

        displayedPercentage = 100f;

        if (loadingPercentageText != null)
            loadingPercentageText.text = "100%";

        if (progressBar != null)
            progressBar.fillAmount = 1f;

        // Wait until user presses Continue/Not Now on ATT panel
        if (AdsManagerWrapper.Instance != null)
        {
            yield return new WaitUntil(() =>
                AdsManagerWrapper.Instance.IsATTFlowFinished
            );
        }

        // Wait until UMP + AdMob setup finishes
        if (AdsManagerWrapper.Instance != null)
        {
            yield return new WaitUntil(() =>
                AdsManagerWrapper.Instance.IsAdsSetupComplete
            );
        }

        if (AdsManagerWrapper.Instance != null &&
            AdsManagerWrapper.Instance.isAppOpenAvailable())
        {
            AdsManagerWrapper.Instance.ShowAppOpen();
            yield return new WaitForSeconds(0.5f);
        }

        SceneManager.LoadScene(gameplaySceneName);
    }
}