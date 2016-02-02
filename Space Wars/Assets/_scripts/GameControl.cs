using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public int currentLevel = 0;

    [SerializeField]
    GameObject loadBackground, loadText, gameWinMenu, HUD;
    Text enemiesText;

    static public bool win = false;
    static public bool lose = false;

    public static bool pause = false;

    int loadProgress = 0;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;

        win = false;
        lose = false;
    }

    void Start()
    {
        AudioListener.pause = false;

        if (PlayerPrefs.GetInt("mode") == 1)
        {
            enemiesText = GameObject.Find("Canvas/HUD/enemies").GetComponent<Text>();
            HUD = GameObject.Find("Canvas/HUD");

            GameObject[] g = GameObject.FindGameObjectsWithTag("enemy");
            enemiesText.text = "Enemies left\n" + g.Length;

        }
        gameWinMenu = GameObject.Find("Canvas/game win");
    }

	void Update () {
	
        if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if(win)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            StartCoroutine(Fade(HUD, -0.05f));
            StartCoroutine(Fade(gameWinMenu, +0.05f));
        }
	}

    public void MultiplayerWin(string winner, GameObject HUD1, GameObject HUD2)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(Fade(HUD1, -0.05f));
        StartCoroutine(Fade(HUD2, -0.05f));
        StartCoroutine(Fade(gameWinMenu, +0.05f));

        gameWinMenu.transform.FindChild("gameover text").GetComponent<Text>().color = Color.white;        
        gameWinMenu.transform.FindChild("gameover text").GetComponent<Text>().text = "Player " + winner + " won!";
    }

    public void Restart()
    {
        StartCoroutine(DisplayLoadingScreen(currentLevel));
    }

    public void LoadMenu()
    {
        StartCoroutine(DisplayLoadingScreen(0));
    }

    public IEnumerator DisplayLoadingScreen(int level)
    {
        loadBackground.SetActive(true);
        loadText.SetActive(true);
        //loadImage.SetActive(true);

        loadText.GetComponent<Text>().text = loadProgress + "%";

        AsyncOperation async = SceneManager.LoadSceneAsync(level);

        while (!async.isDone)
        {
            loadProgress = (int)(async.progress * 100);
            loadText.GetComponent<Text>().text = loadProgress + "%";

            yield return null;
        }

        loadBackground.SetActive(false);
        loadText.SetActive(false);
        //loadImage.SetActive(false);
        loadProgress = 0;
    }

    public IEnumerator Fade(GameObject group, float incrementation)
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
}
