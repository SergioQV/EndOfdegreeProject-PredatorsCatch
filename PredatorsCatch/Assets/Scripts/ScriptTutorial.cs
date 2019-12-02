using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptTutorial : MonoBehaviour
{
    public GameObject[] esconderEnPausa;

    List<GameObject> reanudables;
    bool botonHabilitado;

    // Start is called before the first frame update
    void Awake()
    {
        pausar();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0)
        {
            pausar();
        }

        Color color = GetComponent<Image>().color;

        if(color.a < 1)
        {
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a + 0.1f);

            for (int i = 0; i < transform.childCount - 1; i++)
            {
                if (transform.GetChild(i).tag == "texto")
                {
                    Color color2 = transform.GetChild(i).GetComponent<Text>().color;
                    transform.GetChild(i).GetComponent<Text>().color = new Color(color2.r, color2.g, color2.b, color2.a + 0.1f);
                }
                else
                {
                    Color color2 = transform.GetChild(i).GetComponent<Image>().color;
                    transform.GetChild(i).GetComponent<Image>().color = new Color(color2.r, color2.g, color2.b, color2.a + 0.1f);
                }
            }
        }
        else if (!botonHabilitado)
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
            botonHabilitado = true;
        }
    }


    public void pausar()
    {
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
        UIManager uim = GameObject.Find("UIManager").GetComponent<UIManager>();

        uim.otraPausa = true;
        esconderEnPausa = uim.esconderEnPausa;

        reanudables = new List<GameObject>();

        foreach (GameObject objeto in esconderEnPausa)
        {
            if (objeto.activeInHierarchy)
            {
                reanudables.Add(objeto);
                objeto.SetActive(false);
            }
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }


    public void reanudar()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;

        foreach (GameObject objeto in reanudables)
        {
            objeto.SetActive(true);
        }

        GameObject.Find("UIManager").GetComponent<UIManager>().otraPausa = false;

        reiniciar();

        gameObject.SetActive(false);

    }


    void reiniciar()
    {

        Color color = GetComponent<Image>().color;

        if (color.a > 0)
        {
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0f);

            for (int i = 0; i < transform.childCount - 1; i++)
            {
                if (transform.GetChild(i).tag == "texto")
                {
                    Color color2 = transform.GetChild(i).GetComponent<Text>().color;
                    transform.GetChild(i).GetComponent<Text>().color = new Color(color2.r, color2.g, color2.b, 0f);
                }
                else
                {
                    Color color2 = transform.GetChild(i).GetComponent<Image>().color;
                    transform.GetChild(i).GetComponent<Image>().color = new Color(color2.r, color2.g, color2.b, 0f);
                }
            }
        }
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
            botonHabilitado = false;
    }

}
