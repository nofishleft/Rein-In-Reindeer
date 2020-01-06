using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMeshBaker : MonoBehaviour
{
    public SkinnedMeshRenderer renderer;

    public MeshCollider collider;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        renderer.BakeMesh(mesh);
        collider.sharedMesh = mesh;
    }
}
