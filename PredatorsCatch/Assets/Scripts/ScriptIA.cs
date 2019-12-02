using Opsive.UltimateCharacterController.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Opsive.UltimateCharacterController.Character;
using UnityEngine.UI;

public class ScriptIA : MonoBehaviour
{
    public GameObject amigo;
    public GameObject objetivo;
    public GameObject gameOver;

    public float altura = 1.5f;
    public float salud;
    public Transform firePoint;
    public GameObject prefabBala;
    public GameObject muzzleFlash;
    public NavMeshAgent nav;
    public string entorno;

    /*ELEMENTOS PARA LA SALA 1*/
    public string accionActual;
    public List<Transform> EnemigosSala1;
    /**************************/

    /*ELEMENTOS PARA LA SALA 2*/
    public List<Transform> objetivosSala2;
    public bool enReto;
    public bool enEscondite;
    public bool reto2Terminado;
    /**************************/


    /*ELEMENTOS PARA LA SALA 3*/
    public List<GameObject> enemigosSala3;
    public bool enMirador;
    public bool reto3Terminado;
    /**************************/


    /*ELEMENTOS PARA LA SALA FINAL*/
    public string accionSalaFinal;
    public bool enPosicionLejana;
    List<GameObject> posicionesLejanas;
    GameObject gato;

    public bool atacando;
    public bool siguiendo;

    public float distanciaMedia = 4f;
    public float distanciaMinima = 2f;
    public float velocidadAlta = 8f;
    public float velocidadBaja = 4f;

    bool muerto;
    bool enPosicionDeAtaque;
    string parametroActivo = "DescansaPistola";
    Animator anim;

