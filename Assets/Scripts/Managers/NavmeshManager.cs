using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshManager : MonoBehaviour
{

    public NavMeshSurface surface;

	public void UpdateMesh()
    {
        surface.BuildNavMesh();
    }
}
