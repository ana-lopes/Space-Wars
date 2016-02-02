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
    private GameControl gamecontrol;

    private bool pause = false;
    private bool resumed = false;

    public int life = 100;

    private int gameMode;
    private GameObject HUD, HUD2, pauseMenu, gameOverMenu, gameWinMenu;

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
        gameWinMenu = canvas.transform.FindChild("game win").gameObject;

        gamecontrol = GameObject.Find("GameController").GetComponent<GameControl>();

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
            if (gameMode == 1)
            {
                HUD.GetComponent<CanvasGroup>().alpha = 0;

                foreach (Transform t in gameOverMenu.transform)
                    t.gameObject.SetActive(true);

                Instantiate(explosion, transform.position, Quaternion.identity);
                source.Play();

                transform.FindChild("CameraRig").SetParent(canvas.transform);
                GameObject.Find("infinite terrain").GetComponent<TerrainGenerator>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Destroy(gameObject);
            }

            else
            {
                GameControl.lose = true;

                if(tag == "Player 2")
                {
                    HUD2.GetComponent<CanvasGroup>().alpha = 0;

                    //foreach (Transform t in gameOverMenu.transform)
                    //    t.gameObject.SetActive(true);

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    gamecontrol.MultiplayerWin("1", HUD, HUD2);

                    transform.FindChild("Camera 2").SetParent(canvas.transform);
                    Destroy(gameObject);
                }

                else
                {
                    HUD.GetComponent<CanvasGroup>().alpha = 0;

                    //foreach (Transform t in gameOverMenu.transform)
                    //    t.gameObject.SetActive(true);

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    gamecontrol.MultiplayerWin("2", HUD, HUD2);

                    //para não destruir a camara
                    transform.FindChild("Camera 1").SetParent(canvas.transform);
                    Destroy(gameObject);
                }
            }
        }
	}

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameControl.win == false && GameControl.lose == false)
        {
            pause = !pause;
            if (pause) onPause();
            if (!resumed && !pause) onResume();
        }

    }

    void OnCollisionEnter(Collision col)
    {
        //se for single mode
        if (gameMode == 1)
        {
            if (col.gameObject.CompareTag("enemy") || col.gameObject.CompareTag("ground") || col.gameObject.CompareTag("star destroyer"))
            {
                HUD.GetComponent<CanvasGroup>().alpha = 0;

                foreach (Transform t in gameOverMenu.transform)
                    t.gameObject.SetActive(true);

                Instantiate(explosion, transform.position, Quaternion.identity);
                source.Play();

                transform.FindChild("CameraRig").SetParent(canvas.transform);
                GameObject.Find("infinite terrain").GetComponent<TerrainGenerator>().enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Destroy(gameObject);
            }

            else if (col.gameObject.CompareTag("enemy bullet"))
            {
                Debug.Log("boop");

                life -= 10;
                Destroy(col.gameObject);

                if (life > 0)
                {
                    canvas.transform.FindChild("HUD/life").localScale = new Vector3(life * 0.01f, 1, 1);

                    //muda a cor consoante a vida
                    if (life > 90)
                        canvas.transform.FindChild("HUD/life").GetComponent<Image>().color = Color.green;
                    else if (life < 90 && life > 50)
                        canvas.transform.FindChild("HUD/life").GetComponent<Image>().color = Color.yellow;
                    else if (life < 50)
                        canvas.transform.FindChild("HUD/life").GetComponent<Image>().color = Color.red;
                }
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

                    foreach (Transform t in gameOverMenu.transform)
                        t.gameObject.SetActive(true);

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();
                    Cursor.visible = true;

                    transform.FindChild("Camera 2").SetParent(canvas.transform);
                    Destroy(gameObject);
                }

                else if (col.gameObject.CompareTag("star destroyer"))
                {
                    HUD2.GetComponent<CanvasGroup>().alpha = 0;

                    foreach (Transform t in gameOverMenu.transform)
                        t.gameObject.SetActive(true);

                    gameOverMenu.transform.FindChild("gameover text").GetComponent<Text>().text = "Player 1 wins!";

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();
                    Cursor.visible = true;

                    transform.FindChild("Camera 2").SetParent(canvas.transform);
                    Destroy(gameObject);
                }

                else if (col.gameObject.CompareTag("player bullet"))
                {
                    life -= 10;
                    Destroy(col.gameObject);

                    if (life > 0)
                    {
                        canvas.transform.FindChild("HUD P2/life").localScale = new Vector3(life * 0.01f, 1, 1);

                        if (life > 90)
                            canvas.transform.FindChild("HUD P2/life").GetComponent<Image>().color = Color.green;
                        else if (life < 90 && life > 50)
                            canvas.transform.FindChild("HUD P2/life").GetComponent<Image>().color = Color.yellow;
                        else if (life < 50)
                            canvas.transform.FindChild("HUD P2/life").GetComponent<Image>().color = Color.red;
                    }
                }
            }

            else
            {
                if (col.gameObject.CompareTag("Player 2"))
                {
                    HUD.GetComponent<CanvasGroup>().alpha = 0;
                    
                    foreach (Transform t in gameOverMenu.transform)
                        t.gameObject.SetActive(true);

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();
                    Cursor.visible = true;

                    //para não destruir a camara
                    transform.FindChild("Camera 1").SetParent(canvas.transform);
                    Destroy(gameObject);
                }

                else if (col.gameObject.CompareTag("star destroyer"))
                {
                    HUD2.GetComponent<CanvasGroup>().alpha = 0;

                    foreach (Transform t in gameOverMenu.transform)
                        t.gameObject.SetActive(true);

                    gameOverMenu.transform.FindChild("gameover text").GetComponent<Text>().text = "Player 2 wins!";

                    Instantiate(explosion, transform.position, Quaternion.identity);
                    source.Play();
                    Cursor.visible = true;

                    transform.FindChild("Camera 1").SetParent(canvas.transform);
                    Destroy(gameObject);
                }

                else if (col.gameObject.CompareTag("enemy bullet"))
                {
                    life -= 10;
                    Destroy(col.gameObject);

                    if (life > 0)
                    {
                        canvas.transform.FindChild("HUD P1/life").localScale = new Vector3(life * 0.01f, 1, 1);

                        if (life > 90)
                            canvas.transform.FindChild("HUD P1/life").GetComponent<Image>().color = Color.green;
                        else if (life < 90 && life > 50)
                            canvas.transform.FindChild("HUD P1/life").GetComponent<Image>().color = Color.yellow;
                        else if (life < 50)
                            canvas.transform.FindChild("HUD P1/life").GetComponent<Image>().color = Color.red;
                    }
                }
            }
        }
    }

    public void onPause()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        StartCoroutine(gamecontrol.Fade(pauseMenu, 0.05f));
        pauseMenu.GetComponent<CanvasGroup>().interactable = true;

        Time.timeScale = 0;        
        AudioListener.pause = true;
        resumed = false;

        if (gameMode == 1)
            StartCoroutine(gamecontrol.Fade(HUD, -0.05f));
        else
        {
            StartCoroutine(gamecontrol.Fade(HUD, -0.05f));
            StartCoroutine(gamecontrol.Fade(HUD, -0.05f));
        }
    }

    //Estas duas últimas funções são chamadas quando os botões da UI de pausa são chamados.
    //É preciso definir as mesmas no editor, nos proprios botões (Canvas/pause) no OnClick
    public void onResume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(gamecontrol.Fade(pauseMenu, -0.05f));
        canvas.transform.FindChild("pause").GetComponent<CanvasGroup>().interactable = false;

        Time.timeScale = 1;
        AudioListener.pause = false;
        resumed = true;

        if (gameMode == 1)
            StartCoroutine(gamecontrol.Fade(HUD, +0.05f));
        else
        {
            StartCoroutine(gamecontrol.Fade(HUD, +0.05f));
            StartCoroutine(gamecontrol.Fade(HUD, +0.05f));
        }
    }

    public void exitMain()
    {
        //vai para o menu inicial (cena 0)
        StartCoroutine(gamecontrol.DisplayLoadingScreen(0));
    }
}