using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDeath : MonoBehaviour
{
    const string k_PlayerTag = "Player";
    bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(k_PlayerTag) && !isFalling)
        {
            isFalling = true;
            if (GameManager.Instance)
            {
                GameManager.Instance.UI.Levelfailed.SetActive(true);
            }
           
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
