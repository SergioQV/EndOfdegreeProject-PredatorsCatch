using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaError : MonoBehaviour
{


    private void Start()
    {

        if (Time.timeScale == 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(tiempoVida());
    }


    IEnumerator tiempoVida()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

}
