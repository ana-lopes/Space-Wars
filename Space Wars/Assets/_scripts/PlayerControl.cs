using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    //SerializeField faz com que os objetos apareçam no inspector sem ser necessário a variável ser publica
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioClip audioclip;

    private Canvas canvas;
    private AudioSource source;

    private bool pause = false;
    private bool resumed = false;

    public int life = 100;
    
	void Start () {

        //O canvas é necessário para apresentar elementos de UI no ecrã. Ao buscar a referencia da canvas podemos aceder a qualquer elemento da mesma 
        //(ex: menu de pausa, HUD, entre outros).
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        //Cria um audiosource para reproduzir o som da explosão quando a nave explode
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = audioclip;
	}
	
	void Update () {
        
        if(life <= 0)
        {
           //dying code?
        }
	}

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            if (pause) onPause();
            if (resumed && !pause) onResume();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("enemy") || col.gameObject.CompareTag("ground"))
        {
            //Instantiate(explosion, transform.position, Quaternion.identity);
            //source.Play();
            //Destroy(gameObject);
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

    //Estas duas últimas funções são chamadas quando os botões da UI de pausa são chamados.
    //É preciso definir as mesmas no editor, nos proprios botões (Canvas/pause) no OnClick
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