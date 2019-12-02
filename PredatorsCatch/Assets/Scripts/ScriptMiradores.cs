using Opsive.UltimateCharacterController.Demo.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMiradores : MonoBehaviour
{

    public GameObject puertaMirador;
    public bool retoTerminado;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if((other.name == "Nolan" || other.name == "Aliado") && !retoTerminado)
        {
            puertaMirador.GetComponent<Door>().Close();
        }
    }


}
