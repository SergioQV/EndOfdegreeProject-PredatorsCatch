using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuPausa;
    public GameObject[] esconderEnPausa;

    public bool otraPausa;

    List<GameObject> reanudables;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !otraPausa)
        {
            if(Time.timeScale == 1)
            {
                pausar();
            }
            else
            {
                reanudar();
            }
        }
    }


    void pausar()
    {
        Time.timeScale = 0;
        menuPausa.SetActive(true);

        reanudables = new List<GameObject>();

        foreach(GameObject objeto in esconderEnPausa)
        {
            if (objeto.activeInHierarchy)
            {
                reanudables.Add(objeto);
                objeto.SetActive(false);
            }
        }


    }

    public void reanudar()
    {

        Time.timeScale = 1;
        menuPausa.SetActive(false);

        foreach(GameObject objeto in reanudables)
        {
            objeto.SetActive(true);
        }

    }


    public void menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
