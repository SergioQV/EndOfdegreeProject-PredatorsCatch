using Opsive.UltimateCharacterController.Demo.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSalaFinal : MonoBehaviour
{

    public GameObject puerta;
    public GameObject[] puertasReto;
    public GameObject tutorial1;
    public GameObject tutorialFinal;
    public GameObject misionPersonaje;

    public bool final;

    bool tut1Mostrado;
    bool tutFinalMostrado;
    int retosSuperados;
    int cierraPuerta;
    int personajesEnSala;

    // Start is called before the first frame update
    void Start()
    {
        retosSuperados = 0;
        cierraPuerta = 0;
        personajesEnSala = 0;
        misionPersonaje.transform.GetChild(1).GetComponent<Text>().text = "Avanza por el pasillo";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void superaReto(int reto)
    {
        retosSuperados++;
        cierraPuerta = reto;
        misionPersonaje.transform.GetChild(1).GetComponent<Text>().text = "Retos superados: " + retosSuperados + "/4";

        if (retosSuperados == 4)
        {
            final = true;
            puerta.GetComponent<Door>().Locked = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Nolan" || other.name == "Aliado")
        {
            personajesEnSala++;

            if(personajesEnSala == 2 && cierraPuerta > 0)
            {
                puertasReto[cierraPuerta - 1].GetComponent<Door>().Close();
                puertasReto[cierraPuerta - 1].GetComponent<Door>().Locked = true;
            }

            if (!tut1Mostrado)
            {
                tutorial1.SetActive(true);
                tut1Mostrado = true;
                misionPersonaje.transform.GetChild(1).GetComponent<Text>().text = "Retos superados: " + retosSuperados +"/4";
            }

            if(final && !tutFinalMostrado)
            {
                misionPersonaje.transform.GetChild(1).GetComponent<Text>().text = "Acaba con el depredador";
                tutFinalMostrado = true;
                tutorialFinal.SetActive(true);
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Nolan" || other.name == "Aliado")
        {
            personajesEnSala--;
        }
    }


}
