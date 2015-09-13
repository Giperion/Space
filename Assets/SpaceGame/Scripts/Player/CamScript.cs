using UnityEngine;
using System.Collections;

public class CamScript : MonoBehaviour
{
    public GameObject objPlayer;
    private Vector3 offset;

    void Start ()
    {
        offset = transform.position - objPlayer.transform.position;
    }
	
	void Update ()
    {

    }

    void LateUpdate()
    {
        //transform.LookAt(objPlayer.GetComponent<Transform>());
        transform.position = objPlayer.transform.position + offset;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, objPlayer.transform.eulerAngles.y, objPlayer.transform.eulerAngles.z);
    }
}
