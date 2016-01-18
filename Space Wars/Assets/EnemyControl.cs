using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour {

    GameObject explosion;
    Canvas canvas;

	void Start () {

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
	}
	
    public void OnColliderEnter(Collider col)
    {
        if(col.gameObject.CompareTag("player bullet"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            canvas.transform.FindChild("HUD/tie fighter").GetComponent<Text>().text = "Tie Fighters: " + enemies.Length;

            Destroy(gameObject);
        }
    }
}
