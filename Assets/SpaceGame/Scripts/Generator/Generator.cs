using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

    [Header("Префабы чанков")]
    public GameObject[] M_Chunks; // 0 - лес, 1 - переход лес/пещера, 2 - пещера, 3 - переход пещера лес 
    [Header("Чанки на сцене")]
    public List<GameObject> chunksOnScene; // Добавляем чанки в лист, не исп вроде

    [Header("Префабы для чанков рандомного положения")]
    public GameObject[] PrefabsOfForest;
    public GameObject[] PrefabsOfTransitionForestInputCave;
    public GameObject[] PrefabsOfCave;
    public GameObject[] PrefabsOfTransitionCaveOutputForest;
    [Header("Префабы чёткого назночения")]
    public GameObject[] SpecialThink;
    [Header("Префабы бонусов")]
    public GameObject[] Bonuses;

    GameObject PlayerOnScene;
    GameObject bufferLastChunk;

    float zNotePosCursor = 0;
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
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Ограничитель создания чанка
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

    // Создание чанка на сцене по типу int
    void CreateChunk(int typeOf = 0)
    {
        bufferLastChunk = Instantiate(M_Chunks[WhichChunkNeed()], new Vector3(0, 0, zNotePosCursor), Quaternion.identity) as GameObject;
        chunksOnScene.Add(bufferLastChunk);
        IfAddChunk();
    }

    // Вызов : 
    // 1 Генирация префабов из массива соответсвующих типу чанка
    // 2 Генерация обязательных объектов на чанке по его типу
    // 3 Генерация бонусов на чанке
    void GenerateThinksOnChunk()
    {
        GenerateBonusesOnChunk();
        switch (typeOfLastChunk)
        {
            case 0:
                GenerateThinksRandom(PrefabsOfForest);
                break;
            case 1:
                GenerateThinksRandom(PrefabsOfTransitionForestInputCave);
                GenerateSpecialThinks(1);
                break;
            case 2:
                GenerateThinksRandom(PrefabsOfCave);
                break;
            case 3:
                GenerateThinksRandom(PrefabsOfTransitionCaveOutputForest);
                break;
            default:
                print("Нет такого типа чанков, ошибка при генерации префабов");
                break;
        }
    }

    // Генирация префабов из массива соответсвующих типу чанка
    void GenerateThinksRandom(GameObject[] PrefabsArray, bool AddChildForBufferLastChunk = true, float Xmin = -10, float Xmax = 10, float Ymin = 0, float Ymax = 0, float Zmin = 0, float Zmax = 60)
    {
        for (int i = 0; i < PrefabsArray.Length; i++)
        {
            GameObject bufferPrefab = Instantiate(PrefabsArray[i], new Vector3(Random.Range(Xmin, Xmax), Random.Range(Ymin, Ymax), zNotePosCursor + Random.Range(Zmin, Zmax)), Quaternion.identity) as GameObject;
            if (AddChildForBufferLastChunk) bufferPrefab.transform.parent = bufferLastChunk.transform;
        }
    }

    // Генерация обязательных объектов на чанке по его типу
    void GenerateSpecialThinks(int listForRealization)
    {
        switch (listForRealization)
        {
            case 0:
                break;
            case 1:
                GameObject bufferPrefab = Instantiate(SpecialThink[0], new Vector3(0, 5, zNotePosCursor + 40), Quaternion.identity) as GameObject;
                bufferPrefab.transform.parent = bufferLastChunk.transform;
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                print("Вы вызвали не существующий лист специальных префабов");
                break;
        }
        
    }

    // Генерация бонусов на чанке
    void GenerateBonusesOnChunk(int minCount = 0, int maxCount = 5)
    {
        int CountBonusesOnChunk = Random.Range(minCount-1, maxCount);
        for (int i = CountBonusesOnChunk; i > 0; i--)
        {
            GameObject bufferPrefab = Instantiate(Bonuses[Random.Range(0, Bonuses.Length)], new Vector3(Random.Range(-9, 9), Random.Range(5, 13), zNotePosCursor + Random.Range(0, 60)), Quaternion.identity) as GameObject;
            bufferPrefab.transform.parent = bufferLastChunk.transform;
        }
    }

    // Движение курсора и номер последнего чанка
    void IfAddChunk()
    {
        GenerateThinksOnChunk();
        zNotePosCursor += 59.98f;
        numberOfLastChunk++;
    }

    // Метод используемый для генерации чанка, возвращает тип чанка
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

    // Авто удаление старых чанков (не используется)
    void RemoveOldChunk()
    {
        GameObject bufferChunk = chunksOnScene[0];
        chunksOnScene.Remove(bufferChunk);
        Destroy(bufferChunk);
    }

    //Удаление чанков относительно виверны
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
