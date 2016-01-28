using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

    //SerializeField faz com que os objetos apareçam no inspector sem ser necessário a variável ser publica
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioClip audioclip;

    [SerializeField] private GameObject loadBackground, loadText;
    private int loadProgress = 0;

    private Canvas canvas;
    private AudioSource source;

    private bool pause = false;
    private bool resumed = false;

    public int life = 100;

    private int gameMode;
    private GameObject HUD, HUD2, pauseMenu, gameOverMenu;

    public bool debug = false;
    public int level;

	void Start () {

        if(debug)
        {
            PlayerPrefs.SetInt("mode", level);
        }

        //O canvas é necessário para apresentar elementos de UI no ecrã. Ao buscar a referencia da canvas podemos aceder a qualquer elemento da mesma 
        //(ex: menu de pausa, HUD, entre outros).
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pauseMenu = canvas.transform.FindChild("pause").gameObject;
        gameOverMenu = canvas.transform.FindChild("game over").gameObject;

        //Cria um audiosource para reproduzir o som da explosão quando a nave explode
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = audioclip;

        gameMode = PlayerPrefs.GetInt("mode");

        if(gameMode == 1)
        {
            HUD = canvas.transform.FindChild("HUD").gameObject;
        }

        else
        {
            HUD = canvas.transform.FindChild("HUD P1").gameObject;
            HUD2 = canvas.transform.FindChild("HUD P2").gameObject;
        }
	}
	
	void Update () {

        GetInput();

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
            GameControl.pause = pause;
            if (pause) onPause();
            if (!resumed && !pause) onResume();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        //se for single mode
        if (gameMode == 1)
        {
            if (col.gameObject.CompareTag("enemy") || col.gameObject.CompareTag("ground"))
            {
                StartCoroutine(Fade(HUD, -0.05f));
                StartCoroutine(Fade(gameOverMenu, +0.05f));
                gameOverMenu.GetComponent<CanvasGroup>().interactable = true;

                Instantiate(explosion, transform.position, Quaternion.identity);
                source.Play();
                Destroy(gameObject);
            }

            else if (col.gameObject.CompareTag("enemy bullet") && life - 30 > 0)
            {
                life -= 30;
                canvas.transform.FindChild("HUD/life").GetComponent<Text>().text = life.ToString();
            }
        }

        //se for multiplayer mode
        else if(gameMode == 2)
        {
            if(tag == "Player 2")
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    HUD2.GetComponent<CanvasGroup>().alpha = 0;
                    gameOverMenu.GetComponent<CanvasGroup>().alpha = 1;
                    gameOverMenu.GetComponent<CanvasGroup>().interactable = true;

                    foreach (Transform t in gameOverMenu.transform)
                        t.gameObject.SetActive(true);

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();

                    transform.FindChild("Camera 2").SetParent(canvas.transform);
                    Destroy(gameObject);
                }

                else if (col.gameObject.CompareTag("player bullet") && life - 30 > 0)
                {
                    life -= 30;
                    canvas.transform.FindChild("HUD 2/life").GetComponent<Text>().text = life.ToString();
                }
            }

            else
            {
                if (col.gameObject.CompareTag("Player 2"))
                {
                    HUD.GetComponent<CanvasGroup>().alpha = 0;
                    gameOverMenu.GetComponent<CanvasGroup>().alpha = 1;
                    gameOverMenu.GetComponent<CanvasGroup>().interactable = true;
                    
                    foreach (Transform t in gameOverMenu.transform)
                        t.gameObject.SetActive(true);

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();

                    //para não destruir a camara
                    transform.FindChild("Camera 1").SetParent(canvas.transform);
                    Destroy(gameObject);
                }

                else if (col.gameObject.CompareTag("enemy bullet") && life - 30 > 0)
                {
                    life -= 30;
                    canvas.transform.FindChild("HUD 1/life").GetComponent<Text>().text = life.ToString();
                }
            }
        }
    }

    public void onPause()
    {
        Cursor.visible = true;

        StartCoroutine(Fade(pauseMenu, 0.05f));
        pauseMenu.GetComponent<CanvasGroup>().interactable = true;

        Time.timeScale = 0;        
        AudioListener.pause = true;
        resumed = false;

        if (gameMode == 1)
            StartCoroutine(Fade(HUD, -0.05f));
        else
        {
            StartCoroutine(Fade(HUD, -0.05f));
            StartCoroutine(Fade(HUD2, -0.05f));
        }
    }

    //Estas duas últimas funções são chamadas quando os botões da UI de pausa são chamados.
    //É preciso definir as mesmas no editor, nos proprios botões (Canvas/pause) no OnClick
    public void onResume()
    {
        Cursor.visible = false;

        StartCoroutine(Fade(pauseMenu, -0.05f));
        canvas.transform.FindChild("pause").GetComponent<CanvasGroup>().interactable = false;

        Time.timeScale = 1;
        AudioListener.pause = false;
        resumed = true;

        if (gameMode == 1)
            StartCoroutine(Fade(HUD, +0.05f));
        else
        {
            StartCoroutine(Fade(HUD, +0.05f));
            StartCoroutine(Fade(HUD2, +0.05f));
        }
    }

    public void exitMain()
    {
        //vai para o menu inicial (cena 0)
        StartCoroutine(DisplayLoadingScreen(0));
    }

    #region coroutines n shit

    IEnumerator Fade(GameObject group, float incrementation)
    {
        bool fade = true;

        foreach (Transform t in group.transform)
        {
            if (incrementation > 0)
                t.gameObject.SetActive(true);
            else
                t.gameObject.SetActive(false);
        }

        while (fade)
        {
            group.GetComponent<CanvasGroup>().alpha += incrementation;

            if (group.GetComponent<CanvasGroup>().alpha <= 0 || group.GetComponent<CanvasGroup>().alpha >= 1)
            {
                fade = false;
            }

            yield return null;
        }
    }

    //para ter um loading screen em vez de aparecer tudo preto quando carrega, again coroutine
    IEnumerator DisplayLoadingScreen(int level)
    {
        loadBackground.SetActive(true);
        loadText.SetActive(true);
        //loadImage.SetActive(true);

        loadText.GetComponent<Text>().text = loadProgress + "%";

        AsyncOperation async = SceneManager.LoadSceneAsync(level);

        while (!async.isDone)
        {
            loadProgress = (int)(async.progress * 100);
            loadText.GetComponent<Text>().text = "Loading... " + loadProgress + "%";

            yield return null;
        }

        loadBackground.SetActive(false);
        loadText.SetActive(false);
        //loadImage.SetActive(false);
        loadProgress = 0;
    }
    #endregion
}