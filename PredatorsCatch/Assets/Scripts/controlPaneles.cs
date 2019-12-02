using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlPaneles : MonoBehaviour
{
    // Start is called before the first frame update
    public bool activado = false;
    public bool valido = true;
    public bool terminado = false;
    public int indice;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Aliado")
        {
            other.gameObject.GetComponent<ProcesamientoTexto>().panelCercano = gameObject;
            other.gameObject.GetComponent<ProcesamientoTexto>().enColliderPanel = true;
            other.gameObject.GetComponent<ProcesamientoTexto>().panel = indice % 3;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Aliado")
        {
            other.gameObject.GetComponent<ProcesamientoTexto>().panelCercano = null;
            other.gameObject.GetComponent<ProcesamientoTexto>().enColliderPanel = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Nolan" && Input.GetButtonDown("Action"))
        {
            activado = !activado;

            if (activado)
            {
                transform.parent.GetComponent<ScriptSala4>().activaPanel(indice);
            }

            if (!terminado)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterVoice>().preparaOrdenes(4);
            }
        }
    }

}
