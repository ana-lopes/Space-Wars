using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public int currentLevel = 0;

    [SerializeField]
    GameObject loadBackground, loadText;

    public static bool pause = false;

    int loadProgress = 0;

    void Start()
    {
        Cursor.visible = false;
        AudioListener.pause = false;
    }

	void Update () {
	
        if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
	}

    public void Restart()
    {
        StartCoroutine(DisplayLoadingScreen(currentLevel));
    }

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
