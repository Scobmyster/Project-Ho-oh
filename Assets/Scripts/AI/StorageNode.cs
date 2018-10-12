using UnityEngine;
using System.Collections;

public class StorageNode 
{

    private Transform storageNodeTransform;
    private TextMesh inventoryText;

    private int storageAmount;

    public StorageNode(Transform storageNodeTransform)
    {
        this.storageNodeTransform = storageNodeTransform;
        inventoryText = storageNodeTransform.gameObject.GetComponentInChildren<TextMesh>();
        storageAmount = 0;
        UpdateInventoryText();
    }

    public Vector3 GetPosition()
    {
        return storageNodeTransform.position;
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
