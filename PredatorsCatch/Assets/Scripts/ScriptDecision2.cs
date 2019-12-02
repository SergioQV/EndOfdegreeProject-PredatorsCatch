using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Opsive.UltimateCharacterController.Demo.Objects;
using Opsive.UltimateCharacterController.Character;

public class ScriptDecision2 : MonoBehaviour
{

    public GameObject tutorial2;
    public GameObject puertaReto;
    public GameObject puertaSala;
    public GameObject textoMensaje;
    public GameObject primerObjetivoSala2;
    public bool retoSuperado;

    bool personajeEnArea;
    bool retoEmpezado;
    bool tutorial2Mostrado;

    // Start is called before the first frame update
    void Start()
    {
        personajeEnArea = false;
        retoEmpezado = false;
        retoSuperado = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Action") && personajeEnArea)
        {
            textoMensaje.transform.parent.gameObject.SetActive(false);
            retoEmpezado = true;
            GameObject.FindGameObjectWithTag("aliado").GetComponent<ScriptIA>().objetivo = primerObjetivoSala2;
            puertaReto.GetComponent<Door>().Open();
            puertaSala.GetComponent<Door>().Locked = true;
        }

        if (GameObject.FindGameObjectWithTag("aliado").GetComponent<ScriptIA>().enReto && !retoSuperado)
        {
            puertaReto.GetComponent<Door>().Close();
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Nolan" && !retoEmpezado)
        {
            textoMensaje.transform.parent.gameObject.SetActive(true);
            textoMensaje.GetComponent<Text>().text = "Pulsa F para comenzar el reto de la sala 2";
            personajeEnArea = true;

            if (!tutorial2Mostrado)
            {
                tutorial2.SetActive(true);
                tutorial2Mostrado = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Nolan")
        {
            textoMensaje.transform.parent.gameObject.SetActive(false);
            personajeEnArea = false;
        }
    }

}
