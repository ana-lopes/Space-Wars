using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

    public GameObject laser;
    AudioSource shoot;
    public float nextFire = 0;
    public float fireRate = 0.5f;

    void Awake()
    {
        shoot = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        nextFire += Time.deltaTime;

        if (Input.GetButton("Fire1") && fireRate < nextFire)
        {
            Instantiate(laser, transform.position, transform.rotation);

            //Quaternion.Euler(laser.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));

            shoot.Play();
            nextFire = 0;
        }
    }
}
