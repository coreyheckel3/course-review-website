using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
 
    public GameObject InteractionInfo;
    TextMeshProUGUI interaction_text;
 
    private void Start()
    {
        interaction_text = InteractionInfo.GetComponent<TextMeshProUGUI>();
    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
 
            if (selectionTransform.GetComponent<InteractableObject>())
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                InteractionInfo.SetActive(true);
            }
            else 
            { 
                InteractionInfo.SetActive(false);
            }
 
        }
    }
}
