using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StaticBatchingHandler : MonoBehaviour
{
    public GameObject[] TrackInfo;


    //public int batchSize = 15;
    private void Start()
    {
        //int arrayCount = TrackInfo.Length;
        //if (arrayCount >= 0)
        //{
        //    for (int i = 0; i < arrayCount; i += batchSize)
        //    {
        //        // Determine the number of elements to batch in this iteration
        //        int elementsInBatch = Mathf.Min(batchSize, TrackInfo.Length - i);

        //        // Create a sub-array containing elements for batching
        //        GameObject[] batchArray = new GameObject[elementsInBatch];
        //        Array.Copy(TrackInfo, i, batchArray, 0, elementsInBatch);

        //        // Apply batching to the sub-array

        //    }
        //}
        if (TrackInfo.Length > 0)
        {
            //StaticBatchingUtility.Combine(TrackInfo, this.gameObject);
        }
    }


}
