using UnityEngine;
using System.Collections;
using System;

public enum ResourceProducts { WOOD, WHEAT, POTATO, CARROT, STONE };

public class ResourceNode
{

    private GameManager manager;
    private ResourceProducts product;
    private GameObject myObject;
    private Vector2 nodeCoords;

    public ResourceNode(Vector2 nodeCoords, ResourceProducts product, GameObject myObject)
    {
        manager = GameObject.FindObjectOfType<GameManager>();
        this.nodeCoords = nodeCoords;
        this.product = product;
        this.myObject = myObject;
        manager.AddResourceNode(this);
    }

    public Vector3 GetPosition()
    {
        return myObject.transform.position;
    }

    public Vector2 GetNodeCoords()
    {
        return nodeCoords;
    }

    public ResourceProducts GetProduct()
    {
        return product;
    }

    public void OnDemolish()
    {

    }


}
