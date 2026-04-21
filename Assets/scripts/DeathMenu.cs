using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using UnityEngine;

public class DeathMenu : MonoBehaviour {
    public Text scoreText;
    public Image BackGroundImage;
    private bool isShowned = false;
    private float transition = 0.0f;
	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (!isShowned)
            return;

        transition += Time.deltaTime;
        isShowned = true;
        BackGroundImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);

        	}

    public void ToggleEndMenu (float score)
    {
        gameObject.SetActive(true);
        scoreText.text = ((int)score).ToString ();
        isShowned = true;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MENU");

    }
}
