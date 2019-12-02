using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScriptSalasCombate : MonoBehaviour
{
    public int numeroEnemigos;
    public bool retoSuperado;
    public List<GameObject> intrusos;
        
    // Start is called before the first frame update
    void Start()
    {
        intrusos = new List<GameObject>();
        numeroEnemigos = transform.childCount;
        retoSuperado = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(numeroEnemigos == 0 && !retoSuperado)
        {
            retoSuperado = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterVoice>().combatiendo = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterVoice>().preparaOrdenes(0);

            int cierraPuerta;
            if(name == "Enemigos_sala_1")
            {
                cierraPuerta = 1;
            }
            else
            {
                cierraPuerta = 3;
            }
            GameObject.Find("DecisionFinal").GetComponent<ScriptSalaFinal>().superaReto(cierraPuerta);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (name == "Enemigos_sala_1")
        {
            gestionaEntrada1(other);
        }
        else if(name == "Enemigos_sala_3" && (other.tag == "aliado" || other.tag == "Player"))
        {
            gestionaEntrada3();
        }

    }

    public void gestionaEntrada1(Collider other)
    {
        if (other.name == "Aliado" || other.name == "Nolan")
        {
            intrusos.Add(other.gameObject);

            if (intrusos.Count >= 2)
            {
                System.Random r = new System.Random(); //especificamos System debido a que unity tiene su propia clase random, la cual no elegimos por trabajar con floats
                for (int i = 0; i < transform.childCount; i++)
                {

                    Script_EnemigoCombate iaRobot = transform.GetChild(i).GetComponent<Script_EnemigoCombate>();

                    if (iaRobot.salud > 0)
                    {
                        iaRobot.objetivo = intrusos[r.Next(intrusos.Count)];
                        iaRobot.atacando = true;
                    }
                }
            }

        }
    }


    public void gestionaEntrada3()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        GameObject aliado = GameObject.FindGameObjectWithTag("aliado");


        System.Random r = new System.Random(); //especificamos System debido a que unity tiene su propia clase random, la cual no elegimos por trabajar con floats

        if ((jugador.GetComponent<CharacterVoice>().enMirador && aliado.GetComponent<ScriptIA>().enMirador)
            || (!jugador.GetComponent<CharacterVoice>().enMirador && !aliado.GetComponent<ScriptIA>().enMirador))
        {
            for (int i = 0; i < transform.childCount; i++)
            {

                Script_EnemigoCombate iaRobot = transform.GetChild(i).GetComponent<Script_EnemigoCombate>();

                if (iaRobot.salud > 0)
                {
                    if(r.Next(2) == 0)
                    {
                        iaRobot.objetivo = aliado;
                        iaRobot.objetivoEnMirador = aliado.GetComponent<ScriptIA>().enMirador;
                    }
                    else
                    {
                        iaRobot.objetivo = jugador;
                        iaRobot.objetivoEnMirador = jugador.GetComponent<CharacterVoice>().enMirador;
                    }
                    iaRobot.atacando = true;
                }
            }
        }
        else
        {
            GameObject enMirador;
            GameObject enPelea;

            if(aliado.GetComponent<ScriptIA>().enMirador){
                enMirador = aliado;
                enPelea = jugador;
            }
            else
            {
                enMirador = jugador;
                enPelea = aliado;
            }

            for (int i = 0; i < transform.childCount; i++)
            {

                Script_EnemigoCombate iaRobot = transform.GetChild(i).GetComponent<Script_EnemigoCombate>();

                if (iaRobot.salud > 0)
                {
                    if (r.Next(4) == 0)
                    {
                        iaRobot.objetivo = enMirador;
                        iaRobot.objetivoEnMirador = true;
                    }
                    else
                    {
                        iaRobot.objetivo = enPelea;
                        iaRobot.objetivoEnMirador = false;
                    }
                    iaRobot.atacando = true;
                }
            }


        }

    }



    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Aliado" || other.name == "Nolan")
        {
            intrusos.Remove(other.gameObject);


            if(intrusos.Count > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {

                    Script_EnemigoCombate iaRobot = transform.GetChild(i).GetComponent<Script_EnemigoCombate>();

                    if (iaRobot.salud > 0)
                    {
                        iaRobot.objetivo = intrusos[0];
                        iaRobot.atacando = true;
                    }
                }
            }

            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Script_EnemigoCombate iaRobot = transform.GetChild(i).GetComponent<Script_EnemigoCombate>();

                    iaRobot.objetivo = null;
                    iaRobot.atacando = false;

                    if(iaRobot.salud > 0) //Solo se recuperan los que estén vivos, los muertos siguen muertos
                    {
                        iaRobot.salud = 50;
                    }
                    
                }
            }
        }
    }

}
