using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject mainMenuGroup;
    [SerializeField] private GameObject gameModeGroup;

    [SerializeField] private GameObject loadBackground, loadText, loadImage;
    private int loadProgress;

    private bool fade = false;

	void Start () {
        if (mainMenuGroup == null || gameModeGroup == null)
        {
            mainMenuGroup = GameObject.Find("main menu");
            gameModeGroup = GameObject.Find("game mode");
        }
	}
	
	void Update () {

	}

    public void newGame()
    {
        //coroutines são tipo métodos update, estão sempre a correr até os mandares parar e é basicamente isso que está a acontecer
        StartCoroutine(Fade(mainMenuGroup, -0.05f));
        StartCoroutine(Fade(gameModeGroup, +0.05f));
    }

    public void exit()
    {
        Application.Quit();
    }

    public void singleMode()
    {
        //PlayerPrefs é um ficheiro default que guarda as preferencias do jogador, pode ser definido e guardado o que quisermos.
        //neste caso é usado para definir o modo de jogo
        PlayerPrefs.SetInt("mode", 1);

        StartCoroutine(DisplayLoadingScreen(1));
    }

    public void multiplayerMode()
    {
        PlayerPrefs.SetInt("mode", 2);

        StartCoroutine(DisplayLoadingScreen(2));
    }


    //coroutine, é para dar o efeito de fading 
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
            loadText.GetComponent<Text>().text = loadProgress + "%";

            yield return null;
        }

        loadBackground.SetActive(false);
        loadText.SetActive(false);
        //loadImage.SetActive(false);
        loadProgress = 0;
    }
}
