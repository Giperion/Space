using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

    public GameObject[] chunkObj;
    public List<GameObject> PrefabsForGenerick;
    public int typeOfChunk = 0; // 0 - лес, 1 - переход лес/пещера, 2 - пещера, 3 - переход пещера лес

    void Start ()
    {
	    // т.к. скрипт будет весеть на префабе чанка, то будет генерить на себе мусор относительно его типа
	}
	
	void Update ()
    {
	
	}
}
