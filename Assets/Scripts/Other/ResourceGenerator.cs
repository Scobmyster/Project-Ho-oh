using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceGenerator
{

    private static List<BuildNode> myNodes;
    private static Dictionary<string, GameObject> myBuildings;

    public static List<ResourceMapNode> GenerateResourceMap(List<GameObject> nodes, Dictionary<string, GameObject> resourceBuildings)
    {
        if (myNodes == null)
        {
            myNodes = new List<BuildNode>();
            foreach(GameObject go in nodes)
            {
                myNodes.Add(go.GetComponent<BuildNode>());
            }
        }
        if (myBuildings == null)
            myBuildings = resourceBuildings;
        List<ResourceMapNode> resourceMapNodeList = new List<ResourceMapNode>();
        //int forestClumps = Random.Range(2, 4);
        int forestClumps = 1;
        int mineralClumps = Random.Range(2, 5);
        int naturalClumps = Random.Range(2, 6);

        for(int i = 0; i < forestClumps; i++)
        {
            CreateClump(Random.Range(2, 5), myBuildings["Forest"], resourceMapNodeList);
        }

        return resourceMapNodeList;

    }

    public static void CreateClump(int size, GameObject prefab, List<ResourceMapNode> nodeList)
    {
        Vector2 centrePoint = myNodes[Random.Range(0, myNodes.Count)].GetNodeCoords();

        for(int dx = ((int)centrePoint.x - size); dx <= ((int)centrePoint.x + size); dx++)
        {
            for(int dy = ((int)centrePoint.y - size); dy <= ((int)centrePoint.y + size); dy++)
            {
                ResourceMapNode node = new ResourceMapNode(FindNodeFromPosition(new Vector2(dx, dy)), prefab);
                nodeList.Add(node);
            }
        }
    }

    public static BuildNode FindNodeFromPosition(Vector2 nodeCoords)
    {
        for(int i = 0; i < myNodes.Count; i++)
        {
            if(myNodes[i].GetNodeCoords() == nodeCoords)
            {
                return myNodes[i];
            }
        }
        Debug.LogError("Can't find buildNode at this position: (" + nodeCoords.x + "," + nodeCoords.y + ")");
        return null;
    }

}
