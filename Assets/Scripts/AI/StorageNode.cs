using UnityEngine;
using System.Collections;

public class StorageNode 
{

    private Vector2 nodeCoords;
    private TextMesh inventoryText;
    public string name;
    private GameManager manager;

    private int storageAmount;

    private GameObject myObject;

    public StorageNode(Vector2 nodeCoords, GameObject myObject)
    {
        this.nodeCoords = nodeCoords;
        this.myObject = myObject;
        inventoryText = myObject.GetComponentInChildren<TextMesh>();
        storageAmount = 0;
        manager = GameObject.FindObjectOfType<GameManager>();
        name = manager.GenerateStorageName();
        UpdateInventoryText();
    }

    public Vector3 GetPosition()
    {
        return myObject.transform.position;
    }

    public Vector2 GetNodeCoords()
    {
        return nodeCoords;
    }

    public void DropResource(int amount)
    {
        storageAmount += amount;
        UpdateInventoryText();
    }

    private void UpdateInventoryText()
    {
        inventoryText.text = "" + storageAmount;
    }
   
}
