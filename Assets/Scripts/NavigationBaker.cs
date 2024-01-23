using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    public Transform[] objectsToRotate;

    private bool hasBakedNavMesh = false;

    void Update()
    {
        if (!hasBakedNavMesh)
        {
            for (int i = 0; i < surfaces.Length; i++)
            {
                surfaces[i].BuildNavMesh();
            }
            hasBakedNavMesh = true;
        }

    }
}
