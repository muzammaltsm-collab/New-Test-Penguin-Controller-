using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayConfeties : MonoBehaviour
{
    const string k_PlayerTag = "Player";
    bool check = true;
    [SerializeField] GameObject L_Confeti;
    [SerializeField] GameObject R_Confeti; 
   
    [SerializeField] float ObjectDisableTime = 3f;
    // Start is called before the first frame update
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            L_Confeti.SetActive(true);
            R_Confeti.SetActive(true);
          
            StartCoroutine(ObjectDisable());
        }
    }
    IEnumerator ObjectDisable()
    {
        yield return new WaitForSeconds(ObjectDisableTime);
        L_Confeti.SetActive(false);
        R_Confeti.SetActive(false);
        
    }
}
