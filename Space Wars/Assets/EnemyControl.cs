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
	
    public void OnColliderEnter(Collider col)
    {
        if(col.gameObject.CompareTag("player bullet"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            canvas.transform.FindChild("HUD/tie fighter").GetComponent<Text>().text = "Tie Fighters: " + enemies.Length;

            source.Play();

            Destroy(gameObject);
        }
    }
}
