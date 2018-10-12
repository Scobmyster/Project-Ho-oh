using UnityEngine;
using System.Collections;
using System;

public class ResourceNode
{

    private Transform resourceNodeTransform;
    private TextMesh inventoryText;
    public string name;
    private GameManager manager;

    private int resourceAmount;

    public ResourceNode(Transform resourceNodeTransform)
    {
        manager = GameObject.FindObjectOfType<GameManager>();
        this.resourceNodeTransform = resourceNodeTransform;
        inventoryText = resourceNodeTransform.gameObject.GetComponentInChildren<TextMesh>();
        resourceAmount = 1;
        UpdateInventoryText();
        manager.OnResourceTickTimeUp += AddResourceEachResourceTick;
        name = manager.GenerateResourceName();
        UpdateInventoryText();
    }

    public Vector3 GetPosition()
    {
        return resourceNodeTransform.position;
    }

    public void GrabResource()
    {
        resourceAmount -= 1;
        UpdateInventoryText();
    }

    public bool HasResources()
    {
        return resourceAmount > 0;
    }

    public void UpdateInventoryText()
    {
        inventoryText.text = name + " : " + resourceAmount;
    }

    public void AddResourceEachResourceTick(object sender, EventArgs e)
    {
        resourceAmount += 1;
        UpdateInventoryText();
    }
}
