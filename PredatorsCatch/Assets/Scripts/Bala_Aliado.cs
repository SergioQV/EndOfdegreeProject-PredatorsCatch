using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala_Aliado : MonoBehaviour
{

    public float velocidad = 1f;
    // Start is called before the first frame update

    float tiempoVivo = 0f;
    public Vector3 forward;
    string objetivo;

    void Start()
    {
        objetivo = transform.parent.GetComponent<ScriptIA>().objetivo.name;
        Vector3 posObjetivo = transform.parent.GetComponent<ScriptIA>().objetivo.transform.position + new Vector3(0f, 1.3f, 0f);
        forward = (posObjetivo - transform.position).normalized;
        transform.parent = null; //ponemos el padre de las balas a null una vez han sido instanciadas para que no sigan al padre cuando éste rote
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += forward * (velocidad * Time.deltaTime);
        tiempoVivo += Time.deltaTime;

        if(tiempoVivo >= 6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "enemigo1" || collision.transform.tag == "enemigo3")
        {
            collision.transform.GetComponent<Script_EnemigoCombate>().salud -= 10f;

            if(collision.transform.tag == "enemigo3" && !collision.transform.GetComponent<Script_EnemigoCombate>().atacando)
            {
                collision.transform.parent.GetComponent<ScriptSalasCombate>().gestionaEntrada3();
            }

        }
        else if(collision.transform.tag == "boss")
        {
            collision.gameObject.GetComponent<ScriptGato>().recibeDisparos();
        }

        Destroy(gameObject);
    }
}
