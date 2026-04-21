using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour {

    private float score = 0.0f;
    private int difficultyLevel = 1;
    private int maxDificultyLevel = 10;
    private int scoreToNextLevel = 10;
    private bool isDead = false;
    public Text scoreText;
    public DeathMenu deathMenu;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (isDead)
            return;

        if (score >= scoreToNextLevel)
            LevelUp();

        score += Time.deltaTime * difficultyLevel;
        scoreText.text = ((int)score).ToString();
    }
    void LevelUp()
    {
        if (difficultyLevel == maxDificultyLevel)
            return;


        scoreToNextLevel *= 2;
        difficultyLevel++;

       GetComponent <PlayerMotor>().setSpeed(difficultyLevel);

        Debug.Log(difficultyLevel);
    }

    public void OnDeath() {
        isDead = true;
        if (PlayerPrefs.GetFloat("Highscore") < score)
            PlayerPrefs.SetFloat("Highscore",score);

        deathMenu.ToggleEndMenu (score); 
    }
}
