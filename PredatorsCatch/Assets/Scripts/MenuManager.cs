using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject [] menuPrincipal;
    public GameObject mision;
    public GameObject instrucciones;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nuevaPartida(string carga)
    {
        if(carga == "mision")
        {
            StartCoroutine("cuentaMision");
        }
        else if(carga == "instrucciones")
        {
            StartCoroutine("muestraInstrucciones");
        }
    }

    public IEnumerator cuentaMision()
    {
        foreach(GameObject objeto in menuPrincipal)
        {
            objeto.SetActive(false);
        }

        mision.SetActive(true);

        while(mision.GetComponent<Image>().color.a < 1)
        {
            Color color = mision.GetComponent<Image>().color;
            mision.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a + 0.1f);

            for(int i = 0; i < mision.transform.childCount - 1; i++)
            {
                Color colorHijo = mision.transform.GetChild(i).GetComponent<Text>().color;
                mision.transform.GetChild(i).GetComponent<Text>().color = new Color(colorHijo.r, colorHijo.g, colorHijo.b, colorHijo.a + 0.1f);
            }
            yield return null;
        }
        mision.transform.GetChild(mision.transform.childCount - 1).gameObject.SetActive(true);

    }

    public IEnumerator muestraInstrucciones()
    {
        mision.SetActive(false);

        instrucciones.SetActive(true);

        while (instrucciones.GetComponent<Image>().color.a < 1)
        {
            Color color = instrucciones.GetComponent<Image>().color;
            instrucciones.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a + 0.1f);

            for (int i = 0; i < instrucciones.transform.childCount - 1; i++)
            {
                Color colorHijo = instrucciones.transform.GetChild(i).GetComponent<Text>().color;
                instrucciones.transform.GetChild(i).GetComponent<Text>().color = new Color(colorHijo.r, colorHijo.g, colorHijo.b, colorHijo.a + 0.1f);
            }
            yield return null;
        }
        instrucciones.transform.GetChild(instrucciones.transform.childCount - 1).gameObject.SetActive(true);
    }


    public void cargaJuego()
    {
        SceneManager.LoadScene(1);
    }

    public void quitarJuego()
    {
        Application.Quit();
    }

}
