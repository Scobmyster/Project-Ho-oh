using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildNode : MonoBehaviour
{

    public Material normalMat;
    public Material highlightedMat;
    private bool nodeSelected;
    private Vector2 nodeCoords;
    public bool isBuiltOn;
    public NodeManager manager;
    private BuildingManager builder;
    private NavmeshManager navmesh;
    private GameManager game;
    public UIManager ui;
    public GameObject building;
    public delegate void OnResourceNodePlace(ResourceNode node);
    static public event OnResourceNodePlace OnResourceNodePlaceListeners;


    public void Start()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<MeshRenderer>().sharedMaterial = normalMat;
        nodeSelected = false;
        isBuiltOn = false;
        builder = FindObjectOfType<BuildingManager>();
        ui = FindObjectOfType<UIManager>();
        navmesh = FindObjectOfType<NavmeshManager>();
        game = FindObjectOfType<GameManager>();
    }

    public void SetNodeSelected(bool nodeSelected)
    {
        this.nodeSelected = nodeSelected;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && nodeSelected && !ui.mouseOverUI)
        {
            if (!isBuiltOn && ui.currentMode == UIManager.GAMEMODES.BUILD)
            {
                if (builder.GetCurrentBuilding() != null)
                {
                    Build(builder.GetCurrentBuilding());
                }
                else
                {
                    Debug.LogError("Current building is null");
                }
            }
            if(ui.currentMode == UIManager.GAMEMODES.DISTRIBUTION && isBuiltOn)
            {
                GameObject go = (GameObject)Instantiate(builder.gatherer, new Vector3(this.transform.position.x, builder.gatherer.GetComponent<BuildingInfo>().spawnHeight, this.transform.position.z), builder.gatherer.transform.rotation);
                game.AddGatherer(go.GetComponent<GathererAI>());
            }
        }
    }

    public void Build(GameObject buildingPrefab)
    {
        if (navmesh == null)
            navmesh = FindObjectOfType<NavmeshManager>();
        if (game == null)
            game = FindObjectOfType<GameManager>();
        if (buildingPrefab.GetComponent<BuildingInfo>() == null)
        {
            Debug.LogError("You are trying to place a building without a buildinginfo script I will save you just this once and put one on for you:)");
            buildingPrefab.AddComponent<BuildingInfo>();
            buildingPrefab.GetComponent<BuildingInfo>().spawnHeight = 0.8f;
        }
        building = (GameObject)Instantiate(buildingPrefab, new Vector3(this.transform.position.x, buildingPrefab.GetComponent<BuildingInfo>().spawnHeight, this.transform.position.z), buildingPrefab.transform.rotation, navmesh.surface.gameObject.transform);
        isBuiltOn = true;
        navmesh.UpdateMesh();
        if (building.tag == "Resource")
        {
            ResourceNode node = new ResourceNode(nodeCoords, building.GetComponent<ResourceSpawnInfo>().product, building);
            game.AddResourceNode(node);
            if(OnResourceNodePlaceListeners != null)
                OnResourceNodePlaceListeners(node);

        }
        if (building.tag == "Extractor")
        {
            game.AddExtractor(building.GetComponent<Extractor>());
            building.GetComponent<Extractor>().extractorCoords = nodeCoords;
        }
        if (building.tag == "Storage")
            game.AddStorage(new StorageNode(nodeCoords, building));
    }

    public void SetBuilding(GameObject go)
    {
        building = go;
        isBuiltOn = true;
    }

    public bool BuildOnVertical()
    {
        if (manager.NodeBuiltOn(new Vector2(nodeCoords.x + 1, nodeCoords.y)) || manager.NodeBuiltOn(new Vector2(nodeCoords.x - 1, nodeCoords.y)))
        {
            return true;
        }
        return false;
    }

    public bool BuildOnHorizontal()
    {
        if (manager.NodeBuiltOn(new Vector2(nodeCoords.x, nodeCoords.y + 1)) || manager.NodeBuiltOn(new Vector2(nodeCoords.x, nodeCoords.y - 1)))
        {
            return true;

        }
        return false;
    }

    public void Demolish()
    {
        if(isBuiltOn)
        {
            if(game.FindExtractorFromPosition(building.transform.position) != null)
                game.RemoveExtractor(building);
            else if(game.FindStorageNodeFromPosition(building.transform.position) != null)
                game.RemoveStorage(building);
            Destroy(building);
            building = null;
            isBuiltOn = false;
        }
    }

    public void SetNodeCoords(Vector2 nodeCoords)
    {
        this.nodeCoords = nodeCoords;
    }

    public Vector2 GetNodeCoords()
    {
        return nodeCoords;
    }

    public void ToggleVisual()
    {
        if (GetComponentInChildren<MeshRenderer>().enabled)
            GetComponentInChildren<MeshRenderer>().enabled = false;
        else
            GetComponentInChildren<MeshRenderer>().enabled = true;
    }

}
