using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCostumesTrigger : MonoBehaviour
{
    const string k_PlayerTag = "Player";

    [SerializeField] GameObject parent;
    [SerializeField] GameObject Costumes;
    [SerializeField] GameObject CostumesPickupParticles;
    [SerializeField] int PlayerIndex;
    bool isTrigger;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(k_PlayerTag) && !isTrigger)
        {
            isTrigger = true;
            if (GameManager.Instance)
            {
                GameManager.Instance.SoundManager.Play_GemsCollectSound(GameManager.Instance.AS);
            }
            PlayParticles();
        }
    }
    void PlayParticles()
    {
        GameManager.Instance.PlayerAnimation.PlayerFeature.SelectPlayer(PlayerIndex);
        Costumes.SetActive(false);
        CostumesPickupParticles.SetActive(true);
        Invoke("DisableSwords", 2f);
    }
    void DisableSwords()
    {
       parent.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
