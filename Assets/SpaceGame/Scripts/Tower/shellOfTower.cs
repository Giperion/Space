using UnityEngine;
using System.Collections;

public class shellOfTower : MonoBehaviour {

    private GameObject PlayerForAttack;
    float localTime;
    float speedShell;
	// Use this for initialization
	void Start ()
    {
        PlayerForAttack = GameObject.FindGameObjectWithTag("Player");
        if (PlayerForAttack == null)
        {
            PlayerForAttack = new GameObject();
        }
        localTime = 0f;
        StartCoroutine("DestrForTime");

    }
	
	// Update is called once per frame
	void Update ()
    {
        localTime += Time.deltaTime;
        float ForcePlayer = PlayerForAttack.GetComponent<PlayerSpace>().HowManyForce;
        if (ForcePlayer >= 1)
        {
            speedShell = localTime / 7 * ForcePlayer;
        }
        else
        {
            speedShell = localTime / 7 * 1;
        }
        
    }
    void FixedUpdate()
    {
        Vector3 PlayerForAttackPosition = new Vector3(PlayerForAttack.GetComponent<Transform>().position.x, PlayerForAttack.GetComponent<Transform>().position.y + 0.5f, PlayerForAttack.GetComponent<Transform>().position.z);
        transform.position = Vector3.Lerp(this.gameObject.GetComponent<Transform>().position, PlayerForAttackPosition, localTime / 40);
    }
    void OnCollisionEnter(Collision other)
    {
        print("228 it's worked!");
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerSpace>().HitHP(5);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag != "Tower")
        {
            Destroy(this.gameObject);
        }      
    }
    IEnumerator DestrForTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
