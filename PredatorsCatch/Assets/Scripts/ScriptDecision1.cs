using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDecision1 : MonoBehaviour
{

    public List<GameObject> personajes;
    public List<string> ordenesOriginales;

    public GameObject tutorial1;

    public bool tutorial1Mostrado;

   
    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "Aliado" || other.name == "Nolan")
        {
            personajes.Add(other.gameObject);

            if (personajes.Count >= 2 && !GameObject.Find("Enemigos_sala_1").GetComponent<ScriptSalasCombate>().retoSuperado) //Si se ha superado el reto, no cambian los valores de ninguno
            {
                foreach(GameObject personaje in personajes)
                {
                    if(personaje.name == "Nolan")
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterVoice>().preparaOrdenes(1);
                    }

                    else if(personaje.name == "Aliado")
                    {
                        personaje.GetComponent<ProcesamientoTexto>().entorno = "Sala1";
                    }

                }
            }

            if (!tutorial1Mostrado)
            {
                tutorial1Mostrado = true;
                tutorial1.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Aliado" || other.name == "Nolan")
        {
            if (personajes.Count >= 2)
            {
                foreach (GameObject personaje in personajes)
                {
                    if (personaje.name == "Nolan")
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterVoice>().preparaOrdenes(0);
                    }

                    else if(personaje.name == "Aliado")
                    {
                        personaje.GetComponent<ProcesamientoTexto>().entorno = "EntornoGeneral";
                    }
                }
            }
            personajes.Remove(other.gameObject);
        }
    }

}
