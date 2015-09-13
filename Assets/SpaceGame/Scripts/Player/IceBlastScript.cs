using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IceBlastScript : MonoBehaviour {

    List<GameObject> Trashs;
	// Use this for initialization
	void Start ()
    {
        Trashs = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    void OnCollisionEnter(Collision other)
    {
        Trashs.Remove(other.gameObject);
        print("Blast Boomed");
        foreach (GameObject element in Trashs)
        {
            Destroy(element);
        }
        Trashs.Clear();
        Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider ComponentOfList)
    {
        if (ComponentOfList.tag == "Trash")
        {
            Trashs.Add(ComponentOfList.gameObject);
            print("Add in list: Trash" + ComponentOfList.name);
        }
    }
    
    void OnTriggerExit(Collider ComponentOfList)
    {
        if (ComponentOfList.tag == "Trash")
        {
            Trashs.Remove(ComponentOfList.gameObject);
            print("Remove in list: Trash" + ComponentOfList.name);
        }
    }
}
