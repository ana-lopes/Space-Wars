using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour {

    Rigidbody rb;
    float speed = 200.0f;
    public float lifetime = 20.0f;
    
	void Start () {
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = -Vector3.forward * 200;
	}
	
	// Update is called once per frame
	void Update () {
        lifetime -= Time.deltaTime;
	
        if(lifetime < 0)
        {
            Destroy(gameObject);
        }

        else
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
	}
}
