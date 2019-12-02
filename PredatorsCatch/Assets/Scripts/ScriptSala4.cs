using System.Collections;
using System.Collections.Generic;
using Opsive.UltimateCharacterController.Demo.Objects;
using UnityEngine;

public class ScriptSala4 : MonoBehaviour
{

    public GameObject panelPrincipal;
    public GameObject puertaSala;
    public GameObject[] puertasIzquierda;
    public GameObject[] puertasDerecha;
    public GameObject[] panelesIzquierda;
    public GameObject[] panelesDerecha;
    public GameObject tutorial4;


    bool puertasAbiertas;
    bool tutorial4Mostrado;
    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < panelesIzquierda.Length; i++)
        {
            panelesDerecha[i].GetComponent<controlPaneles>().indice = i;
            panelesIzquierda[i].GetComponent<controlPaneles>().indice = i + 3;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(panelPrincipal.GetComponent<controlPaneles>().activado && !puertasAbiertas)
        {
            foreach(GameObject puerta in puertasDerecha)
            {
                puerta.GetComponent<Door>().Open();
            }
            foreach (GameObject puerta in puertasIzquierda)
            {
                puerta.GetComponent<Door>().Open();
            }
            puertasAbiertas = true;
        }
    }

    public void empiezaReto()
    {
        puertasDerecha[0].GetComponent<Door>().Open();
        puertasIzquierda[0].GetComponent<Door>().Open();
        puertaSala.GetComponent<Door>().Locked = true;
    }

    public void activaPanel(int i)
    {
        if (!puertasAbiertas)
        {
            switch (i)
            {
                case 0:

                    foreach(GameObject panel in panelesIzquierda)
                    {
                        panel.GetComponent<controlPaneles>().activado = false;
                        panel.GetComponent<controlPaneles>().valido = true;
                    }
                    foreach (GameObject panel in panelesDerecha)
                    {
                        if(panel.GetComponent<controlPaneles>().indice != 0)
                        {
                            panel.GetComponent<controlPaneles>().activado = false;
                            panel.GetComponent<controlPaneles>().valido = true;
                        }
                        else
                        {
                            panel.GetComponent<controlPaneles>().valido = false;
                        }
                    }
                    puertasDerecha[0].GetComponent<Door>().Close();
                    puertasIzquierda[0].GetComponent<Door>().Close();
                    puertasIzquierda[1].GetComponent<Door>().Open();
                    puertasIzquierda[3].GetComponent<Door>().Open();

                    break;
                case 1:
                    foreach (GameObject panel in panelesIzquierda)
                    {
                        panel.GetComponent<controlPaneles>().activado = false;
                        panel.GetComponent<controlPaneles>().valido = true;
                    }
                    foreach (GameObject panel in panelesDerecha)
                    {
                        if (panel.GetComponent<controlPaneles>().indice != 1)
                        {
                            panel.GetComponent<controlPaneles>().activado = false;
                            panel.GetComponent<controlPaneles>().valido = true;
                        }
                        else
                        {
                            panel.GetComponent<controlPaneles>().valido = false;
                        }
                    }
                    puertasDerecha[1].GetComponent<Door>().Close();
                    puertasDerecha[2].GetComponent<Door>().Close();
                    puertasDerecha[3].GetComponent<Door>().Close();
                    puertasIzquierda[1].GetComponent<Door>().Open();
                    puertasIzquierda[2].GetComponent<Door>().Open();

                    break;
                case 2:
                    foreach (GameObject panel in panelesIzquierda)
                    {
                        panel.GetComponent<controlPaneles>().activado = false;
                        panel.GetComponent<controlPaneles>().valido = true;
                    }
                    foreach (GameObject panel in panelesDerecha)
                    {
                        if (panel.GetComponent<controlPaneles>().indice != 2)
                        {
                            panel.GetComponent<controlPaneles>().activado = false;
                            panel.GetComponent<controlPaneles>().valido = true;
                        }
                        else
                        {
                            panel.GetComponent<controlPaneles>().valido = false;
                        }
                    }

                    puertasDerecha[1].GetComponent<Door>().Close();
                    puertasDerecha[2].GetComponent<Door>().Close();
                    puertasIzquierda[1].GetComponent<Door>().Open();
                    puertasIzquierda[2].GetComponent<Door>().Open();

                    break;
                case 3:
                    foreach (GameObject panel in panelesDerecha)
                    {
                        panel.GetComponent<controlPaneles>().activado = false;
                        panel.GetComponent<controlPaneles>().valido = true;
                    }
                    foreach (GameObject panel in panelesIzquierda)
                    {
                        if (panel.GetComponent<controlPaneles>().indice != 3)
                        {
                            panel.GetComponent<controlPaneles>().activado = false;
                            panel.GetComponent<controlPaneles>().valido = true;
                        }
                        else
                        {
                            panel.GetComponent<controlPaneles>().valido = false;
                        }
                    }
                    puertasDerecha[0].GetComponent<Door>().Close();
                    puertasIzquierda[0].GetComponent<Door>().Close();
                    puertasDerecha[1].GetComponent<Door>().Open();
                    puertasDerecha[3].GetComponent<Door>().Open();


                    break;

                case 4:
                    foreach (GameObject panel in panelesDerecha)
                    {
                        panel.GetComponent<controlPaneles>().activado = false;
                        panel.GetComponent<controlPaneles>().valido = true;
                    }
                    foreach (GameObject panel in panelesIzquierda)
                    {
                        if (panel.GetComponent<controlPaneles>().indice != 4)
                        {
                            panel.GetComponent<controlPaneles>().activado = false;
                            panel.GetComponent<controlPaneles>().valido = true;
                        }
                        else
                        {
                            panel.GetComponent<controlPaneles>().valido = false;
                        }
                    }
                    puertasIzquierda[1].GetComponent<Door>().Close();
                    puertasIzquierda[2].GetComponent<Door>().Close();
                    puertasIzquierda[3].GetComponent<Door>().Close();
                    puertasDerecha[1].GetComponent<Door>().Open();
                    puertasDerecha[2].GetComponent<Door>().Open();
                    break;

                case 5:
                    foreach (GameObject panel in panelesDerecha)
                    {
                        panel.GetComponent<controlPaneles>().activado = false;
                        panel.GetComponent<controlPaneles>().valido = true;
                    }
                    foreach (GameObject panel in panelesIzquierda)
                    {
                        if (panel.GetComponent<controlPaneles>().indice != 5)
                        {
                            panel.GetComponent<controlPaneles>().activado = false;
                            panel.GetComponent<controlPaneles>().valido = true;
                        }
                        else
                        {
                            panel.GetComponent<controlPaneles>().valido = false;
                        }
                    }
                    puertasDerecha[1].GetComponent<Door>().Open();
                    puertasDerecha[2].GetComponent<Door>().Open();
                    puertasIzquierda[1].GetComponent<Door>().Close();
                    puertasIzquierda[2].GetComponent<Door>().Close();

                    break;

                case 6:

                    foreach (GameObject panel in panelesDerecha)
                    {
                        panel.GetComponent<controlPaneles>().terminado = true;
                    }
                    foreach (GameObject panel in panelesIzquierda)
                    {
                        panel.GetComponent<controlPaneles>().terminado = true;
                    }
                    panelPrincipal.GetComponent<controlPaneles>().terminado = true;

                    puertasAbiertas = true;
                    puertasDerecha[0].GetComponent<Door>().Open();
                    puertasDerecha[1].GetComponent<Door>().Open();
                    puertasDerecha[2].GetComponent<Door>().Open();
                    puertasDerecha[3].GetComponent<Door>().Open();
                    puertasIzquierda[0].GetComponent<Door>().Open();
                    puertasIzquierda[1].GetComponent<Door>().Open();
                    puertasIzquierda[2].GetComponent<Door>().Open();
                    puertasIzquierda[3].GetComponent<Door>().Open();
                    puertaSala.GetComponent<Door>().Locked = false;

                    GameObject.Find("DecisionFinal").GetComponent<ScriptSalaFinal>().superaReto(4);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterVoice>().terminaReto4();
                    GameObject.FindGameObjectWithTag("aliado").GetComponent<ProcesamientoTexto>().terminarReto4();
                    break;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Nolan" && !tutorial4Mostrado)
        {
            tutorial4Mostrado = true;
            tutorial4.SetActive(true);
        }
    }


}
