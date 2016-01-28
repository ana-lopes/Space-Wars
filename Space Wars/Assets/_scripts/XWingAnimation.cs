using UnityEngine;
using System.Collections;

public class XWingAnimation : MonoBehaviour {

    //Serialize Field faz com que as variáveis apareçam no inspector do unity sem terem de ser public (podem ser private neste caso)
    //não é necessário nem importante mas são boas práticas de programação

    [SerializeField] private bool animate = false;
    [SerializeField] private static bool open = false;

    [SerializeField] private GameObject[] RightUpWing;
    [SerializeField] private GameObject[] RightDownWing;
    [SerializeField] private GameObject[] LeftUpWing;
    [SerializeField] private GameObject[] LeftDownWing;
	
	//Fixed Update é usado para as transformações físicas, mas também podem ser feitas no Update (dependendo das coisas i guess)
	void FixedUpdate () {
	
        if(animate)
        {
            //se já estiverem abertas fecha e vice-versa
            if(!open)
            {
                //verifica se as asas já estão no angulo certo e altera a variável que faz "trigger" à animação
                //é feita a transformação a todos os objetos de cada asa no referencial local (de modo a que não dependa da posição do objeto no espaço)
                if (RightUpWing[0].transform.localEulerAngles.z < 194)
                {
                    //Quaternion.euler transforma a rotação num Vector3 (ninguém sabe o que raio é o quaternion)
                    //Quaternion.Slerp faz uma transformação de um vetor inicial para um final ao longo do tempo de modo a que não seja tão brusco

                    foreach (GameObject ru in RightUpWing)
                        ru.transform.localRotation = Quaternion.Slerp(ru.transform.localRotation,
                            Quaternion.Euler(ru.transform.localRotation.eulerAngles.x, ru.transform.localRotation.eulerAngles.y, 195), 2f * Time.deltaTime);

                    foreach (GameObject rd in RightDownWing)
                        rd.transform.localRotation = Quaternion.Slerp(rd.transform.localRotation,
                            Quaternion.Euler(rd.transform.localRotation.eulerAngles.x, rd.transform.localRotation.eulerAngles.y, 195), 2f * Time.deltaTime);

                    foreach (GameObject lu in LeftUpWing)
                        lu.transform.localRotation = Quaternion.Slerp(lu.transform.localRotation,
                            Quaternion.Euler(lu.transform.localRotation.eulerAngles.x, lu.transform.localRotation.eulerAngles.y, 15), 2f * Time.deltaTime);

                    foreach (GameObject ld in LeftDownWing)
                        ld.transform.localRotation = Quaternion.Slerp(ld.transform.localRotation,
                            Quaternion.Euler(ld.transform.localRotation.eulerAngles.x, ld.transform.localRotation.eulerAngles.y, 15), 2f * Time.deltaTime);
                }

                else {
                    animate = false;
                    open = true;
                }
            }

            else
            {
                if (RightUpWing[0].transform.localEulerAngles.z > 181)
                {
                    foreach (GameObject ru in RightUpWing)
                        ru.transform.localRotation = Quaternion.Slerp(ru.transform.localRotation,
                            Quaternion.Euler(ru.transform.localRotation.eulerAngles.x, ru.transform.localRotation.eulerAngles.y, 180), 2f * Time.deltaTime);

                    foreach (GameObject rd in RightDownWing)
                        rd.transform.localRotation = Quaternion.Slerp(rd.transform.localRotation,
                            Quaternion.Euler(rd.transform.localRotation.eulerAngles.x, rd.transform.localRotation.eulerAngles.y, 180), 2f * Time.deltaTime);

                    foreach (GameObject lu in LeftUpWing)
                        lu.transform.localRotation = Quaternion.Slerp(lu.transform.localRotation,
                            Quaternion.Euler(lu.transform.localRotation.eulerAngles.x, lu.transform.localRotation.eulerAngles.y, 0), 2f * Time.deltaTime);

                    foreach (GameObject ld in LeftDownWing)
                        ld.transform.localRotation = Quaternion.Slerp(ld.transform.localRotation,
                            Quaternion.Euler(ld.transform.localRotation.eulerAngles.x, ld.transform.localRotation.eulerAngles.y, 0), 2f * Time.deltaTime);
                }

                else {
                    animate = false;
                    open = false;
                }
            }
        }
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && !animate)
        {
            animate = true;
        }
    }

    public bool Animate
    {
        get { return animate; }
        set { animate = value; }
    }

    public static bool isInAttackMode
    {
        get { return open; }
    }
}
