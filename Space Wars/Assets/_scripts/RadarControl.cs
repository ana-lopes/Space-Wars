using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarControl : MonoBehaviour {

    public static bool changed = false;
    public GameObject spherePrefab;
    private GameObject[] trackedObjects;
    private List<GameObject> borderObjects;
    private string tagged, radar;

    public Transform helpTransform;

    public float switchDistance;
    private bool switchObjects = false;

    public Transform player;
    
	void Start () {
       
        trackedObjects = GameObject.FindGameObjectsWithTag("enemy");

        GetPlayMode();
        CreateRadarObjects();
	}
	
	// Update is called once per frame
	void Update () {
        if(changed)
        {
            Debug.Log("hel ye");
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
                trackedObjects[i].transform.FindChild("Sphere").gameObject.layer = LayerMask.NameToLayer("Invisible");

                switchObjects = false;
            }
            else
            {
                if (!switchObjects)
                {
                    //switch back to radar objects
                    borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                    trackedObjects[i].transform.FindChild("Sphere").gameObject.layer = LayerMask.NameToLayer(radar);
                    switchObjects = true;
                }
            }
        }
	}

    void FixedUpdate()
    {
        if(player!= null)
            transform.position = new Vector3(player.position.x, player.position.y + 500, player.position.z);
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
            aux.layer = LayerMask.NameToLayer("Invisible");
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
        }

        else if (PlayerPrefs.GetInt("mode") == 2 && tag == "Player")
        {
            tagged = "Player 2";
            radar = "Radar 1";
        }

        else if (PlayerPrefs.GetInt("mode") == 2 && tag == "Player 2")
        {
            tagged = "Player";
            radar = "Radar 2";
        }
    }
}
