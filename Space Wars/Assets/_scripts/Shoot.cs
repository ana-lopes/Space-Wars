using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

    //SerializeField faz com que os objetos apareçam no inspector sem ser necessário a variável ser publica
    [SerializeField] private GameObject laser;

    [SerializeField] private float nextFire = 0;
    [SerializeField] private float fireRate = 0.5f;

    private AudioSource shootSource;
    private Light flash;
    private GameObject laserClone;

    //Variáveis para mandar tiros para o centro do ecrã. x e y representam o centro do ecrã.
    float x = Screen.width / 2f;
    float y = Screen.height / 2f;
    Vector3 direction;

    void Awake()
    {
        shootSource = GetComponent<AudioSource>();
        flash = GetComponent<Light>();
        flash.intensity = 0;
    }
	
	void Update () {

        nextFire += Time.deltaTime;

        //TODO: dar tiro apenas quando as asas estão em modo ataque
        if (Input.GetButton("Fire1") && fireRate < nextFire)
        {            
            //É lançado um raio na direção do centro do ecrã, esta direção será depois usada para mover a bala no espaço
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));

            laserClone = (GameObject)Instantiate(laser, transform.position, transform.rotation);

            direction = (ray.GetPoint(100000.0f) - laserClone.transform.position).normalized; //direção normalizada
            laserClone.GetComponent<MoveBullet>().velocity = direction * 200; //a bala é movida através do seu rigidbody (ver classe MoveBullet)

            //Debug.DrawLine(transform.position, ray.GetPoint(20), Color.red, 2, true);
 
            shootSource.Play();
            nextFire = 0;

            flash.intensity = 3.0f;
        }

        //"desliga" a luz depois do tiro dado. Math.Lerp cria uma interpolação entre os valores ao longo do tempo de modo a que a transição não seja tão brusca
        if(flash.intensity > 0)
            flash.intensity = Mathf.Lerp(flash.intensity, 0, Time.deltaTime*2);
    }
}
