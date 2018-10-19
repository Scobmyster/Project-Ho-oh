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
    private string lastStorageName;
    private List<Extractor> extractorList;
    private List<StorageNode> storageNodeList;
    private List<ResourceNode> resourceNodeList;
    private Dictionary<string, int> extractorNameToNumber;

    public EventHandler OnResourceTickTimeUp;
    public delegate void OnExtractorDemolishDelegate(Extractor node);
    static public event OnExtractorDemolishDelegate OnExtractorDemolishListeners;
    public delegate void OnStorageNodeDemolishDelegate(StorageNode node);
    static public event OnStorageNodeDemolishDelegate OnStorageDemolishListeners;
    public delegate void OnResourceNodeDemolishDelegate(ResourceNode node);
    static public event OnResourceNodeDemolishDelegate OnResourceDemolishListeners;



    private void Awake()
    {
        resourceNodeList = new List<ResourceNode>();
        extractorList = new List<Extractor>();
        storageNodeList = new List<StorageNode>();
        gatherers = new List<GathererAI>();
        extractorNameToNumber = new Dictionary<string, int>();
        resourceTickTime = 3f;        //1f is about one second
        resourceTickCounter = 0f;
        lastResourceName = "Resource #0";
        lastStorageName = "Storage #0";
    }

    public void AddGatherer(GathererAI ai)
    {
        gatherers.Add(ai);
    }

    public void AddResourceNode(ResourceNode node)
    {
        resourceNodeList.Add(node);
    }

    public void AddExtractor(Extractor extractor)
    {
        extractorList.Add(extractor);
    }

    public void AddStorage(StorageNode node)
    {
        storageNodeList.Add(node);
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

    public string GenerateExtractorName(string extractorName)
    {
        if(extractorNameToNumber.ContainsKey(extractorName))
        {
            extractorNameToNumber[extractorName] += 1;
        }
        else
        {
            extractorNameToNumber.Add(extractorName, 0);
        }
        string toReturn = extractorName + " #" + extractorNameToNumber[extractorName];
        return toReturn;
    }

    public string GenerateStorageName()
    {
        string[] number = Regex.Split(lastStorageName, @"\D+");
        lastStorageName = "Storage #" + (int.Parse(number[1]) + 1);
        return lastStorageName;
    }

    public Extractor FindExtractorFromPosition(Vector3 position)
    {
        foreach(Extractor node in extractorList)
        {
            if(node.GetPosition() == position)
            {
                return node;
            }
        }
        return null;
    }

    public StorageNode FindStorageNodeFromPosition(Vector3 position)
    {
        foreach (StorageNode node in storageNodeList)
        {
            if (node.GetPosition() == position)
            {
                return node;
            }
        }
        return null;
    }

    public ResourceNode FindResourceNodeFromPosition(Vector3 position)
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

    public List<ResourceNode> GrabGameResourceList()
    {
        return resourceNodeList;
    }

    public void RemoveExtractor(GameObject go)
    {
        Extractor extractor = FindExtractorFromPosition(go.transform.position);
        extractor.OnDemolish();
        OnExtractorDemolishListeners(extractor);
        extractorList.Remove(extractor);
    }

    public void RemoveResource(GameObject go)
    {
        ResourceNode resource = FindResourceNodeFromPosition(go.transform.position);
        resource.OnDemolish();
        OnResourceDemolishListeners(resource);
        resourceNodeList.Remove(resource);
    }

    public void RemoveStorage(GameObject go)
    {
        StorageNode storage = FindStorageNodeFromPosition(go.transform.position);
        OnStorageDemolishListeners(storage);
        storageNodeList.Remove(storage);
    }

}
