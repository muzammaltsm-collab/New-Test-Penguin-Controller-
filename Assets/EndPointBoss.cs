using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointBoss : MonoBehaviour
{
    public GameObject[] Boss;
    public Transform BossPosition;
    int CurrentLevelNum = 0;
    int BossIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Boss.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 boss prefabs in the inspector.");
            return;
        }
        CurrentLevelNum = PlayerPrefs.GetInt("CurrentLevel", 0);
        SpawnBoss();

    }
    void SpawnBoss()
    {
        // Determine which boss to spawn
        int bossIndex = (CurrentLevelNum / 5) % Boss.Length;

        // Instantiate the boss
        Instantiate(Boss[bossIndex], BossPosition);

        // Print to console which boss is spawned for debugging
        Debug.Log("Spawned Boss: " + Boss[bossIndex].name);
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with this one has a "Player" tag
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0.5f;

            // Call the jump function of the player
            PlayerMovementController playerMovementController = PlayerMovementController.GetInstance();
            //GameManager.Instance.SoundManager.Play_JumpSound(AS);
            // Now you can call methods of PlayerMovementController instance
            if (playerMovementController != null)
            {
                playerMovementController.EndBossKillJump();

            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
