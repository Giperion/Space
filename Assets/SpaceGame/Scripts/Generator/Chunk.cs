using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

    public GameObject[] PrefabsForGenerick;
    public int typeOfChunk = 0; // 0 - лес, 1 - переход лес/пещера, 2 - пещера, 3 - переход пещера лес
    public ChunkType type;

    public float ChunkSize;

    void Start ()
    {
        if (typeOfChunk == 0)
        {
            CreateForestChunk();
        }
        // т.к. скрипт будет весеть на префабе чанка, то будет генерить на себе мусор относительно его типа
    }
    void CreateForestChunk()
    {
        //for (int i = 0; i <= 3; i++)
        //{
        //    GameObject bufferPrefab = Instantiate(PrefabsForGenerick[i], new Vector3(this.transform.localPosition.x + Random.Range(-15, 15), this.transform.localPosition.y, this.transform.localPosition.z + Random.Range(-15, 15)), Quaternion.identity) as GameObject;
        //    bufferPrefab.transform.parent = transform;
        //}
        //Instantiate(PrefabsForGenerick[0], this.transform.localPosition, Quaternion.identity);
    }

    public GameObject[] GetAllPlacesForJunk ()
    {
        return null;
    }
	
	void Update ()
    {
	    
	}
}


public enum ChunkType
{
    none,
    forest,
    cave,
    transition
}