using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {

    public GameObject explosion;
    
	void Start () {
	
	}
	
	void Update () {

        //Bounds do mapa
        if (transform.position.x < 100)
            transform.position = new Vector3(100, transform.position.y, transform.position.z);
        if (transform.position.x > 1900)
            transform.position = new Vector3(1900, transform.position.y, transform.position.z);
        if (transform.position.z < 100)
            transform.position = new Vector3(transform.position.x, transform.position.y, 100);
        if (transform.position.z > 1900)
            transform.position = new Vector3(transform.position.x, transform.position.y, 1900);
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("enemy") || col.gameObject.CompareTag("ground"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
