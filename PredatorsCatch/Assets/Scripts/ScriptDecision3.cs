using Opsive.UltimateCharacterController.Demo.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDecision3 : MonoBehaviour
{

    public GameObject puertaSala;
    public GameObject tutorial3;

    bool tutorial3Mostrado;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void accionOrden(string detalle = "porDefecto")
    {
        if(detalle == "espera")
        {
            GetComponentsInChildren<Door>()[0].Locked = true;
        }
        else
        {
            GetComponentsInChildren<Door>()[0].Locked = false;
        }
        puertaSala.GetComponent<Door>().Locked = true;

        if(detalle != "seguir" && detalle != "entra")
        {
            GetComponentsInChildren<Door>()[1].Open();
            GetComponentsInChildren<Door>()[2].Open();
        }
        else
        {
            GetComponentsInChildren<Door>()[1].Close();
            GetComponentsInChildren<Door>()[2].Close();
        }
    }

    public void terminaReto()
    {
        GameObject.Find("MiradorDerecho").GetComponent<ScriptMiradores>().retoTerminado = true;
        GameObject.Find("MiradorIzquierdo").GetComponent<ScriptMiradores>().retoTerminado = true;
        puertaSala.GetComponent<Door>().Locked = false;
        GetComponentsInChildren<Door>()[0].Locked = false;
        GetComponentsInChildren<Door>()[1].Open();
        GetComponentsInChildren<Door>()[2].Open();

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Nolan" && !tutorial3Mostrado)
        {
            tutorial3Mostrado = true;
            tutorial3.SetActive(true);
        }
    }



}
