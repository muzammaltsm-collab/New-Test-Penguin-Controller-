using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI; // Reference to the UI Text component to display coins

public class CoinUIController : MonoBehaviour // This script should be attached to the UI Text component that displays the coin amount
{
    public RectTransform containerRect; // Reference to the coin icon RectTransform
    public Text coinText; // Reference to the UI Text component to display coins

    //Animation parameters
    public float popScale = 1.5f; // How much to scale up the coin icon
    public float raiseAmount = 30f; // How much to raise the coin icon
    public float animationSpeed = 0.5f; // Speed of the animation

    private int currentTotalCoins = 0; // Track the current total coins for animation purposes
    private Vector3 originalScale; // Store the original scale of the coin icon
    private Vector2 originalPosition; // Store the original position of the coin icon
    private Coroutine currentAnimation; // Reference to the current animation coroutine

    private void Start()
    {
        //cahe original scale and position
        if (containerRect == null) containerRect = GetComponentInChildren<RectTransform>(); // Fallback to find the RectTransform in case it's not set in the inspector
        if (coinText == null) coinText = GetComponent<Text>();   // Fallback to find the Text component in case it's not set in the inspector
        originalScale = containerRect.localScale;
        originalPosition = containerRect.anchoredPosition;

        // Initialize the coin text with the current total coins
        coinText.text = currentTotalCoins.ToString();

    }
    // Call this method to update the coin amount and trigger the animation
    public void UpdateCoinAmount(int newTotalCoins)
    {
        if (newTotalCoins != currentTotalCoins) // Only update and animate if the coin amount has changed
        {
            currentTotalCoins = newTotalCoins;
            coinText.text = currentTotalCoins.ToString(); // Update the displayed coin amount
            // If an animation is already running, stop it before starting a new one
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
                ResetAnimation(); // Reset to original state before starting the new animation
            }
            currentAnimation = StartCoroutine(AnimateCoin());// Start the animation coroutine

        }
    }

    private IEnumerator AnimateCoin() // Coroutine to animate the coin icon
    {
        float elapsedTime = 0f;
        Vector3 targetScale = originalScale * popScale;
        Vector2 targetPosition = originalPosition + new Vector2(0, raiseAmount);
        while (elapsedTime < animationSpeed)
        {
            float t = elapsedTime / animationSpeed;
            containerRect.localScale = Vector3.Lerp(originalScale, targetScale, t);
            containerRect.anchoredPosition = Vector2.Lerp(originalPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        containerRect.localScale = targetScale;
        containerRect.anchoredPosition = targetPosition;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(ResetAnimation());
    }

    private IEnumerator ResetAnimation()
    {
        float elapsed = 0f;
        while (elapsed < animationSpeed)
        {
            float t = elapsed / animationSpeed;
            containerRect.localScale = Vector3.Lerp(containerRect.localScale, originalScale, t);
            containerRect.anchoredPosition = Vector2.Lerp(containerRect.anchoredPosition, originalPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        containerRect.localScale = originalScale;
        containerRect.anchoredPosition = originalPosition;
    }
}
