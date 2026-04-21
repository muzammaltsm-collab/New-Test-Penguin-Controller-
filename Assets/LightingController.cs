using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingController : MonoBehaviour
{
    public Material[] skyboxes; // Assign skyboxes for each level in the Inspector
    public Color[] fogColors;   // Assign fog colors for each level in the Inspector
    public bool[] useFog;       // Assign whether fog is enabled for each level in the Inspector

    private void Start()
    {
        int currentLevel = GetCurrentLevel(); // Implement a method to get the current level

        // Set Skybox
        RenderSettings.skybox = skyboxes[currentLevel];

        // Set Fog
        RenderSettings.fog = useFog[currentLevel];
        if (RenderSettings.fog)
        {
            RenderSettings.fogColor = fogColors[currentLevel];
        }

        // Set Camera Background Color
        Camera.main.backgroundColor = GetCameraBackgroundColor(currentLevel); // Implement a method to get camera background color
    }

    // Method to get the current level (You should implement your own logic here)
    private int GetCurrentLevel()
    {
        // Example implementation
        // You might use PlayerPrefs, GameManager, or some other method to track the current level
        return 0; // For demonstration, returning 0 assuming the first level
    }

    // Method to get camera background color for each level (You should implement your own logic here)
    private Color GetCameraBackgroundColor(int level)
    {
        // Example implementation
        // You can define an array of colors or switch statements based on the level
        return Color.black; // For demonstration, returning black color
    }
}
