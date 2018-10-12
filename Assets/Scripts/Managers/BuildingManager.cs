using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public GameObject buildPreview;
    private GameObject currentBuilding;
    public Dictionary<string, GameObject> buildings;
    public GameObject road;
    public GameObject storage;
    public GameObject resource;
    public GameObject gatherer;

    public void Start()
    {
        StartCoroutine(LoadBuildings());
    }

    IEnumerator LoadBuildings()
    {
        buildings = new Dictionary<string, GameObject>();
        buildings.Add("Road", road);
        buildings.Add("Storage", storage);
        buildings.Add("Resource", resource);
        yield return null;
    }

    public void SetBuildPreview(Vector3 position)
    {
        buildPreview.transform.position = new Vector3(position.x, 1f, position.z);
    }

    public void SetCurrentBuilding(string name)
    {
        if (buildings.ContainsKey(name))
        {
            currentBuilding = buildings[name];
        }
        else
        {
            Debug.LogError("SetCurrentBuilding has been passed a key it does not contain: " + name);
        }
    }

    public GameObject GetCurrentBuilding()
    {
        return currentBuilding;
    }

}
