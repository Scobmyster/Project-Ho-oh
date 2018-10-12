using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    private List<GameObject> nodes;
    public GameObject prefabNode;
    public GameObject ground;
    private bool displayNodes;
    public GameObject prefabRoadTurn;

    private void Start()
    {
        displayNodes = false;
        int xSize = (int)ground.transform.localScale.x;
        Debug.Log(xSize);
        int zSize = (int)ground.transform.localScale.z;
        Debug.Log(zSize);
        nodes = new List<GameObject>();
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

    /*public void UpdateSurroundingNodes(Vector2 nodeCoords)
    {
        Vector2 leftDir = new Vector2(nodeCoords.x + 1, nodeCoords.y);
        Vector2 rightDir = new Vector2(nodeCoords.x - 1, nodeCoords.y);
        Vector2 topDir = new Vector2(nodeCoords.x, nodeCoords.y - 1);
        Vector2 bottomDir = new Vector2(nodeCoords.x, nodeCoords.y + 1);


        bool left = NodeBuiltOn(leftDir);
        bool right = NodeBuiltOn(rightDir);
        bool top = NodeBuiltOn(topDir);
        bool bottom = NodeBuiltOn(bottomDir);

        if(left && top && bottom && right)
        {

        }
        else if(left || right)
        {
            Debug.Log("Built on node to either left or right");
            if(top)
            {
                Debug.Log("Built on node to top");
                BuildNode centre = GrabGOAtNodeCoords(nodeCoords).GetComponent<BuildNode>();
                Destroy(centre.building);
                if(left)
                {
                    GameObject swapRoad = (GameObject)Instantiate(prefabRoadTurn, new Vector3(centre.building.transform.position.x, 0.7f, centre.building.transform.position.z), Quaternion.Euler(-90, 270, 0));
                    centre.SetBuilding(swapRoad);
                }
                else
                {
                    GameObject swapRoad = (GameObject)Instantiate(prefabRoadTurn, new Vector3(centre.building.transform.position.x, 0.7f, centre.building.transform.position.z), Quaternion.Euler(-90, 0, 0));
                    centre.SetBuilding(swapRoad);
                }
            }
            else if (bottom)
            {
                BuildNode centre = GrabGOAtNodeCoords(nodeCoords).GetComponent<BuildNode>();
                Destroy(centre.building);
                Debug.Log("NodeBuiltOn on node to bottom");
                if (left)
                {
                    GameObject swapRoad = (GameObject)Instantiate(prefabRoadTurn, new Vector3(centre.building.transform.position.x, 0.7f, centre.building.transform.position.z), Quaternion.Euler(-90, 180, 0));
                    centre.SetBuilding(swapRoad);
                }
                else
                {
                    GameObject swapRoad = (GameObject)Instantiate(prefabRoadTurn, new Vector3(centre.building.transform.position.x, 0.7f, centre.building.transform.position.z), Quaternion.Euler(-90, 90, 0));
                    centre.SetBuilding(swapRoad);
                }
            }
            else if (left)
                GrabGOAtNodeCoords(leftDir).GetComponent<BuildNode>().building.transform.rotation = Quaternion.Euler(-90, 0, 90);
            else
                GrabGOAtNodeCoords(rightDir).GetComponent<BuildNode>().building.transform.rotation = Quaternion.Euler(-90, 0, 90);
        }
        else if(top || bottom)
        {
            if (top)
                GrabGOAtNodeCoords(topDir).GetComponent<BuildNode>().building.transform.rotation = Quaternion.Euler(-90, 90, 90);
            else
                GrabGOAtNodeCoords(bottomDir).GetComponent<BuildNode>().building.transform.rotation = Quaternion.Euler(-90, 90, 90);
        }
    }*/

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
}
