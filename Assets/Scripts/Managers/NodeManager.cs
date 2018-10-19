using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeManager : MonoBehaviour {

    private List<GameObject> nodes;
    public GameObject prefabNode;
    public GameObject ground;
    private bool displayNodes;
    public GameObject prefabRoadTurn;
    public BuildingManager builder;

    private void Start()
    {
        displayNodes = false;
        int xSize = (int)ground.transform.localScale.x;
        Debug.Log(xSize);
        int zSize = (int)ground.transform.localScale.z;
        Debug.Log(zSize);
        nodes = new List<GameObject>();
        builder = FindObjectOfType<BuildingManager>();
        BuildingManager.OnLoadBuildings += GenerateResources;
        for (int x = -xSize / 2; x < xSize / 2; x++)
        {
            for (int z = -zSize / 2; z < zSize / 2; z++)
            {
                GameObject go = (GameObject)Instantiate(prefabNode, new Vector3(x + 0.5f, 0.66f, z + 0.5f), Quaternion.identity);
                go.name = "Node: (" + x + "," + z + ")";
                go.GetComponent<BuildNode>().SetNodeCoords(new Vector2(x, z));
                go.GetComponent<BuildNode>().manager = this;
                nodes.Add(go);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !displayNodes)
        {
            displayNodes = true;
            foreach(GameObject go in nodes)
            {
                go.GetComponent<BuildNode>().ToggleVisual();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Q) && displayNodes)
        {
            displayNodes = false;
            foreach (GameObject go in nodes)
            {
                go.GetComponent<BuildNode>().ToggleVisual();
            }
        }
    }

    public bool NodeBuiltOn(Vector2 nodeCoords)
    {
        BuildNode node = GrabGOAtNodeCoords(nodeCoords).GetComponent<BuildNode>();
        if (node == null)
            Debug.LogError("Could not find a node at supplied nodeCoords");
        if(node.isBuiltOn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject GrabGOAtNodeCoords(Vector2 nodeCoords)
    {
        foreach(GameObject go in nodes)
        {
            BuildNode node = go.GetComponent<BuildNode>();
            if (node.GetNodeCoords().x == nodeCoords.x && node.GetNodeCoords().y == nodeCoords.y)
            {
                return go;
            }
        }
        return null;
    }

    public void GenerateResources(object sender, EventArgs e)
    {
        /*int resourceCount = 30;
        string[] keys = new string[builder.resources.Count];
        builder.resources.Keys.CopyTo(keys, 0);
        for (int i = 0; i < resourceCount; i++)
        {
            BuildNode node = nodes[UnityEngine.Random.Range(0, nodes.Count)].GetComponent<BuildNode>();
            while(node.isBuiltOn)
            {
                node = nodes[UnityEngine.Random.Range(0, nodes.Count)].GetComponent<BuildNode>();
            }
            node.Build(builder.resources[keys[UnityEngine.Random.Range(0, keys.Length)]]);
        }*/
        List<ResourceMapNode> map = ResourceGenerator.GenerateResourceMap(nodes, builder.resources);
        foreach(ResourceMapNode node in map)
        {
            BuildNode buildNode = node.GetNode();
            buildNode.Build(node.GetResourcePrefab());
        }
    }
}
