using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    public GameObject[] M_Chunks; // 0 - лес, 1 - переход лес/пещера, 2 - пещера, 3 - переход пещера лес 

    int zNotePosCursor = 0;
    int numberOfLastChunk = 0;
    int typeOfLastChunk = 0; // 0 - лес, 1 - переход лес/пещера, 2 - пещера, 3 - переход пещера лес 

    void Start ()
    {
        StartCoroutine(callPositionPlayer());
    }

	void Update ()
    {  
          
    }

    IEnumerator callPositionPlayer()
    {
        yield return new WaitForSeconds(2);
        AddCunk();      
    }

    void AddCunk()
    {
        // Добавить условие на нужду создание этого чанка по времени
        if (numberOfLastChunk == 0)
        {
           Instantiate(M_Chunks[0], new Vector3(0, 0, zNotePosCursor), Quaternion.identity);    
        }
        else if (numberOfLastChunk != 0)
        {
            if (typeOfLastChunk == 0)
            {
                //рандомим тип след чанка
            }
        }

        zNotePosCursor += 10;
        numberOfLastChunk++;
    }
    void RemoveChunk()
    {

    }
}
