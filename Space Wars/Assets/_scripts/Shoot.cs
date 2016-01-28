using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

    //SerializeField faz com que os objetos apareçam no inspector sem ser necessário a variável ser publica
    [SerializeField] private GameObject laser;

    [SerializeField] private float nextFire = 0;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private Camera cam;

    private AudioSource shootSource;
    private Light flash;
    private GameObject laserClone;

    //Variáveis para mandar tiros para o centro do ecrã. x e y representam o centro do ecrã.
    float x, y;
    Vector3 direction;

    bool input;

    void Awake()
    {
        shootSource = GetComponent<AudioSource>();
        flash = GetComponent<Light>();
        flash.intensity = 0;
    }

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        //Calcular o center point para cada tipo de camara
        switch(cam.name)
        {
            case "Camera 1":
                x = cam.pixelWidth / 2f;
                y = cam.pixelHeight / 2f + cam.pixelHeight;
                break;

            case "Camera 2":
                x = cam.pixelWidth / 2f;
                y = cam.pixelHeight / 2f;
                break;

            default:
                x = Screen.width / 2;
                y = Screen.height / 2;
                break;
        }
    }
	
	void Update () {

        nextFire += Time.deltaTime;

        //Escolhe o tipo de input dependendo do objeto pai (verifica se é o jogador 1 ou 2 a carregar e armazena essa info num boleano)
        if (tag == "Player 2")
            input = Input.GetButton("Fire1");
        else
            if(XWingAnimation.isInAttackMode) input = Input.GetKeyDown(KeyCode.Space); //se as asas estiverem abertas dispara

        if (!GameControl.pause && input && fireRate < nextFire)
        {            
            //É lançado um raio na direção do centro do ecrã, esta direção será depois usada para mover a bala no espaço
            Ray ray = cam.ScreenPointToRay(new Vector3(x, y, 0));

            laserClone = (GameObject)Instantiate(laser, transform.position, transform.rotation);

            direction = (ray.GetPoint(100000.0f) - laserClone.transform.position).normalized; //direção normalizada
            laserClone.GetComponent<MoveBullet>().velocity = direction * 250; //a bala é movida através do seu rigidbody (ver classe MoveBullet)

            Debug.DrawLine(transform.position, ray.GetPoint(20), Color.red, 2, true);
 
            shootSource.Play();
            nextFire = 0;

            flash.intensity = 2.0f;
        }

        //"desliga" a luz depois do tiro dado. Math.Lerp cria uma interpolação entre os valores ao longo do tempo de modo a que a transição não seja tão brusca
        if(flash.intensity > 0)
            flash.intensity = Mathf.Lerp(flash.intensity, 0, Time.deltaTime*2);
    }
}
