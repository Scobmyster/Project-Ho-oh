using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour
{

    private GameObject hoverObject;
    private RaycastHit hit;
    private Color selectColour;
    private Color pastColour;
    private bool hovering;

    void Start()
    {
        selectColour = new Color(0f, 0f, 1f);
    }

   
    void Update()
    {
        /*if(HoverOverRoad())
        {
            pastColour = hoverObject.GetComponent<Material>().color;
            hoverObject.GetComponent<Material>().color = selectColour;
            hovering = true;
        }
        else if(!HoverOverRoad() && hovering)
        {
            hoverObject.GetComponent<Material>().color = pastColour;
            hovering = false;
        }*/
    }

    private bool HoverOverRoad()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * Mathf.Infinity, Color.yellow, 50f);
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.name.Contains("Road"))
            {
                hoverObject = hit.transform.gameObject;
                return true;
            }
        }
        return false;
    }
}