    List<string> partesDelAliado = new List<string> { "Nolan", "ORG-forearm_R", "ORG-forearm_L", "ORG-upper_arm_L", "ORG-upper_arm_R" };

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.SetBool("DescansaPistola", true);
        objetivo = amigo;
        siguiendo = true;
        atacando = false;
        enPosicionDeAtaque = false;
        reto2Terminado = false;
        accionActual = "ataca";
        entorno = "general";
        accionSalaFinal = "libertad";
    }

    // Update is called once per frame
    void Update()
    {
        if (!muerto)
        {
            RaycastHit hit;

            //obtenemos todos los valores a usar
            float distanciaAObjetivo = nav.remainingDistance;
            bool parado = nav.isStopped;
            nav.stoppingDistance = distanciaMinima;


            if (nav.destination != objetivo.transform.position && !enMirador && !(accionSalaFinal == "lejos" && enPosicionLejana)) //para renovar el destino
            {
                nav.SetDestination(objetivo.transform.position);
                distanciaAObjetivo = nav.remainingDistance;
            }

            if (distanciaAObjetivo <= distanciaMinima && (objetivo.tag == "Player" || objetivo.tag == "enemigo1" || objetivo.tag == "enemigo3") || objetivo.tag == "boss")
            {
                transform.LookAt(objetivo.transform);
            }
            else
            {
                enPosicionDeAtaque = false;
                //nav.isStopped = false;
            }

            //si tiene la orden de curar, está en la sala 1 y el protanonista tiene la salud baja
            if (accionActual == "cura" && entorno == "Sala1" && amigo.GetComponent<CharacterHealth>().HealthValue < 60f)
            {
                amigo.GetComponent<CharacterHealth>().Heal(40f);
            }


            //obtención de la vista frontal
            Vector3 puntoOrigen = transform.position + new Vector3(0f, altura, 0f);
            bool choca = Physics.Raycast(puntoOrigen, transform.forward, out hit, 15);

            //para indicarle al animator Controller que se está ejecutando la animación actual y evitar que siga entrando
            anim.SetBool("Actuando", true);

            //si el npc está siguiendo al personaje
            if (siguiendo || (atacando && !enPosicionDeAtaque) && !enMirador)
            {
                //si está lejos, no está quieto y no está ejecutando la animación de correr, la ejecutamos
                if (distanciaAObjetivo > distanciaMedia && !parado && !anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol Run"))
                {
                    cambiaEstado("CorrePistola");
                    nav.speed = velocidadAlta; //velocidad alta
                }

                //si está a media distancia, no está quiero y no ejecuta la animación de caminar, la ejecutamos
                else if (distanciaAObjetivo > distanciaMinima && distanciaAObjetivo <= distanciaMedia && !parado && !anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol Walk"))
                {
                    cambiaEstado("CaminaPistola");
                    nav.speed = velocidadBaja; //velocidad baja
                }

                //Si esta a poca distancia y no ejecuta la animación de Idle, la ejecutamos
                else if (distanciaAObjetivo <= distanciaMinima && !anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol Idle"))
                {
                    cambiaEstado("DescansaPistola");
                    nav.speed = 0; //quieto
                    enPosicionDeAtaque = true;
                }
            }

            else if (atacando)
            {
                bool chocaAliado = false;

                if (hit.transform != null)
                {
                    chocaAliado = partesDelAliado.Contains(hit.transform.gameObject.name);
                }
                bool atacaAliado = objetivo.Equals(amigo);

                if (choca && (!chocaAliado || chocaAliado && atacaAliado) && anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol Idle"))
                {
                    cambiaEstado("DisparaPistola");
                }
                else if ((!choca || (choca && chocaAliado && !atacaAliado)) && anim.GetCurrentAnimatorStateInfo(0).IsName("Pistol Shooting"))
                {
                    cambiaEstado("DescansaPistola");
                }

                if (objetivo.tag == "enemigo1" && objetivo.GetComponent<Script_EnemigoCombate>().salud <= 0)
                {
                    ataqueSala1();
                }
                else if (objetivo.tag == "enemigo3" && objetivo.GetComponent<Script_EnemigoCombate>().salud <= 0)
                {
                    ataqueSala3(enMirador);
                }
                else if (objetivo.tag == "boss" && objetivo.GetComponent<ScriptGato>().salud <= 0)
                {
                    accionDescansa();
                }

            }


            Debug.DrawRay(puntoOrigen, transform.forward * 15f, Color.red);
        }
        
    }

    void cambiaEstado(string nuevoEstado)
    {
        anim.SetBool(parametroActivo, false);
        parametroActivo = nuevoEstado;
        anim.SetBool(parametroActivo, true);
        anim.SetBool("Actuando", false);
    }

    public void instanciarBala()
    {
        Instantiate(prefabBala, firePoint.position, Quaternion.identity, transform);
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
    }

    public void ataqueSala1()
    {

        if (accionActual == "ataca")
        {
            atacando = true;
            siguiendo = false;

            objetivo = null;
            foreach (Transform enemigo in EnemigosSala1)
            {
                if (enemigo.GetComponent<Script_EnemigoCombate>().salud > 0)
                {

                    if (objetivo == null || Vector3.Distance(transform.position, enemigo.position) < nav.remainingDistance)
                    {

                        objetivo = enemigo.gameObject;
                        nav.destination = enemigo.position;
                    }
                }
                distanciaMedia = 8f;
                distanciaMinima = 4f;
            }


            if (objetivo == null)
            {
                salud = 150;
                accionDescansa();
            }
        }

    }

    public void cura(float cantidad)
    {
        if(nav.remainingDistance < distanciaMinima)
        {
            amigo.GetComponent<CharacterHealth>().Heal(cantidad);
        }
    }


    public void valoresSala2()
    {
        distanciaMinima = 2;
        velocidadAlta = 4;
        velocidadBaja = 2;

    }


    public void accionDescansa()
    {
        objetivo = amigo;
        atacando = false;
        siguiendo = true;
        nav.SetDestination(objetivo.transform.position);
        distanciaMedia = 5f;
        distanciaMinima = 3f;
        velocidadAlta = 8f;
        velocidadBaja = 4f;
    }


    public void ataqueSala3(bool enMirador)
    {
        enemigosSala3 = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemigo3"));

        this.enMirador = enMirador;

        atacando = true;
        siguiendo = false;

        objetivo = null;

        foreach (GameObject enemigo in enemigosSala3)
        {
            if (enemigo.GetComponent<Script_EnemigoCombate>().salud > 0)
            {

                if (objetivo == null || Vector3.Distance(transform.position, enemigo.transform.position) < Vector3.Distance(transform.position, objetivo.transform.position))
                {

                    objetivo = enemigo.gameObject;
                    if (enMirador)
                    {
                        nav.SetDestination(objetivo.transform.position);
                        distanciaMinima = nav.remainingDistance;
                    }
                }
            }
            distanciaMedia = 8f;
            distanciaMinima = 4f;
        }


        if (objetivo == null)
        {
            salud = 150;
            accionDescansa();
            GetComponent<ProcesamientoTexto>().terminaReto3();
            reto3Terminado = true;
            this.enMirador = false;
        }


    }


    void veEscondite(int escondite)
    {
        objetivo = posicionesLejanas[escondite];
        distanciaMinima = 1f;
    }

    void atacaGato()
    {
        atacando = true;
        siguiendo = false;
        objetivo = gato;
        distanciaMedia = 8f;
        distanciaMinima = 4f;

        if (accionSalaFinal == "lejos")
        {
            nav.SetDestination(objetivo.transform.position);
            distanciaMinima = Vector3.Distance(transform.position, gato.transform.position);
        }

    }


    public void recibeDaño()
    {
        salud -= 5f;
        if(salud <= 0)
        {
            gameOver.GetComponent<PantallaGameOver>().motivo = "HAN MATADO A TU ALIADO";
            gameOver.SetActive(true);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "Enemigos_sala_1":
                if (!other.GetComponent<ScriptSalasCombate>().retoSuperado) //si entramos y aún no hemos superado el reto
                {
                    for (int i = 0; i < other.transform.childCount; i++)
                    {
                        EnemigosSala1.Add(other.transform.GetChild(i));
                    }
                    ataqueSala1();
                }
                entorno = "Sala1";
                break;
            case "Enemigos_sala_2":

                if (!reto2Terminado)
                {
                    GameObject objetivos = GameObject.Find("Objetivos_Sala_2");

                    for (int i = 0; i < objetivos.transform.childCount; i++)
                    {
                        objetivosSala2.Add(objetivos.transform.GetChild(i));
                    }
                    valoresSala2();
                }


                entorno = "Sala2";
                break;
            case "PosicionEspera1":
                amigo.GetComponent<UltimateCharacterLocomotionHandler>().enabled = true;
                amigo.GetComponent<CharacterVoice>().preparaOrdenes(2);
                enReto = true;
                enEscondite = true;
                break;
            case "PosicionEspera2":
            case "PosicionEspera3":
            case "consolaReto2":
                enEscondite = true;
                amigo.GetComponent<CharacterVoice>().preparaOrdenes(2);
                break;
            case "MiradorDerecho":
            case "MiradorIzquierdo":
                amigo.GetComponent<UltimateCharacterLocomotionHandler>().enabled = true;
                break;
            case "SalaBoss":

                posicionesLejanas = new List<GameObject>();

                for(int i = 0; i < other.transform.childCount - 1; i++) //pasamos por todos los hijos menos 1 que es el gato
                {
                    posicionesLejanas.Add(other.transform.GetChild(i).gameObject);
                }

                gato = other.transform.GetChild(other.transform.childCount - 1).gameObject;

                if(accionSalaFinal == "lejos")
                {
                    veEscondite(new System.Random().Next(4));
                    gato.GetComponent<ScriptGato>().objetivo = gameObject;
                }
                else
                {
                    gato.GetComponent<ScriptGato>().objetivo = amigo;
                    atacaGato();
                }

                break;
            case "PuntoEstrategico1":
            case "PuntoEstrategico2":
            case "PuntoEstrategico3":
            case "PuntoEstrategico4":

                if(accionSalaFinal == "lejos")
                {
                    enPosicionLejana = true;
                    atacaGato();
                }

                break;
            case "Bip01 R Hand":
                if (!GameObject.FindGameObjectWithTag("boss").GetComponent<ScriptGato>().muerto)
                {
                    salud -= 20f;

                    if(salud <= 0)
                    {
                        gato.GetComponent<ScriptGato>().objetivo = amigo;
                        muerto = true;
                        nav.enabled = false;
                        anim.SetTrigger("muere");
                    }
                }
                break;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.name)
        {
            case "Enemigos_sala_1":
            case "Enemigos_sala_2":
                entorno = "general";
                break;

            case "PosicionEspera1":
            case "PosicionEspera2":
            case "PosicionEspera3":
                enEscondite = false;
                break;

        }
    }


}
