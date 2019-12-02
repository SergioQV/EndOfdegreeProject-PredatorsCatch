using Opsive.UltimateCharacterController.Demo.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSala2 : MonoBehaviour
{
    // Start is called before the first frame update

    public bool retoSuperado;
    public GameObject puertaReto;
    public GameObject puertaSala;

    void Start()
    {
        retoSuperado = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void completaObjetivo()
    {
        retoSuperado = true;

        GameObject.Find("Decision_Sala_2").GetComponent<ScriptDecision2>().retoSuperado = true;
        GameObject.Find("DecisionFinal").GetComponent<ScriptSalaFinal>().superaReto(2);

        for (int i = 0; i < transform.childCount-1; i++)
        {
            transform.GetChild(i).GetComponent<Script_Enemigo2>().muere();
        }
        puertaReto.GetComponent<Door>().Open();
        puertaSala.GetComponent<Door>().Locked = false;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Aliado" || other.name == "Nolan")
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).tag == "enemigo2")
                {
                    Script_Enemigo2 iaRobot = transform.GetChild(i).GetComponent<Script_Enemigo2>();
                    if (!iaRobot.muerto)
                    {
                        iaRobot.vuelveAPatrullar();
                    }
                }
            }


        }
    }

}
