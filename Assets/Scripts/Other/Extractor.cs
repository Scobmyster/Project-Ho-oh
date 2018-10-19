using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ExtractorTypes { FARM, MINER};

public class Extractor : MonoBehaviour
{

    private int resourceSize;
    public int maxResourceSize;

    public string prefabName;
    private string extractorName;

    private TextMesh inventoryText;

    private List<ResourceNode> resourceNodes;
    public ResourceProducts[] acceptedProducts;

    private GameManager game;

    public Vector2 extractorCoords;

    private GameObject myObject;

    void Start()
    {
        game = FindObjectOfType<GameManager>();
        resourceSize = 0;
        extractorName = game.GenerateExtractorName(prefabName);
        game.OnResourceTickTimeUp += AddResourceEachResourceTick;
        myObject = this.gameObject;
        inventoryText = myObject.GetComponentInChildren<TextMesh>();
        CheckForSurroundingResources();
        UpdateInventoryText();
        BuildNode.OnResourceNodePlaceListeners += CheckIfResourceIsNextToMe;
    }

    public string GetExtractorName()
    {
        return extractorName;
    }

    public void GrabResource()
    {
        resourceSize -= 1;
    }

	void Update ()
    {
		
	}

    public void UpdateInventoryText()
    {
        inventoryText.text = extractorName + " : " + resourceSize;
    }

    public void AddResourceEachResourceTick(object sender, EventArgs e)
    {
        foreach(ResourceNode node in resourceNodes)
        {
            resourceSize += 1;
        }
        UpdateInventoryText();
    }

    public bool HasResources()
    {
        return resourceSize > 0;
    }

    public Vector3 GetPosition()
    {
        return myObject.transform.position;
    }

    public Vector2 GetExtractorCoords()
    {
        return extractorCoords;
    }

    public void OnDemolish()
    {
        game.OnResourceTickTimeUp -= AddResourceEachResourceTick;

    }

    public void CheckForSurroundingResources()
    {
        List<ResourceNode> resourceNodeList = game.GrabGameResourceList();
        List<ResourceNode> surroundingResources = new List<ResourceNode>();
        Vector2[] directions = {
            new Vector2(extractorCoords.x - 1, extractorCoords.y),
            new Vector2(extractorCoords.x + 1, extractorCoords.y),
            new Vector2(extractorCoords.x, extractorCoords.y - 1),
            new Vector2(extractorCoords.x, extractorCoords.y + 1),
            new Vector2(extractorCoords.x - 1, extractorCoords.y - 1),
            new Vector2(extractorCoords.x - 1, extractorCoords.y + 1),
            new Vector2(extractorCoords.x + 1, extractorCoords.y + 1),
            new Vector2(extractorCoords.x + 1, extractorCoords.y - 1),
        };

        foreach(Vector2 direction in directions)
        {
            foreach(ResourceNode node in resourceNodeList)
            {
                if(direction == node.GetNodeCoords() && AcceptedProduct(node.GetProduct()))
                {
                    surroundingResources.Add(node);
                    break;
                }
            }
        }

        resourceNodes = surroundingResources;
        Debug.Log("Resource Node count: " + resourceNodes.Count);

    }

    public void CheckIfResourceIsNextToMe(ResourceNode node)
    {
        Vector2[] directions = {
            new Vector2(extractorCoords.x - 1, extractorCoords.y),
            new Vector2(extractorCoords.x + 1, extractorCoords.y),
            new Vector2(extractorCoords.x, extractorCoords.y - 1),
            new Vector2(extractorCoords.x, extractorCoords.y + 1),
            new Vector2(extractorCoords.x - 1, extractorCoords.y - 1),
            new Vector2(extractorCoords.x - 1, extractorCoords.y + 1),
            new Vector2(extractorCoords.x + 1, extractorCoords.y + 1),
            new Vector2(extractorCoords.x + 1, extractorCoords.y - 1),
        };

        foreach(Vector2 direction in directions)
        {
            if(direction == node.GetNodeCoords() && AcceptedProduct(node.GetProduct()))
            {
                resourceNodes.Add(node);
                break;
            }
        }
        Debug.Log("Resource Node count: " + resourceNodes.Count);
    }

    public bool AcceptedProduct(ResourceProducts proposedProduct)
    {
        foreach(ResourceProducts product in acceptedProducts)
        {
            if(proposedProduct == product)
            {
                return true;
            }
        }
        return false;
    }
}
