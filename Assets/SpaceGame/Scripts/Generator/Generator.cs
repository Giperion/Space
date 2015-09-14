using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    public GameObject[] ForestChunks;
    public GameObject PlayerObj;

    public bool isCavern;

    Vector3 posNextChunck = new Vector3(0, 0, 0);

    private bool NowCanCallPlayerPos;

    // Use this for initialization
    void Start ()
    {
        NowCanCallPlayerPos = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(callPositionPlayer());
    }
    IEnumerator callPositionPlayer()
    {
        if (NowCanCallPlayerPos)
        {
            NowCanCallPlayerPos = false;
            yield return new WaitForSeconds(2);
            print(PlayerObj.GetComponent<Transform>().position.z.ToString());
            Instantiate(ForestChunks[1], V3_whereCreateChank(PlayerObj.GetComponent<Transform>().position), Quaternion.identity);
            NowCanCallPlayerPos = true;
        }
    }
    Vector3 V3_whereCreateChank(Vector3 nowPosPlayer)
    {
        // Здесь надо позицию игрока обработать и предать положение чанка
        posNextChunck = nowPosPlayer;
        return posNextChunck;
    }
}
