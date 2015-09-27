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


    void Start ()
    {
        PlayerOnScene = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(callPositionPlayer());
    }


    void Update()
    {

    }

    IEnumerator callPositionPlayer()
    {
        while (true)
        {
            RemoveOldChunk(PlayerOnScene.transform.position.z - 75.0f);
            AddChunk();
            yield return new WaitForSeconds(1);
        }
    }

    void AddChunk()
    {
        if (numberOfLastChunk == 0)
        {
            while (chunksOnScene.Count < 3)
            {
                CreateChunk();
            }  
        }
        else if (numberOfLastChunk != 0 && chunksOnScene.Count < 5)
        {           
            CreateChunk();  
        }  
    }

    void CreateChunk(int typeOf = 0)
    {
        GameObject bufferChunk = Instantiate(M_Chunks[WhichChunkNeed()], new Vector3(0, 0, zNotePosCursor), Quaternion.identity) as GameObject; // нужно как то доаблять их сразу в лист chunksOnScene
        chunksOnScene.Add(bufferChunk);
        IfAddChunk();
    }

    void IfAddChunk()
    {
        zNotePosCursor += 60;
        numberOfLastChunk++;
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
        GameObject bufferChunk = chunksOnScene[0];
        chunksOnScene.Remove(bufferChunk);
        Destroy(bufferChunk);
    }

    void RemoveOldChunk(float z)
    {
        for (int i = 0; i < chunksOnScene.Count; i++)
        {
            GameObject bufferChunk = chunksOnScene[i];
            float chunkZ = bufferChunk.transform.position.z;
            if (chunkZ > z)
            {
            if (i == 0)
                break;
                for (int x = 0; x < i; x++)
                {
                    Destroy(chunksOnScene[x]);
                }
                chunksOnScene.RemoveRange(0, i);
                break;
            }
        }
    }
}
