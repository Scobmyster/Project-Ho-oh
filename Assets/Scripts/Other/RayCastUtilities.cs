using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastUtilities
{

    public static BuildNode RayCastComponentOfBuildNodeInParent()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

        for(int i = 0; i < hits.Length; i++)
        { 
            if (hits[i].collider.gameObject.GetComponentInParent<BuildNode>() != null)
            {
                return hits[i].collider.gameObject.GetComponentInParent<BuildNode>();
            }
        }
        return null;
    }

}
