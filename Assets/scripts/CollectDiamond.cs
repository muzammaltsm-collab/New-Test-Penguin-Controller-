using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectDiamond : MonoBehaviour
{
    const string k_PlayerTag = "Player";
    [SerializeField] GameObject Diamond;
    [SerializeField] GameObject DiamondParticles;
    [SerializeField] AudioSource AS;
    //[SerializeField] Vector3 moveDirection;// Change this to your desired direction
    //[SerializeField] float moveSpeed = 5f;
   
    //[SerializeField] float ScaleDecreasesSpeed = 5f;
    bool isMoving = false;
    Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        Diamond.SetActive(true);
        DiamondParticles.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(k_PlayerTag) && !isMoving)
        {
            isMoving = true;
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
    //IEnumerator MoveAndHide()
    //{
    //    // Move the diamond in the specified direction
    //    float distanceMoved = 0f;
    //    while (distanceMoved < moveSpeed)
    //    {
    //        Invoke("DisableDiamod", 0.3f);
    //        transform.localScale -= initialScale * Time.deltaTime * ScaleDecreasesSpeed;
    //        transform.position += moveDirection * Time.deltaTime * moveSpeed;
    //        distanceMoved += Time.deltaTime * moveSpeed;
    //        yield return null;
    //    }

    //    // Hide the diamond

    //}
    void DisableDiamod()
    {
        gameObject.SetActive(false);
    }
    void PlayParticles()
    {
        Diamond.SetActive(false);
        DiamondParticles.SetActive(true);
        Invoke("DisableDiamod",2f);
    }
    // Update is called once per frame
    void Update()
    {

    }
}