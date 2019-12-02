using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Demo.Objects;
using Opsive.UltimateCharacterController.Traits;
using UnityEngine.Windows.Speech;
using System.Collections;

public class CharacterVoice : MonoBehaviour
{

    public GameObject aliado;
    public GameObject canvasListaOpciones;
    public GameObject textoMensaje;
    public GameObject estadoAliado;
    public GameObject alertaHabla;
    public List<string> listaOrdenes;
    public bool apareceMensaje;

    /***Variables de reto 1****/
    public bool combatiendo;

    /***Variables de reto 2****/
    public bool reto2Terminado;
    bool enDecision2;
    bool reto2Empezado;
    bool enPosicionGuia;


    /***Variables del reto 3****/
    public bool enDecision3;
    public bool enMirador;
    public bool reto3Empezado;
    public bool reto3Terminado;
    public bool ordenReversible;
    public string mirador;

    /***Variables del reto 4****/
    public bool reto4Terminado;
    public bool reto4Empezado;
    bool enPanel;
    GameObject panelCercano;
    ScriptSala4 controlSala4;
    string ladoAliado;

    bool enCombateFinal; // para cuando estemos en el combate final


    //PARA EL RECONOCIMIENTO DE VOZ
    DictationRecognizer m_DictationRecognizer;
    bool grabando;
    bool completado;
    bool tareaEmpezada = false;
    bool permisoParaHablar = false;
    string fraseReconocida;

    // Start is called before the first frame update
    void Start()
    {
        //reiniciarTarea(); //creamos la tarea nada más empezar a funcionar el objeto
        preparaOrdenes(0);
        canvasListaOpciones.SetActive(false);
        alertaHabla.SetActive(false);
        apareceMensaje = false;
        combatiendo = false;
        reto2Empezado = false;
        mirador = null;


        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        m_DictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
        m_DictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        m_DictationRecognizer.DictationError += DictationRecognizer_DictationError;

    }

    // Update is called once per frame
    void Update()
    {

        ScriptIA iaAmigo = aliado.GetComponent<ScriptIA>();

        if (!tareaEmpezada)
        {
            canvasListaOpciones.SetActive(false);
            alertaHabla.SetActive(false);
        }


        if(tareaEmpezada && grabando && !permisoParaHablar) //Si la tarea ha empezado, el reconocedor ha empezado a grabar y aún no se ha advertido de que se puede
        {
            Text alerta = alertaHabla.transform.GetChild(0).GetComponent<Text>();
            alerta.text = "Habla";
            alertaHabla.GetComponent<Image>().color = Color.green;
            permisoParaHablar = true;
        }

        if (Input.GetButtonDown("Voice") //si se ha pulsado el botón 
            &&  !tareaEmpezada //y la tarea no ha empezado
            && !combatiendo //y no está combatiendo en el primer reto
            && !enDecision2 //no esté en el trigger de decisión 2
            && (!(reto2Empezado && !enPosicionGuia) || reto2Terminado)//y si no sucede que haya empezado el reto y no esté en la posición de guía o si el reto a terminado
            && !(!reto2Terminado && iaAmigo.enReto && !iaAmigo.enEscondite)  //y si no sucede que, no habiendo terminado el reto, el aliado esté en el reto y no esté en el escondite
            && (!(reto3Empezado && !enMirador && (!enDecision3 || (enDecision3 && !ordenReversible))) // y si no ocurre que haya empezado el reto 3, no este en mirador y bien no esté en la decision o esté y la orden no se pueda revertir
                || reto3Terminado) //o que el tercer reto haya terminado
            && (!(reto4Empezado && !enPanel) || reto4Terminado) //y si no ocurre que haya empezado el reto 4 y no esté en un panel o si ha terminado el reto 4
            && !enCombateFinal)
        {
            GetComponent<UltimateCharacterLocomotionHandler>().enabled = false;

            canvasListaOpciones.SetActive(true);
            Text textoOpciones = canvasListaOpciones.transform.GetChild(0).GetComponent<Text>();
            textoOpciones.text = "";

            foreach(string opcion in listaOrdenes)
            {
                textoOpciones.text += opcion + "\n";
            }

            alertaHabla.SetActive(true);
            Text alerta = alertaHabla.transform.GetChild(0).GetComponent<Text>();

            alerta.text = "No hables aun";
            alertaHabla.GetComponent<Image>().color = Color.red;

            tareaEmpezada = true;
            Debug.Log("Boton pulsado");
            //tareaAsincrona.Start();
            StartCoroutine("reconocimiento");
        }

    }


