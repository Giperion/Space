using UnityEngine;
using System.Collections;

public class B_SpeedScript : MonoBehaviour
{
	void Start ()
    {
        
    }
	
	void Update ()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider PlayerCol)
    {
        if (PlayerCol.tag == "Player")
        { 
            Destroy(this.gameObject);
            PlayerCol.gameObject.GetComponent<PlayerSpace>().UsingBonusSpeed();
        }
    }
}
