using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptGameCleared : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a + 0.005f);
            yield return null;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
}
