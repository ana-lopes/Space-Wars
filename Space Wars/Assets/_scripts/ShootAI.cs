using UnityEngine;
using System.Collections;

public class ShootAI : MonoBehaviour {

    public GameObject laser;
    AudioSource shoot;
    public float nextFire = 0;
    public float fireRate = 1.0f;
    Light flash;

    void Awake()
    {
        shoot = GetComponent<AudioSource>();
        flash = GetComponent<Light>();
        flash.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        nextFire += Time.deltaTime;

        if (fireRate < nextFire)
        {
            Instantiate(laser, transform.position, transform.rotation);
            shoot.Play();
            nextFire = 0;

            flash.intensity = 3.0f;
            fireRate = Random.Range(0f, 2.0f);
        }

        flash.intensity = Mathf.Lerp(flash.intensity, 0, Time.deltaTime*2f);
    }
}

