using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UI_Interaction : MonoBehaviour
{
    public GameObject ui_canvaus;
    GraphicRaycaster ui_RayCaster;

    PointerEventData click_data;
    List<RaycastResult> click_results;
    // Start is called before the first frame update
    private void Start()
    {
        ui_RayCaster = ui_canvaus.GetComponent<GraphicRaycaster>();
        click_data = new PointerEventData(EventSystem.current);
        click_results = new List<RaycastResult>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GetUiElementsClicked();
        }
    }
    void GetUiElementsClicked()
    {
        click_data.position = Mouse.current.position.ReadValue();
        click_results.Clear();

        ui_RayCaster.Raycast(click_data, click_results);

        foreach (RaycastResult result in click_results)
        {
            GameObject ui_element = result.gameObject;
            Debug.Log(ui_element.name); 
        }
    }
    
    

}
