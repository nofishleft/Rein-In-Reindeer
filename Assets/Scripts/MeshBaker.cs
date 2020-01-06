using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBaker : MonoBehaviour
{
    public SkinnedMeshRenderer renderer;

    public MeshCollider collider;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.BakeMesh(mesh);
        collider.sharedMesh = mesh;
    }
}
