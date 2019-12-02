using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.UltimateCharacterController.Events;
using Opsive.UltimateCharacterController.Traits;
using UnityEngine.AI;

public class Script_EnemigoCombate : MonoBehaviour
{

    public GameObject objetivo;
    public GameObject bala;
    public Transform firePoint;
    public GameObject muzzleFlash;
    public float salud = 50f;

    public float velocidadMovimiento = 3f;
    public bool atacando;

    //Para la sala 3
    public bool objetivoEnMirador;

    bool muerto;
    NavMeshAgent nav;
    Animator anim;
    float distanciaParada = 4f;
    string parametroActivo = "Descansa";
    // Start is called before the first frame update
    public void Awake()
    {
        EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnObjectImpact", OnImpact);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        muerto = false;
        atacando = false;
        nav = GetComponent<NavMeshAgent>();
        anim.SetBool("Descansa", true);
        nav.stoppingDistance = distanciaParada;
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetBool("Actuando", true);
        if (salud <= 0 && !muerto)
        {
            nav.enabled = false;
            anim.SetTrigger("Muere");
            muerto = true;
            transform.parent.GetComponent<ScriptSalasCombate>().numeroEnemigos -= 1;
            EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnObjectImpact", OnImpact);
        }

        if (!muerto && atacando)
        {
            RaycastHit hit;

            //obtenemos todos los valores a usar
            float distanciaAObjetivo = nav.remainingDistance;
            bool parado = nav.isStopped;
            
            //función para mirar al objetivo
            
            if (nav.destination != objetivo.transform.position && !objetivoEnMirador)
            {
                nav.SetDestination(objetivo.transform.position);
                distanciaAObjetivo = nav.remainingDistance;
            }
            if (distanciaAObjetivo <= distanciaParada)
            {
                transform.LookAt(objetivo.transform);
            }

            //obtención de la vista frontal
            Vector3 puntoOrigen = transform.position + new Vector3(0f, 1.5f, 0f);
            bool choca = Physics.Raycast(puntoOrigen, transform.forward, out hit, 15);
            Debug.DrawRay(puntoOrigen, transform.forward * 15f, Color.red);
            
            //para indicarle al animator Controller que se está ejecutando la animación actual y evitar que siga entrando
            anim.SetBool("Actuando", true);


            if(distanciaAObjetivo > distanciaParada && !anim.GetCurrentAnimatorStateInfo(0).IsName("Walk") && !objetivoEnMirador)
            {
                cambiaEstado("Camina");
                nav.speed = velocidadMovimiento;
            }else if (distanciaAObjetivo <= distanciaParada && choca && miraObjetivo(hit) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
            {
                cambiaEstado("Dispara");
                nav.speed = 0;
            }
            else if (distanciaAObjetivo <= distanciaParada && (!choca || (choca && !miraObjetivo(hit))) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                cambiaEstado("Descansa");
                nav.speed = 0;
            }


        }
        else if(!muerto && !atacando && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            cambiaEstado("Descansa");
            nav.speed = 0;
        }
    }


    void cambiaEstado(string nuevoEstado)
    {
        anim.SetBool(parametroActivo, false);
        parametroActivo = nuevoEstado;
        anim.SetBool(parametroActivo, true);
        anim.SetBool("Actuando", false);
    }

    bool miraObjetivo(RaycastHit hit)
    {
        List<string> partesDeNolan = new List<string> { "Nolan", "ORG-forearm_R", "ORG-forearm_L", "ORG-upper_arm_L", "ORG-upper_arm_R" };

        if (objetivo.name == "Aliado" && hit.transform.gameObject.name == objetivo.name)
        {
            return true;
        }
        else if(objetivo.name == "Nolan" && partesDeNolan.Contains(hit.transform.gameObject.name))
        {
            return true;
        }
        return false;

    }


    public void instanciarBala()
    {
        Instantiate(bala, firePoint.position, Quaternion.identity, transform);
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
    }

    private void OnImpact(float amount, Vector3 position, Vector3 forceDirection, GameObject attacker, Collider hitCollider)
    {
        if (!(tag == "enemigo1" && !atacando))
        {
            salud -= 10f;
        }

        if (!atacando && tag == "enemigo3")
        {
            attacker.GetComponent<CharacterVoice>().combatiendo = true;
            GameObject aliado = GameObject.FindGameObjectWithTag("aliado");

            if (!aliado.GetComponent<ScriptIA>().atacando)
            {
                aliado.GetComponent<ScriptIA>().ataqueSala3(aliado.GetComponent<ProcesamientoTexto>().enMirador);
                aliado.GetComponent<ProcesamientoTexto>().estadoAliado.SetActive(false);
            }
            GetComponentInParent<ScriptSalasCombate>().gestionaEntrada3();            
        }

    }

}
