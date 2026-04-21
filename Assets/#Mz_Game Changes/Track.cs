using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform EndSpawnPoint;

    public GameObject[] trackObjects;
    public GameObject[] boxHurdles;
    public GameObject[] coins;
    public GameObject[] fish;
    public GameObject[] jumps;

    private void Start()
    {
        CombineIfValid(trackObjects);
        CombineIfValid(boxHurdles);
        CombineIfValid(coins);
        CombineIfValid(fish);
        CombineIfValid(jumps);
    }

    private void CombineIfValid(GameObject[] objectsToCombine)
    {
        if (objectsToCombine == null || objectsToCombine.Length == 0)
            return;

        List<GameObject> validObjects = new List<GameObject>();

        foreach (GameObject obj in objectsToCombine)
        {
            if (obj != null)
                validObjects.Add(obj);
        }

        if (validObjects.Count > 0)
            StaticBatchingUtility.Combine(validObjects.ToArray(), gameObject);
    }
}