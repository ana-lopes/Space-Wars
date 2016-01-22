using UnityEngine;
using System.Collections;

public class MoveBullet : MonoBehaviour {

    private Rigidbody rb;
    public float lifetime = 5.0f;
    private float speed = 200.0f;

    public Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }
    	
	void Update () {
        //As balas são destruidas ao fim do tempo de vida, deste modo não "enchem" a cena caso não atinjam nenhum objeto.
        //As balas seguem uma direção especifica caso ainda não tenham morrido.
        lifetime -= Time.deltaTime;
	
        if(lifetime < 0)
        {
            Destroy(gameObject);
        }

        //else
        //{
        //    transform.position += transform.forward * Time.deltaTime * speed;
        //}
	}
}
