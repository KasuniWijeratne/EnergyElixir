using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbineInteract : MonoBehaviour
{
    // Start is called before the first frame update

    private bool playerInRange;
    private WindTurbine windTurbine;
    void Start()
    {
        windTurbine = GetComponent<WindTurbine>();
        if (windTurbine == null)
        {
            Debug.LogError("WindTurbine component is not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Return) )
        {
            windTurbine.SwitchOnTurbine();
            Debug.Log("Wind Turbine is switched on.");
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player is in range.");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player is out of range.");
        }
    }
}
