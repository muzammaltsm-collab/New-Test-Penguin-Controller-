using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class MainMenu : MonoBehaviour {

    public Text highscoreText;
    public Text TotalPointsText;
    // Use this for initialization
    void Start () {
        highscoreText.text = "Highscore : " + ((int)PlayerPrefs.GetFloat("Highscore")).ToString();
        TotalPointsText.text = "TotalPoints : " + (PlayerPrefs.GetInt("TotalPoints")).ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ToGame() {
        SceneManager.LoadScene("Game");
    }

}
