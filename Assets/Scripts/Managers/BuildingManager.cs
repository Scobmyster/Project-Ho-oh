using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingManager : MonoBehaviour
{

    public GameObject buildPreview;
    private GameObject currentBuilding;
    public GameObject road;
    public GameObject storage;
    public GameObject forest;
    public GameObject gatherer;
    public GameObject farm;
    public GameObject wheatField;
    public GameObject rock;
    public GameObject carrotField;

    public Dictionary<string, GameObject> buildings;
    public Dictionary<string, GameObject> resources;

    public static EventHandler OnLoadBuildings;

    public void Start()
    {
        StartCoroutine(LoadBuildings());
    }

    IEnumerator LoadBuildings()
    {
        buildings = new Dictionary<string, GameObject>();
        buildings.Add("Road", road);
        buildings.Add("Storage", storage);
        buildings.Add("Farm", farm);
        buildings.Add("WheatField", wheatField);
        buildings.Add("CarrotField", carrotField);
        resources = new Dictionary<string, GameObject>();
        resources.Add("Forest", forest);
        resources.Add("Stone", rock);
        yield return new WaitForSeconds(1);
        OnLoadBuildings(this, EventArgs.Empty);
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
        else if (resources.ContainsKey(name))
        {
            currentBuilding = resources[name];
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
