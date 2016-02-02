using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour {

    public GameObject explosion;
    private AudioSource source;
    public AudioClip clip;
    Canvas canvas;

	void Start () {

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = clip;
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("player bullet"))
        {
            Debug.Log("boop");

            Instantiate(explosion, transform.position, Quaternion.identity);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            canvas.transform.FindChild("HUD/enemies").GetComponent<Text>().text = "Enemies left\n " + (enemies.Length - 1);

            source.Play();

            if (enemies.Length - 1 == 0)
                GameControl.win = true;

            RadarControl.changed = true;

            Destroy(gameObject);
        }

        else if (col.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            source.Play();

            Destroy(gameObject);
        }
    }
}
