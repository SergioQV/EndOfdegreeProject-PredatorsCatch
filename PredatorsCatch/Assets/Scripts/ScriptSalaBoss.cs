using Opsive.UltimateCharacterController.Demo.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSalaBoss : MonoBehaviour
{

    public GameObject puerta;
    public GameObject gato;

    int numIndividuos;

    // Start is called before the first frame update
    void Start()
    {
        numIndividuos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Aliado" || other.name == "Nolan")
        {
            gato.GetComponent<ScriptGato>().alertaGato();
            numIndividuos++;

            if(numIndividuos == 2)
            {
                puerta.GetComponent<Door>().Close();
                puerta.GetComponent<Door>().Locked = true;
            }
        }
    }

}
