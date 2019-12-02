using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Demo.Objects;
using Opsive.UltimateCharacterController.Traits;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcesamientoTexto : MonoBehaviour
{
    public GameObject tutorialMision;
    public GameObject tutorialControles;
    public GameObject mensajeError;
    public GameObject estadoAliado;

    //datos necesarios para el reto 2
    string posicionEnReto2;
    bool reto2Terminado;

    //datos para el reto 3
    public GameObject decision3;
    public bool enMirador;
    bool reto3Terminado;
    string orden;

    //datos para el reto 4
    public ScriptSala4 controlSala4;
    public bool enColliderPanel;
    public bool reto4Empezado;
    public bool reto4Terminado;
    public GameObject panelCercano;
    public int panel;
    string lado;

    //Para atender ordenes especificas de cada entorno
    public string entorno = "EntornoGeneral";
    public GameObject salaActual;

    string frase;
    string[] frasePartida;
    ScriptIA iaAliado;
    CharacterVoice vozPersonaje;

    private void Start()
    {
        iaAliado = gameObject.GetComponent<ScriptIA>();
        vozPersonaje = iaAliado.amigo.GetComponent<CharacterVoice>();
        orden = "";
    }


    private void Update()
    {

    }

    public void procesaResultado(string orden)
    {
        if (orden != null)
        {
            frase = orden;
            Debug.Log(orden);
            frasePartida = orden.Split(' '); //nos quedamos con el primer resultado porque,
                                                          //al haber puesto una fiabilidad del 70%, el reconocedor solo devuelve 1 resultado

            switch (entorno)
            {
                case "EntornoGeneral":
                    ordenesGenerales();
                    break;
                case "Sala1":
                    accionAtaque();
                    break;
                case "Sala2":
                    ordenesSala2();
                    break;
                case "Decision3":
                    ordenesSala3();
                    break;
                case "Sala4":
                    ordenesSala4();
                    break;
                case "Final":
                    ordenesFinales();
                    break;
            }

        }
        else
        {
            mensajeError.SetActive(true);
        }
    }


    void ordenesGenerales()
    {
        if(frase == "cúrame")
        {

            iaAliado.cura(100f - iaAliado.amigo.GetComponent<CharacterHealth>().HealthValue);
            vozPersonaje.preparaOrdenes(0);
        }
        else if(frase == "explícame la misión")
        {
            tutorialMision.SetActive(true);
        }
        else if(frase == "resumen de los controles")
        {
            tutorialControles.SetActive(true);
        }
        else
        {
            mensajeError.SetActive(true);
        }
    }

    void accionAtaque()
    {
        if (entorno == "Sala1" && frase == "ataca")
        {
            iaAliado.accionActual = "ataca";
        }
        else if (entorno == "Sala1" && frase == "sígueme y cúrame")
        {
            iaAliado.accionActual = "cura";
        }
        else
        {
            mensajeError.SetActive(true);
        }

    }

    private void ordenesSala2()
    {
        if (frasePartida[0] == "ve" && posicionEnReto2 != "objetivo")
        {
            if (frasePartida[2] == "escondite" && frasePartida.Length == 6)
            {
                switch (frasePartida[5])
                {
                    case "izquierda":
                        if (posicionEnReto2 != "espera2")
                        {
                            iaAliado.objetivo = iaAliado.objetivosSala2[1].gameObject;
                        }
                        break;
                    case "derecha":
                        if (posicionEnReto2 != "espera3")
                        {
                            iaAliado.objetivo = iaAliado.objetivosSala2[2].gameObject;
                        }
                        break;
                    default:
                        mensajeError.SetActive(true);
                        break;
                }
            }
            else if (frasePartida[2] == "principio" && posicionEnReto2 != "espera1")
            {
                iaAliado.objetivo = iaAliado.objetivosSala2[0].gameObject;
            }
            else if (frasePartida[2] == "objetivo")
            {
                iaAliado.objetivo = iaAliado.objetivosSala2[3].gameObject;
            }
            else
            {
                mensajeError.SetActive(true);
            }
        }
        else if (frase == "activa el panel")
        {
            salaActual.GetComponent<ScriptSala2>().completaObjetivo();
            estadoAliado.SetActive(false);
            vozPersonaje.reto2Terminado = true;
            vozPersonaje.preparaOrdenes(0);
            iaAliado.accionDescansa();
            iaAliado.reto2Terminado = true;
            reto2Terminado = true;
            entorno = "EntornoGeneral";
        }
        else
        {
            mensajeError.SetActive(true);
        }


    }

    void ordenesSala3()
    {
        if (frasePartida[0] == "ve" && !enMirador && (frasePartida[3] == "izquierda" || frasePartida[3] == "derecha" || frasePartida[2] == "otro"))
        {
            vozPersonaje.reto3Empezado = true;
            vozPersonaje.ordenReversible = false;
            iaAliado.amigo.GetComponent<UltimateCharacterLocomotionHandler>().enabled = false;
            if (frasePartida[3] == "izquierda" && !vozPersonaje.enMirador)
            {
                iaAliado.objetivo = GameObject.Find("MiradorIzquierdo");
                decision3.GetComponent<ScriptDecision3>().accionOrden();
            }
            else if (frasePartida[3] == "derecha" && !vozPersonaje.enMirador)
            {
                iaAliado.objetivo = GameObject.Find("MiradorDerecho");
                decision3.GetComponent<ScriptDecision3>().accionOrden();
            }
            else if (frasePartida[2] == "otro" && vozPersonaje.enMirador)
            {
                iaAliado.objetivo = GameObject.Find(vozPersonaje.mirador == "MiradorDerecho" ? "MiradorIzquierdo" : "MiradorDerecho");
            }
            orden = "ve";
            estadoAliado.GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "Moviendose";
            estadoAliado.SetActive(true);


        }
        else if (frasePartida[0] == "espera" && !enMirador)
        {
            vozPersonaje.reto3Empezado = true;
            vozPersonaje.ordenReversible = true;
            iaAliado.objetivo = decision3;
            decision3.GetComponent<ScriptDecision3>().accionOrden("espera");
            orden = "espera";

            estadoAliado.GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "Moviendose";
            estadoAliado.SetActive(true);

        }
        else if (frasePartida[0] == "sígueme" && !enMirador && !vozPersonaje.enMirador)
        {
            vozPersonaje.reto3Empezado = true;
            vozPersonaje.ordenReversible = true;
            iaAliado.accionDescansa();
            decision3.GetComponent<ScriptDecision3>().accionOrden("seguir");
            orden = "seguir";

            estadoAliado.GetComponent<Image>().color = Color.green;
            estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
            estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "Siguiendo";
            estadoAliado.SetActive(true);

        }
        else if (frasePartida[0] == "entra" && !enMirador && vozPersonaje.enMirador)
        {
            if (!iaAliado.reto3Terminado)
            {

                decision3.GetComponent<ScriptDecision3>().accionOrden("entra");
                iaAliado.ataqueSala3(false);
                vozPersonaje.combatiendo = true;
                estadoAliado.SetActive(false);
            }
        }
        else if (frasePartida[0] == "dispara" && enMirador && vozPersonaje.enMirador)
        {

            if (!iaAliado.reto3Terminado)
            {
                iaAliado.ataqueSala3(true);
                vozPersonaje.combatiendo = true;
                estadoAliado.SetActive(false);
            }
        }
        else
        {
            mensajeError.SetActive(true);
        }


    }


    public void terminaReto3()
    {
        decision3.GetComponent<ScriptDecision3>().terminaReto();
        entorno = "EntornoGeneral";
        reto3Terminado = true;
        vozPersonaje.combatiendo = false;
        vozPersonaje.reto3Terminado = true;
        vozPersonaje.preparaOrdenes(0);
    }


    public void ordenesSala4()
    {

        if (frasePartida[0] == "ve" && frasePartida[3] == "izquierda" && !reto4Empezado)
        {

            lado = "izquierda";
            vozPersonaje.empiezaReto4(controlSala4, lado);
            panel = 0;
            controlSala4.empiezaReto();
            iaAliado.objetivo = controlSala4.panelesIzquierda[panel];
            iaAliado.distanciaMinima = 1f;
            iaAliado.velocidadBaja = 3;
            iaAliado.velocidadAlta = 4;
            iaAliado.amigo.GetComponent<UltimateCharacterLocomotionHandler>().enabled = false;


            estadoAliado.GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "Moviendose";
            estadoAliado.SetActive(true);

        }
        else if (frasePartida[0] == "ve" && frasePartida[3] == "derecha" && !reto4Empezado)
        {
            lado = "derecha";
            vozPersonaje.empiezaReto4(controlSala4, lado);
            panel = 0;
            controlSala4.empiezaReto();
            iaAliado.objetivo = controlSala4.panelesDerecha[panel];
            iaAliado.distanciaMinima = 1f;
            iaAliado.velocidadBaja = 3;
            iaAliado.velocidadAlta = 4;
            iaAliado.amigo.GetComponent<UltimateCharacterLocomotionHandler>().enabled = false;

            estadoAliado.GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.red;
            estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "Moviendose";
            estadoAliado.SetActive(true);
        }
        else if (frasePartida[0] == "ve" && frasePartida[3] == "anterior" && reto4Empezado && panel != 0 && panelCercano.GetComponent<controlPaneles>().indice != 6)
        {
            if (lado == "izquierda" && controlSala4.puertasIzquierda[panel].GetComponent<Door>().isOpened())
            {
                iaAliado.objetivo = controlSala4.panelesIzquierda[panel - 1];
            }

            else if (lado == "derecha" && controlSala4.puertasDerecha[panel].GetComponent<Door>().isOpened())
            {
                iaAliado.objetivo = controlSala4.panelesDerecha[panel - 1];
            }
        }
        else if (frasePartida[0] == "ve" && frasePartida[3] == "siguiente" && panelCercano.GetComponent<controlPaneles>().indice != 6)
        {
            if (panel < 2)
            {
                if (lado == "izquierda" && controlSala4.puertasIzquierda[panel + 1].GetComponent<Door>().isOpened())
                {

                    iaAliado.objetivo = controlSala4.panelesIzquierda[panel + 1];

                }

                else if (lado == "derecha" && controlSala4.puertasDerecha[panel + 1].GetComponent<Door>().isOpened())
                {
                    iaAliado.objetivo = controlSala4.panelesDerecha[panel + 1];
                }
            }
            else if ((lado == "izquierda" && controlSala4.puertasIzquierda[panel + 1].GetComponent<Door>().isOpened()) ||
                (lado == "derecha" && controlSala4.puertasDerecha[panel + 1].GetComponent<Door>().isOpened()))
            {
                iaAliado.objetivo = controlSala4.panelPrincipal;
            }
        }
        else if (frasePartida[0] == "activa" && !panelCercano.GetComponent<controlPaneles>().activado)
        {
            panelCercano.GetComponent<controlPaneles>().activado = true;
            panelCercano.transform.parent.GetComponent<ScriptSala4>().activaPanel(panelCercano.GetComponent<controlPaneles>().indice);
            if (!reto4Terminado)
            {
                vozPersonaje.preparaOrdenes(4);
            }
        }
        else
        {
            mensajeError.SetActive(true);
        }


    }

    public void terminarReto4()
    {
        reto4Terminado = true;
        entorno = "EntornoGeneral";
        iaAliado.accionDescansa();
    }

    public void ordenesFinales()
    {

        if (frasePartida.Length == 3 && frasePartida[0] == "ataca" && (frasePartida[2] == "lejos" || frasePartida[2] == "libertad"))
        {
            iaAliado.accionSalaFinal = frasePartida[2];
        }
        else
        {
            mensajeError.SetActive(true);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "Enemigos_sala_2":
                if (!reto2Terminado)
                {
                    entorno = "Sala2";
                }
                salaActual = other.gameObject;
                break;
            case "PosicionEspera1":
                estadoAliado.GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En principio";
                posicionEnReto2 = "espera1";
                break;
            case "PosicionEspera2":
                estadoAliado.GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En escondite izquierdo";
                posicionEnReto2 = "espera2";
                break;
            case "PosicionEspera3":
                estadoAliado.GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En escondite derecho";
                posicionEnReto2 = "espera3";
                break;
            case "consolaReto2":
                estadoAliado.GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En objetivo";
                posicionEnReto2 = "objetivo";
                break;
            case "Decision_Sala_3":
                if (!reto3Terminado && orden == "espera")
                {
                    estadoAliado.GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En Espera";

                }
                break;
            case "MiradorIzquierdo":
            case "MiradorDerecho":
                if (!reto3Terminado)
                {
                    enMirador = true;
                    vozPersonaje.preparaOrdenes(3);
                    estadoAliado.GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En Mirador";
                }
                break;
            case "Enemigos_sala_3":
                estadoAliado.SetActive(false);
                break;
            case "consolaDerecha1":
                if (!reto4Empezado)
                {
                    iaAliado.amigo.GetComponent<UltimateCharacterLocomotionHandler>().enabled = true;
                    controlSala4.puertasDerecha[0].GetComponent<Door>().Close();
                    reto4Empezado = true;
                }
                if (!reto4Terminado)
                {
                    vozPersonaje.preparaOrdenes(4);
                    estadoAliado.GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En panel " + (panel + 1);
                }
                break;
            case "consolaIzquierda1":
                if (!reto4Empezado)
                {
                    iaAliado.amigo.GetComponent<UltimateCharacterLocomotionHandler>().enabled = true;
                    controlSala4.puertasIzquierda[0].GetComponent<Door>().Close();
                    reto4Empezado = true;
                }
                if (!reto4Terminado)
                {
                    vozPersonaje.preparaOrdenes(4);
                    estadoAliado.GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En panel " + (panel + 1);
                }
                break;
            case "consolaDerecha2":
            case "consolaDerecha3":
            case "consolaIzquierda2":
            case "consolaIzquierda3":
            case "consolaReto":
                if (!reto4Terminado)
                {
                    vozPersonaje.preparaOrdenes(4);
                    estadoAliado.GetComponent<Image>().color = Color.green;
                    estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.green;

                    if (other.name != "consolaReto")
                    {
                        estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En panel " + (panel + 1);
                    }
                    else
                    {
                        estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "En objetivo";
                    }
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.name)
        {
            case "Decision_Sala_3":
            case "PosicionEspera1":
            case "PosicionEspera2":
            case "PosicionEspera3":
            case "consolaDerecha1":
            case "consolaDerecha2":
            case "consolaDerecha3":
            case "consolaIzquierda1":
            case "consolaIzquierda2":
            case "consolaIzquierda3":
            case "consolaReto":

                if (!(other.name == "Decision_Sala_3" && orden == "seguir"))
                {
                    estadoAliado.GetComponent<Image>().color = Color.red;
                    estadoAliado.transform.GetChild(0).GetComponent<Image>().color = Color.red;
                    estadoAliado.transform.GetChild(1).GetComponent<Text>().text = "Moviendose";
                }
                break;
        }
    }



}

