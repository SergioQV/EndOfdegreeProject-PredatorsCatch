using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Opsive.UltimateCharacterController.Events;
using Opsive.UltimateCharacterController.Traits;

public class scriptPrueba : MonoBehaviour
{
    // Start is called before the first frame update
    public void Awake()
    {
        EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnObjectImpact", OnImpact);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnImpact(float amount, Vector3 position, Vector3 forceDirection, GameObject attacker, Collider hitCollider)
    {
        
        Debug.Log(name + " impacted by " + attacker + " on collider " + hitCollider + ".");
        attacker.GetComponent<Health>().Damage(5);
        
    }

    /// <summary>
    /// The GameObject has been destroyed.
    /// </summary>
    public void OnDestroy()
    {
        EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(gameObject, "OnObjectImpact", OnImpact);
    }
}
