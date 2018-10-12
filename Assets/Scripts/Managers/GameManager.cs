using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{

    private float resourceTickTime;
    private float resourceTickCounter;
    private List<GathererAI> gatherers;
    private string lastResourceName;
    private List<ResourceNode> resourceNodeList;
    private List<StorageNode> storageNodeList;

    public EventHandler OnResourceTickTimeUp;

    private void Awake()
    {
        resourceNodeList = new List<ResourceNode>();
        storageNodeList = new List<StorageNode>();
        resourceTickTime = 3f;        //1f is about one second
        resourceTickCounter = 0f;
        gatherers = new List<GathererAI>();
        lastResourceName = "Resource #0";
    }

    public void UpdateGathererAI()
    {
        UpdateGameData();
    }

    public void UpdateGameData()
    {
        List<ResourceNode> grabbedResources = GrabResourceNodes();
        int sizeOfList = grabbedResources.Count;
        if (sizeOfList > 0)
        { 
            for (int i = 0; i < sizeOfList; i++)
            {
                resourceNodeList.Add(grabbedResources[i]);
            }
        }
        List<StorageNode> grabbedStorages = GrabStorageNodes();
        sizeOfList = grabbedStorages.Count;
        if(sizeOfList > 0)
        {
            for (int i = 0; i < sizeOfList; i++)
            {
                storageNodeList.Add(grabbedStorages[i]);
            }
        }
    }

    public void AddGatherer(GathererAI ai)
    {
        Debug.Log("Adding ai");
        gatherers.Add(ai);
    }

    public List<ResourceNode> GrabResourceNodes()
    {
        List<ResourceNode> resources = new List<ResourceNode>();
        GameObject[] collection = GameObject.FindGameObjectsWithTag("Resource");
        bool match = false;
        
        for(int i = 0; i < collection.Length; i++)
        {
            match = false;
            foreach (ResourceNode node in resourceNodeList)
            {
                if (collection[i].transform.position == node.GetPosition())
                {
                    match = true;
                }
            }
            if(!match)
                resources.Add(new ResourceNode(collection[i].transform));
        }

        return resources;
    }

    public List<StorageNode> GrabStorageNodes()
    {
        List<StorageNode> storages = new List<StorageNode>();
        GameObject[] collection = GameObject.FindGameObjectsWithTag("Storage");
        bool match = false;

        for (int i = 0; i < collection.Length; i++)
        {
            match = false;
            foreach (StorageNode node in storageNodeList)
            {
                if(collection[i].transform.position == node.GetPosition())
                {
                    match = true;
                }
            }
            if(!match)
                storageNodeList.Add(new StorageNode(collection[i].transform));
        }

        return storageNodeList;
    }

    private void Update()
    {
        if (resourceTickCounter <= 0)
        {
            resourceTickCounter = resourceTickTime;
            if(OnResourceTickTimeUp != null)
                OnResourceTickTimeUp(this, EventArgs.Empty);
        }
        else
        {
            resourceTickCounter -= Time.deltaTime;
        }
    }

    public string GenerateResourceName()
    {
        string[] number = Regex.Split(lastResourceName, @"\D+");
        lastResourceName = "Resource #" + (int.Parse(number[1]) + 1);
        return lastResourceName;
    }

    public ResourceNode FindNodeFromPosition(Vector3 position)
    {
        foreach(ResourceNode node in resourceNodeList)
        {
            if(node.GetPosition() == position)
            {
                return node;
            }
        }
        return null;
    }

}