    ////RECONOCEDOR DE VOZ DE UNITY EN WINDOWS 10 (CAMBIO DE ULTIMA HORA)
    private IEnumerator reconocimiento()
    {
        grabando = true;

        m_DictationRecognizer.Start();
        m_DictationRecognizer.InitialSilenceTimeoutSeconds = 3;

        yield return new WaitForSeconds(3);

        m_DictationRecognizer.Stop();

        Debug.Log("Termina");
    }

    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        fraseReconocida = text;
    }

    private void DictationRecognizer_DictationHypothesis(string text)
    {
        //Debug.LogFormat("Dictation hypothesis: {0}", text);
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause completionCause)
    {
        tareaEmpezada = false;
        permisoParaHablar = false;
        grabando = false;

        GetComponent<UltimateCharacterLocomotionHandler>().enabled = true;
        aliado.GetComponent<ProcesamientoTexto>().procesaResultado(fraseReconocida);
        fraseReconocida = null;

        if (completionCause != DictationCompletionCause.Complete)
            Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
    }

    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
    }


    private void OnDestroy()
    {
        m_DictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
        m_DictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        m_DictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
        m_DictationRecognizer.DictationError -= DictationRecognizer_DictationError;
        m_DictationRecognizer.Dispose();
    }


    public void preparaOrdenes(int ordenes)
    {
        switch (ordenes)
        {
            case 0:
                apareceMensaje = false;
                listaOrdenes = new List<string>();

                if(GetComponent<CharacterHealth>().HealthValue < 100)
                {
                    listaOrdenes.Add( "- Curame" );
                }
                listaOrdenes.Add("- Explicame la mision");
                listaOrdenes.Add("- Resumen de los controles");
                break;
            case 1:
                apareceMensaje = true;
                listaOrdenes = new List<string> { "- Ataca", "- Sigueme y curame" };
                break;
            case 2:
                if (!reto2Terminado)
                {
                    apareceMensaje = true;
                    listaOrdenes = new List<string> { };
                    if (aliado.GetComponent<ScriptIA>().objetivo.name == "PosicionEspera1")
                    {
                        listaOrdenes.Add("- Ve al escondite de la izquierda");
                        listaOrdenes.Add("- Ve al escondite de la derecha");
                        listaOrdenes.Add("- Ve al objetivo");
                    }
                    else if (aliado.GetComponent<ScriptIA>().objetivo.name == "PosicionEspera2")
                    {
                        listaOrdenes.Add("- Ve al escondite de la derecha");
                        listaOrdenes.Add("- Ve al objetivo");
                        listaOrdenes.Add("- Ve al principio");
                    }
                    else if (aliado.GetComponent<ScriptIA>().objetivo.name == "PosicionEspera3")
                    {
                        listaOrdenes.Add("- Ve al escondite de la izquierda");
                        listaOrdenes.Add("- Ve al objetivo");
                        listaOrdenes.Add("- Ve al principio");
                    }
                    else
                    {
                        listaOrdenes.Add("- Activa el panel");
                    }
                }
                break;
            case 3:
                apareceMensaje = true;
                if(enMirador && aliado.GetComponent<ProcesamientoTexto>().enMirador)
                {
                    listaOrdenes = new List<string> { "- Dispara" };
                }
                else if (enMirador)
                {
                    listaOrdenes = new List<string> { "- Entra", "- Ve al otro mirador" };
                }
                else
                {
                    listaOrdenes = new List<string> { "- Ve por/a la izquierda", "- Ve por/a la derecha", "- Espera aqui", "- Sigueme" };
                }
                break;
            case 4:
                apareceMensaje = true;
                if (!enPanel)
                {
                    listaOrdenes = new List<string> { "- Ve por/a la izquierda", "- Ve por/a la derecha" };
                }
                else
                {
                    listaOrdenes = new List<string> { };
                    int indicePanelAliado = aliado.GetComponent<ProcesamientoTexto>().panel;

                    if (!aliado.GetComponent<ProcesamientoTexto>().panelCercano.GetComponent<controlPaneles>().activado)
                    {
                        listaOrdenes.Add("- Activa el panel");
                    }
                    if (indicePanelAliado != 0)
                    {
                        if((ladoAliado == "derecha" && controlSala4.puertasDerecha[indicePanelAliado].GetComponent<Door>().isOpened())
                            || (ladoAliado == "izquierda" && controlSala4.puertasIzquierda[indicePanelAliado].GetComponent<Door>().isOpened()))
                        {
                            listaOrdenes.Add("- Ve al panel anterior");
                        }
                    }
                    if(aliado.GetComponent<ProcesamientoTexto>().panelCercano.GetComponent<controlPaneles>().indice != 6)
                    {

                        if ((ladoAliado == "derecha" && controlSala4.puertasDerecha[indicePanelAliado + 1].GetComponent<Door>().isOpened())
                           || (ladoAliado == "izquierda" && controlSala4.puertasIzquierda[indicePanelAliado + 1].GetComponent<Door>().isOpened()))
                        {
                            listaOrdenes.Add("- Ve al panel siguiente");
                        }

                    }
                }
                break;

            case 5:
                apareceMensaje = true;
                listaOrdenes = new List<string> {"- Ataca desde lejos", "- Ataca con libertad" };
                break;
        }
    }


    public void empiezaReto4(ScriptSala4 controlSala4, string ladoAliado)
    {
        this.controlSala4 = controlSala4;
        reto4Empezado = true;
        this.ladoAliado = ladoAliado;
    }

    public void entraEnPanel(GameObject panel)
    {
        if (!reto4Terminado)
        {
            enPanel = true;
            panelCercano = panel;
            preparaOrdenes(4);
        }
    }

    public void terminaReto4()
    {
        reto4Terminado = true;
        preparaOrdenes(0);
        estadoAliado.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.name)
        {
            case "Enemigos_sala_1":
                combatiendo = true;
                break;
            case "Decision_Sala_2":
                if (!reto2Terminado || !reto2Empezado)
                {
                    enDecision2 = true;
                } 
                break;
            case "Mirador_Sala_2":
                enPosicionGuia = true;
                if (!reto2Terminado)
                {
                    estadoAliado.SetActive(true);
                }
                break;
            case "Decision_Sala_3":
                if (!reto3Terminado)
                {
                    aliado.GetComponent<ProcesamientoTexto>().entorno = "Decision3";
                    aliado.GetComponent<ProcesamientoTexto>().decision3 = other.gameObject;
                    preparaOrdenes(3);
                    enDecision3 = true;
                }
                break;
            case "MiradorDerecho":
            case "MiradorIzquierdo":
                if (!reto3Terminado)
                {
                    enMirador = true;
                    preparaOrdenes(3);
                    mirador = other.name;
                }
                break;
            case "Enemigos_sala_3":
                aliado.GetComponent<ScriptIA>().ataqueSala3(aliado.GetComponent<ProcesamientoTexto>().enMirador);
                estadoAliado.SetActive(false);
                break;
            case "Puzzle_sala_4":

                if (!reto4Empezado)
                {
                    aliado.GetComponent<ProcesamientoTexto>().entorno = "Sala4";
                    aliado.GetComponent<ProcesamientoTexto>().controlSala4 = other.GetComponent<ScriptSala4>();
                    preparaOrdenes(4);
                }

                break;
            case "consolaDerecha1":
                if (!reto4Terminado)
                {
                    controlSala4.puertasDerecha[0].GetComponent<Door>().Close();
                    entraEnPanel(other.gameObject);
                }

                break;
            case "consolaIzquierda1":
                if (!reto4Terminado)
                {
                    controlSala4.puertasIzquierda[0].GetComponent<Door>().Close();
                    entraEnPanel(other.gameObject);
                }
                break;
            case "consolaDerecha2":
            case "consolaDerecha3":
            case "consolaIzquierda2":
            case "consolaIzquierda3":
                entraEnPanel(other.gameObject);
                break;
            case "consolaReto":
                enPanel = true;
                break;
            case "DecisionFinal":
                if (other.GetComponent<ScriptSalaFinal>().final)
                {
                    aliado.GetComponent<ProcesamientoTexto>().entorno = "Final";
                    preparaOrdenes(5);
                }
                break;
            case "SalaBoss":
                enCombateFinal = true;
                break;
            case "Bip01 R Hand":
                if (!GameObject.FindGameObjectWithTag("boss").GetComponent<ScriptGato>().muerto)
                {
                    GetComponent<CharacterHealth>().Damage(20);
                }
                break;

        }

    }

    

    private void OnTriggerStay(Collider other)
    {

        switch (other.name)
        {
            case "Decision_Sala_1":
                if (!tareaEmpezada && apareceMensaje && Time.timeScale > 0)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa V para dar las ordenes del reto 1";
                }
                else
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }
                break;
            case "Decision_Sala_2":
                if (Input.GetButtonDown("Action") && !reto2Empezado)
                {
                    reto2Empezado = true;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<UltimateCharacterLocomotionHandler>().enabled = false;
                    enDecision2 = false;
                }
                break;
            case "Mirador_Sala_2":
                if (!tareaEmpezada && apareceMensaje && reto2Empezado && !reto2Terminado && Time.timeScale > 0)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa V para dar las ordenes del reto 2";
                }
                else
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }
                break;

            case "Decision_Sala_3":
                if (!tareaEmpezada && apareceMensaje && !(reto3Empezado && !ordenReversible) && !reto3Terminado && Time.timeScale > 0)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa V para dar las ordenes del reto 3";
                }
                else
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }
                break;
            case "MiradorDerecho":
            case "MiradorIzquierdo":
                if (!tareaEmpezada && apareceMensaje && !combatiendo && !reto3Terminado)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa V para dar las ordenes del mirador";
                }
                else
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }
                break;
            case "Puzzle_sala_4":
                if (!tareaEmpezada && apareceMensaje && !reto4Empezado  && !reto4Terminado && Time.timeScale > 0)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa V para dar las ordenes del reto 4";
                }
                else if(!enPanel)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }
                break;
            case "consolaDerecha1":
            case "consolaDerecha2":
            case "consolaDerecha3":
            case "consolaIzquierda1":
            case "consolaIzquierda2":
            case "consolaIzquierda3":

                if(!tareaEmpezada && apareceMensaje && aliado.GetComponent<ProcesamientoTexto>().enColliderPanel && !reto4Terminado)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa V para dar las ordenes";

                    if (panelCercano.GetComponent<controlPaneles>().valido)
                    {
                        textoMensaje.GetComponent<Text>().text += "\nPulsa F para activar el panel";
                    }

                }
                else
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }

                break;
            case "consolaReto":
                if (!reto4Terminado)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa F para activar el panel";

                }
                else
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }

                break;
            case "DecisionFinal":
                if(!tareaEmpezada && apareceMensaje && other.GetComponent<ScriptSalaFinal>().final && Time.timeScale > 0)
                {
                    textoMensaje.transform.parent.gameObject.SetActive(true);
                    textoMensaje.GetComponent<Text>().text = "Pulsa V para dar las ultimas ordenes";
                }
                else
                {
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                }
                break;
        }


    }

    private void OnTriggerExit(Collider other)
    {

        switch (other.name)
        {
            case "Enemigos_sala_1":
                combatiendo = false;
                aliado.GetComponent<ScriptIA>().accionDescansa();
                preparaOrdenes(0);
                break;
            case "Decision_Sala_1":
                textoMensaje.transform.parent.gameObject.SetActive(false);
                break;
            case "Decision_Sala_2":
                enDecision2 = false;
                break;
            case "Mirador_Sala_2":
                textoMensaje.transform.parent.gameObject.SetActive(false);
                estadoAliado.SetActive(false);
                enPosicionGuia = false;
                break;
            case "Decision_Sala_3":
                textoMensaje.transform.parent.gameObject.SetActive(false);
                if (!reto3Empezado)
                {
                    aliado.GetComponent<ProcesamientoTexto>().entorno = "EntornoGeneral";
                    preparaOrdenes(0);
                }
                enDecision3 = false;
                break;
            case "MiradorDerecho":
            case "MiradorIzquierdo":
                textoMensaje.transform.parent.gameObject.SetActive(false);
                enMirador = false;
                mirador = null;
                break;
            case "Puzzle_sala_4":
                textoMensaje.transform.parent.gameObject.SetActive(false);
                if (!reto4Empezado)
                {
                    aliado.GetComponent<ProcesamientoTexto>().entorno = "EntornoGeneral";
                    aliado.GetComponent<ProcesamientoTexto>().controlSala4 = null;
                    preparaOrdenes(0);
                }
                break;
            case "consolaDerecha1":
            case "consolaDerecha2":
            case "consolaDerecha3":
            case "consolaIzquierda1":
            case "consolaIzquierda2":
            case "consolaIzquierda3":
                enPanel = false;
                panelCercano = null;
                break;
            case "DecisionFinal":
                if (other.GetComponent<ScriptSalaFinal>().final)
                {
                    aliado.GetComponent<ProcesamientoTexto>().entorno = "EntornoGeneral";
                    textoMensaje.transform.parent.gameObject.SetActive(false);
                    preparaOrdenes(0);
                }
                break;
            case "SalaBoss":
                enCombateFinal = false;
                break;

        }
    }




    


}
