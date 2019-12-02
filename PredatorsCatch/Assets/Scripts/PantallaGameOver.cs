using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PantallaGameOver : MonoBehaviour
{

    public string motivo;

    float velocidad = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).GetComponent<Text>().text = motivo;

        if (motivo != "HAS MUERTO")
        {
            velocidad = 4;
        }

        StartCoroutine("finDelJuego");

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator finDelJuego()
    {
        while (GetComponent<Image>().color.a < 1)
        {
            Color color = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a + 0.0067f * velocidad);
            yield return null;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
        UIManager uim = GameObject.Find("UIManager").GetComponent<UIManager>();

        uim.otraPausa = true;
        GameObject[] esconderEnPausa = uim.esconderEnPausa;

        foreach (GameObject objeto in esconderEnPausa)
        {
            if (objeto.activeInHierarchy)
            {
                objeto.SetActive(false);
            }
        }

        Time.timeScale = 0;


        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

    }

    public void reload()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
