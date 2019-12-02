using Opsive.UltimateCharacterController.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScriptGato : MonoBehaviour
{

    public float salud;
    public bool muerto;
    public float distanciaLento;
    public float distanciaParada;
    public float velocidadCorre;
    public float velocidadAnda;
    public GameObject objetivo;
    public GameObject gameCleared;

    Animator anim;
    bool alerta;
    bool atacando;
    bool triggerParaActivado;
    bool triggerCorreActivado;
    bool triggerAndaActivado;
    bool triggerAtaqueActivado;
    NavMeshAgent nav;


    private void Awake()
    {
        EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnObjectImpact", OnImpact);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!muerto && atacando)
        {
            float distanciaAObjetivo = nav.remainingDistance;
            bool parado = nav.isStopped;

            //función para mirar al objetivo

            if (nav.destination != objetivo.transform.position)
            {
                nav.SetDestination(objetivo.transform.position);
                distanciaAObjetivo = nav.remainingDistance;
            }

            if (distanciaAObjetivo <= distanciaParada)
            {
                transform.LookAt(objetivo.transform);
            }

            AnimatorStateInfo estadoAnimacion = anim.GetCurrentAnimatorStateInfo(0);

            if (distanciaAObjetivo > distanciaLento)
            {
                triggerAtaqueActivado = false;
                triggerAndaActivado = false;
                triggerParaActivado = false;
                if (!estadoAnimacion.IsName("Corre") && !triggerCorreActivado)
                {
                    nav.speed = velocidadCorre;
                    anim.SetTrigger("corre");
                    triggerCorreActivado = true;
                }
                else if (estadoAnimacion.IsName("Corre"))
                {
                    triggerCorreActivado = false;
                }
            }
            else if(distanciaAObjetivo <= distanciaLento && distanciaAObjetivo > distanciaParada)
            {
                triggerAtaqueActivado = false;
                triggerCorreActivado = false;
                triggerParaActivado = false;
                if (!estadoAnimacion.IsName("Camina") && !triggerAndaActivado)
                {
                    nav.speed = velocidadAnda;
                    anim.SetTrigger("anda");
                    triggerAndaActivado = true;
                }
                else if (estadoAnimacion.IsName("Camina"))
                {
                    triggerAndaActivado = false;
                }
            }
            else
            {
                nav.speed = 0;

                triggerAndaActivado = false;
                triggerCorreActivado = false;
                if (!estadoAnimacion.IsName("IdleErguido") && !estadoAnimacion.IsName("Ataque") && !triggerParaActivado)
                {
                    anim.SetTrigger("para");
                    triggerParaActivado = true;
                }
                else if (estadoAnimacion.IsName("IdleErguido") && !triggerAtaqueActivado)
                {
                    if (triggerParaActivado)
                    {
                        triggerParaActivado = false;
                    }
                    anim.SetTrigger("ataca");
                    triggerAtaqueActivado = true;
                }
                else if (estadoAnimacion.IsName("Ataque"))
                {
                    triggerAtaqueActivado = false;
                }
            }

        }

    }


    public void alertaGato()
    {
        if (!alerta)
        {
            anim.SetTrigger("EntraGente");
            alerta = true;
        }
    }

    public void recibeDisparos()
    {
        salud -= 10f;

        if(salud <= 0)
        {
            muerto = true;
            anim.SetTrigger("muere");
            EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnObjectImpact", OnImpact);
            nav.enabled = false;
            gameCleared.SetActive(true);
        }
        else if (!atacando)
        {
            atacando = true;

            if(objetivo == null) //Si no tiene ningún objetivo, por defecto atacará al jugador 
            {
                objetivo = GameObject.FindGameObjectWithTag("Player");
            }

            nav.SetDestination(objetivo.transform.position);
        }
    }

    private void OnImpact(float amount, Vector3 position, Vector3 forceDirection, GameObject attacker, Collider hitCollider)
    {
        recibeDisparos();
    }

}
