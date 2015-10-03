using UnityEngine;
using System.Collections;

public class TowerEnemy : MonoBehaviour
{
    public Object PrefabShellForTower;
    Vector3 positionForShell;
    bool stayOfReload = true;

    void Start ()
    {
        positionForShell = new Vector3 (this.gameObject.GetComponent<Transform>().position.x, this.gameObject.GetComponent<Transform>().position.y + 3, this.gameObject.GetComponent<Transform>().position.z);
    }

	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (stayOfReload == true)
            {
                stayOfReload = false;
                StartCoroutine("TimeForFire");
            }
        }
    }
    IEnumerator TimeForFire()
    {
        CreateShell();
        yield return new WaitForSeconds(2);
        stayOfReload = true;
    }

    void CreateShell()
    {
        Instantiate(PrefabShellForTower, positionForShell, Quaternion.identity);
    }
}
