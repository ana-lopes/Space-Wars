using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Collisions : MonoBehaviour {

    Canvas canvas;

    public int life = 100;
    public GameObject explosion;
    bool pause = false;
    bool resumed = false;
    
	void Start () {

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>() ;
	}
	
	void Update () {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            if(pause) onPause();
            if(resumed && !pause) onResume();
        }

        if(life <= 0)
        {
            SceneManager.LoadScene(0);
        }

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

        else if(col.gameObject.CompareTag("enemy bullet") && life - 30> 0)
        {
            life -= 30;
            canvas.transform.FindChild("HUD/life").GetComponent<Text>().text = life.ToString();
        }
    }

    public void onPause()
    {
        Time.timeScale = 0;
        canvas.transform.FindChild("pause").GetComponent<CanvasGroup>().alpha = 1;
        AudioListener.pause = true;
        resumed = false;       
    }

    public void onResume()
    {
        canvas.transform.FindChild("pause").GetComponent<CanvasGroup>().alpha = 0;
        Time.timeScale = 1;
        AudioListener.pause = false;
        resumed = true;
    }

    public void exit()
    {
        Application.Quit();
    }
}
