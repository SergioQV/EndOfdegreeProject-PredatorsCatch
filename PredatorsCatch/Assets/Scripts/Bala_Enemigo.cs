using Opsive.UltimateCharacterController.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala_Enemigo : MonoBehaviour
{
    public float velocidad = 1f;
    public Vector3 forward;
    bool reto2;
    float tiempoVivo = 0f;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Vector3 posObjetivo;
            if (transform.parent.tag == "enemigo1" || transform.parent.tag == "enemigo3")
            {
                posObjetivo = transform.parent.GetComponent<Script_EnemigoCombate>().objetivo.transform.position + new Vector3(0f, 1.3f, 0f);
            }
            else
            {
                posObjetivo = transform.parent.GetComponent<Script_Enemigo2>().objetivoAtaque.transform.position + new Vector3(0f, 1.3f, 0f);
                reto2 = true;
            }

            forward = (posObjetivo - transform.position).normalized;
            transform.parent = null; //ponemos el padre de las balas a null una vez han sido instanciadas para que no sigan al padre cuando éste rote
        }
        catch (System.NullReferenceException)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += forward * (velocidad * Time.deltaTime);
        tiempoVivo += Time.deltaTime;

        if (tiempoVivo >= 6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Nolan")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHealth>().Damage(5f);
        }
        else if(collision.gameObject.name == "Aliado")
        {
            collision.gameObject.GetComponent<ScriptIA>().recibeDaño();

            if (reto2)
            {
                collision.gameObject.GetComponent<ScriptIA>().gameOver.GetComponent<PantallaGameOver>().motivo = "TU ALIADO HA SIDO ATRAPADO EN EL RETO 2";
                collision.gameObject.GetComponent<ScriptIA>().gameOver.SetActive(true);
            }

        }

        Destroy(gameObject);
    }
}
