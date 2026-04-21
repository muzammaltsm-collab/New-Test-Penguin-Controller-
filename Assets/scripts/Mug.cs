using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Mug : MonoBehaviour
{
    public int points = 0;
    public int OldTotalPoint;
    // Use this for initialization
    void Start()
    {
        OldTotalPoint = (PlayerPrefs.GetInt("TotalPoints"));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }

   
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.name == "Player")
            {
             other.GetComponent<PlayerMotor>().points++;
             Destroy(gameObject);
             PlayerPrefs.SetInt("TotalPoints", (other.GetComponent<PlayerMotor>().points));
             Debug.Log(other.GetComponent<PlayerMotor>().points);
            
            }

    }  

}

