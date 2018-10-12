using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnMouseEnter()
    {
        builder.SetBuildPreview(this.transform.position);
        GetComponentInChildren<MeshRenderer>().sharedMaterial = highlightedMat;
        nodeSelected = true;
    }

    private void OnMouseExit()
    {
        GetComponentInChildren<MeshRenderer>().sharedMaterial = normalMat;
        nodeSelected = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && nodeSelected && !ui.mouseOverUI)
        {
            if (!isBuiltOn && ui.currentMode == UIManager.GAMEMODES.BUILD)
            {
                if (builder.GetCurrentBuilding() != null)
                { 
                    this.GetComponentInChildren<MeshRenderer>().enabled = false;
                    building = (GameObject)Instantiate(builder.GetCurrentBuilding(), new Vector3(this.transform.position.x, builder.GetCurrentBuilding().GetComponent<BuildingInfo>().spawnHeight, this.transform.position.z), builder.GetCurrentBuilding().transform.rotation, navmesh.surface.gameObject.transform);
                    isBuiltOn = true;
                    navmesh.UpdateMesh();
                    game.UpdateGathererAI();
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
