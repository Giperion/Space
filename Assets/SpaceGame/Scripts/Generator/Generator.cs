using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

    public GameObject[] M_Chunks; // 0 - лес, 1 - переход лес/пещера, 2 - пещера, 3 - переход пещера лес 
    public List<GameObject> chunksOnScene;
    
    GameObject PlayerOnScene;

    int zNotePosCursor = 0;
    int numberOfLastChunk = 0;
    int typeOfLastChunk = 0; // 0 - лес, 1 - переход лес/пещера, 2 - пещера, 3 - переход пещера/лес 

    bool needToReedNow = true;

    void Start ()
    {
        PlayerOnScene = GameObject.FindGameObjectWithTag("Player");
    }

	void Update ()
    {
        if (needToReedNow)
            StartCoroutine(callPositionPlayer());
    }

    IEnumerator callPositionPlayer()
    {
        needToReedNow = false;
        AddCunk();
        yield return new WaitForSeconds(2);
        needToReedNow = true;
    }

    void AddCunk()
    {
        if (numberOfLastChunk == 0)
        {
            while (chunksOnScene.Count < 3)
            {
                CreateChunk();
            }  
        }
        else if (numberOfLastChunk != 0 && chunksOnScene.Count < 12)
        {           
            CreateChunk();  
        }  
    }

    void CreateChunk(int typeOf = 0)
    {
        GameObject bufferCunk = Instantiate(M_Chunks[WhichChunkNeed()], new Vector3(0, 0, zNotePosCursor), Quaternion.identity) as GameObject; // нужно как то доаблять их сразу в лист chunksOnScene
        chunksOnScene.Add(bufferCunk);
        IfAddChunk();
    }

    void IfAddChunk()
    {
        zNotePosCursor += 30;
        numberOfLastChunk++;
        RemoveOldChunk();
    }

    int WhichChunkNeed()
    {
        if (typeOfLastChunk == 0)
        {
            typeOfLastChunk = Random.Range(0, 2);
        }
        else if (typeOfLastChunk == 1)
        {
            typeOfLastChunk = 2;
        }
        else if (typeOfLastChunk == 2)
        {
            typeOfLastChunk = Random.Range(2, 4);
        }
        else if (typeOfLastChunk == 3)
        {
            typeOfLastChunk = 0;
        }
        return typeOfLastChunk;
    }

    void RemoveOldChunk()
    {
        if (chunksOnScene.Count >= 12)
        {
            
        }
    }
}
