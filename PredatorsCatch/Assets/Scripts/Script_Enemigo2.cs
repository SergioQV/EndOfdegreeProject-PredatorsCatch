using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_Enemigo2 : MonoBehaviour
{


    public Transform[] puntosPatrulla;
    public Transform firePoint;
    public GameObject objetivoAtaque;
    public GameObject bala;
    public GameObject muzzleFlash;
    public float anguloVision = 45f;
    public float distanciaParada = 4f;
    public float velocidadMovimiento = 3f;
    public bool muerto;

    int indicePuntoPatrulla;
    float velocidadPatrulla = 4f;
    bool patrullando;
    bool atacando;
    string parametroActivo;
    Animator anim;
    SphereCollider col;
    NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();
        indicePuntoPatrulla = 0;
        patrullando = true;
        muerto = false;
        parametroActivo = "Descansa";
        anim.SetBool("Descansa", true);
        nav.destination = puntosPatrulla[indicePuntoPatrulla].position;
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetBool("Actuando", true);

        if (!muerto)
        {
            if (patrullando)
            {
                if (nav.destination == null || nav.remainingDistance < nav.stoppingDistance)
                {

                    if (indicePuntoPatrulla == puntosPatrulla.Length - 1)
                    {
                        indicePuntoPatrulla = 0;
                    }
                    else
                    {
                        indicePuntoPatrulla++;
                    }

                }

                nav.destination = puntosPatrulla[indicePuntoPatrulla].position;

                if (patrullando && !nav.isStopped && !anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    cambiaEstado("Camina");
                    nav.speed = velocidadPatrulla;
                }
            }
            else if (atacando)
            {
                RaycastHit hit;

                //obtenemos todos los valores a usar
                float distanciaAObjetivo = nav.remainingDistance;
                bool parado = nav.isStopped;

                //función para mirar al objetivo

                if (nav.destination != objetivoAtaque.transform.position)
                {
                    nav.SetDestination(objetivoAtaque.transform.position);
                    distanciaAObjetivo = nav.remainingDistance;
                }
                if (distanciaAObjetivo <= distanciaParada)
                {
                    transform.LookAt(objetivoAtaque.transform);
                }

                //obtención de la vista frontal
                Vector3 puntoOrigen = transform.position + new Vector3(0f, 1.5f, 0f);
                bool choca = Physics.Raycast(puntoOrigen, transform.forward, out hit, 15);
                Debug.DrawRay(puntoOrigen, transform.forward * 15f, Color.red);

                //para indicarle al animator Controller que se está ejecutando la animación actual y evitar que siga entrando

                if (distanciaAObjetivo > distanciaParada && !anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    cambiaEstado("Camina");
                    nav.speed = velocidadMovimiento;
                }
                else if (distanciaAObjetivo <= distanciaParada && choca && miraObjetivo(hit) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Shooting"))
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

        if (objetivoAtaque.name == "Aliado" && hit.transform.gameObject.name == objetivoAtaque.name)
        {
            return true;
        }
        else if (objetivoAtaque.name == "Nolan" && partesDeNolan.Contains(hit.transform.gameObject.name))
        {
            return true;
        }
        return false;

    }

    public void vuelveAPatrullar()
    {
        atacando = false;
        patrullando = true;
        objetivoAtaque = null;

        indicePuntoPatrulla = 0;
        nav.destination = puntosPatrulla[indicePuntoPatrulla].position;

        for(int i = 1; i < puntosPatrulla.Length; i++)
        {
            if (Vector3.Distance(puntosPatrulla[i].position, transform.position) < nav.remainingDistance)
            {
                indicePuntoPatrulla = i;
                nav.destination = puntosPatrulla[indicePuntoPatrulla].position;
            }
        }

    }

    public void instanciarBala()
    {
        Instantiate(bala, firePoint.position, Quaternion.identity, transform);
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
    }

    public void muere()
    {
        nav.enabled = false;
        muerto = true;
        anim.SetTrigger("Muere");
    }

    private void OnTriggerStay(Collider other)
    {
        if((other.name == "Aliado" || other.name == "Nolan") && !atacando)
        {
            Vector3 direccion = other.transform.position - transform.position;
            float angulo = Vector3.Angle(direccion, transform.forward);

            if(angulo < anguloVision * 0.5)
            {
                RaycastHit hit;

                Vector3 puntoOrigen = transform.position + new Vector3(0f, 1.5f, 0f);
                if (Physics.Raycast(puntoOrigen, direccion.normalized, out hit,col.radius))
                {
                    if((hit.collider.name == "Aliado" || hit.collider.name == "Nolan"))
                    {
                        atacando = true;
                        patrullando = false;
                        objetivoAtaque = hit.collider.gameObject;
                    }
                }
            }

        }
    }
}
