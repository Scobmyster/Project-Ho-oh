using UnityEngine;
using System.Collections;

public class ResourceMapNode 
{

    private BuildNode node;
    private GameObject resourcePrefab;

    public ResourceMapNode(BuildNode node, GameObject reosurcePrefab)
    {
        this.node = node;
        this.resourcePrefab = reosurcePrefab;
    }

    public BuildNode GetNode()
    {
        return node;
    }

    public GameObject GetResourcePrefab()
    {
        return resourcePrefab;
    }
}
