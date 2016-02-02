using UnityEngine;
using System.Collections;

public class ShootAI : MonoBehaviour {

    [SerializeField] private GameObject laser;
    [SerializeField] private Transform target;
    [SerializeField] private Transform otherCannon;

    [SerializeField] private float nextFire = 0;
    [SerializeField] private float fireRate = 1.0f;

    float originalRate;

    AudioSource shoot;
    Light flash1, flash2;

    GameObject laserClone1, laserClone2;
    Vector3 direction = Vector3.zero;

    void Awake()
    {
        shoot = GetComponent<AudioSource>();
        flash1 = GetComponent<Light>();
        flash1.intensity = 0;

        flash2 = otherCannon.gameObject.GetComponent<Light>();
        flash2.intensity = 0;

        originalRate = fireRate;
        fireRate = Random.Range(1, originalRate);        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate < nextFire && target != null)
        {
            laserClone1 = (GameObject) Instantiate(laser, transform.position, transform.rotation);
            laserClone1.GetComponent<MoveBullet>().velocity = Vector3.forward * 200;

            laserClone2 = (GameObject)Instantiate(laser, otherCannon.position, otherCannon.rotation);
            laserClone2.GetComponent<MoveBullet>().velocity = Vector3.forward * 200;

            shoot.Play();
            nextFire = 0;

            flash1.intensity = 3.0f;
            flash2.intensity = 3.0f;

            fireRate = Random.Range(1, originalRate);
        }

        nextFire += Time.deltaTime;

        if (flash1.intensity > 0)
        {
            flash1.intensity = Mathf.Lerp(flash1.intensity, 0, Time.deltaTime * 2f);
            flash2.intensity = Mathf.Lerp(flash2.intensity, 0, Time.deltaTime * 2f);
        }
    }
}

