using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordsPickupTrigger : MonoBehaviour
{
    const string k_PlayerTag = "Player";

    [SerializeField] GameObject parent;
    [SerializeField] GameObject Swords;
    [SerializeField] GameObject SwordsPickupParticles;
    [SerializeField] AudioSource AS;
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
                GameManager.Instance.SoundManager.Play_GemsCollectSound(AS);
                GameManager.Instance._gemsUpdater.AddGems(1);
            }
            PlayParticles();
            //GameManager.Instance.PlayerAnimation.PlayerFeature.DiamondCollectParticle.SetActive(true);
            //StartCoroutine(MoveAndHide());
        }
    }
    void PlayParticles()
    {
        GameManager.Instance.PlayerAnimation.PlayerFeature.AddCharacterAmount(10);
        Swords.SetActive(false);
        SwordsPickupParticles.SetActive(true);
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
