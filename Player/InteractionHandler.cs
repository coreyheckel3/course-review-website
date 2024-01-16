using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;

    private void Update()
    {
        if(Input.GetKeyDown(interactionKey))
        Interact();
    }

    private void Interact()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(transform.position, transform.forward, out hit, interactionRange));
        {
            if (Physics.Raycast(ray, out hit))
            {
                Pickup pickup = hit.transform.GetComponent<Pickup>();

                if(pickup != null)
                {
                    
                    GetComponentInParent<WindowHandler>().inventory.AddItem(pickup);
                }

                if(pickup == null)
                {
                    Debug.Log("Nothing to interact with");
                }
            }
        }

        
    }
}
