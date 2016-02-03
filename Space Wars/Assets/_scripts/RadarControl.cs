using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarControl : MonoBehaviour {

    public static bool changed = false;
    public GameObject spherePrefab;
    private GameObject[] trackedObjects;
    private List<GameObject> borderObjects;
    private string tagged, radar, invisible, rlyInvisible;

    public Transform helpTransform;

    public float switchDistance;
    private bool switchObjects = false;

    public GameObject player;
    
	void Start () {
        GetPlayMode();

        trackedObjects = GameObject.FindGameObjectsWithTag(tagged);

        CreateRadarObjects();
	}
	
	// Update is called once per frame
	void Update () {
        if(changed)
        {
            UpdateObjects();

            foreach (GameObject g in borderObjects)
                Destroy(g.gameObject);

            CreateRadarObjects();

            changed = false;
        }

        for(int i = 0; i < trackedObjects.Length; i++)
        {
            helpTransform.position = new Vector3(helpTransform.position.x, trackedObjects[i].transform.position.y, helpTransform.position.z);

            if (Vector3.Distance(trackedObjects[i].transform.position, helpTransform.position) > switchDistance)
            {
                //switch to border objects
                helpTransform.LookAt(trackedObjects[i].transform.position);
                borderObjects[i].transform.position = transform.position + switchDistance * helpTransform.forward;
                borderObjects[i].transform.position = new Vector3(borderObjects[i].transform.position.x, helpTransform.position.y, borderObjects[i].transform.position.z);

                borderObjects[i].layer = LayerMask.NameToLayer(radar);
                trackedObjects[i].transform.FindChild("Sphere").gameObject.layer = LayerMask.NameToLayer(invisible);

                switchObjects = false;
            }
            else
            {
                if (!switchObjects)
                {
                    //switch back to radar objects
                    borderObjects[i].layer = LayerMask.NameToLayer(rlyInvisible);
                    trackedObjects[i].transform.FindChild("Sphere").gameObject.layer = LayerMask.NameToLayer("Radar");
                    switchObjects = true;
                }
            }
        }
	}

    void FixedUpdate()
    {
        if(player!= null)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 500, player.transform.position.z);
    }

    private void UpdateObjects()
    {
        trackedObjects = GameObject.FindGameObjectsWithTag(tagged);
    }

    private void CreateRadarObjects()
    {
        GameObject aux;
        borderObjects = new List<GameObject>();

        foreach(GameObject g in trackedObjects)
        {
            aux = Instantiate(spherePrefab, g.transform.position, Quaternion.identity) as GameObject;
            aux.layer = LayerMask.NameToLayer(invisible);
            aux.transform.parent = transform;
            borderObjects.Add(aux);
        }
    }

    private void GetPlayMode()
    {
        if (PlayerPrefs.GetInt("mode") == 1)
        {
            tagged = "enemy";
            radar = "Radar";
            invisible = "Invisible";
            rlyInvisible = invisible;
        }

        else if (PlayerPrefs.GetInt("mode") == 2 && player.tag == "Player")
        {
            tagged = "Player 2";
            radar = "Radar";
            invisible = "Invisible";
            rlyInvisible = "Hella Invisible";
        }

        else if (PlayerPrefs.GetInt("mode") == 2 && player.tag == "Player 2")
        {
            tagged = "Player";
            radar = "Radar 2";
            invisible = "Invisible 2";
            rlyInvisible = "Hella Invisible";
        }
    }
}
