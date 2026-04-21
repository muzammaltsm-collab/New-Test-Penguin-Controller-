using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject[] tilePerfabs;

    private Transform PlayerTransform;
    private float spawnZ = -12.0f;
    private float tileLength = 20.0f;
    private int amnTilesOnScreen = 10;
    private float safeZone = 15.0f;
    private int lastPrefabIndex = 0;

    private List<GameObject> activeTiles;

	// Use this for initialization
	void Start () {
        activeTiles = new List<GameObject>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < amnTilesOnScreen; i++)
        {
            if (i < 9)
                SpawnTile(0);
            else
            SpawnTile();
        }
      
    }
	
	// Update is called once per frame
	void Update () {
		if(PlayerTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
	}
    private void SpawnTile (int prefabIndex = -1)
    {
        GameObject go;
        if (prefabIndex == -1) 
            go = Instantiate(tilePerfabs[RandomPrefabIndex()]) as GameObject ;
        else
            go = Instantiate(tilePerfabs[prefabIndex])as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt (0);
    }
    private int RandomPrefabIndex()
    {
        if (tilePerfabs.Length <= 1)
            return 0;
        int randomIndex = lastPrefabIndex;
        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePerfabs.Length);
        }
        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
