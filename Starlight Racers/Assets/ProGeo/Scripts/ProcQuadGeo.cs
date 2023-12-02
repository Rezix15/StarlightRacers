using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcQuadGeo : MonoBehaviour
{
    public void Awake()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Quad";

        //lists the vertices of the quad
        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(-1, 1),
            new Vector3(1, 1),
            new Vector3(-1, -1),
            new Vector3(1, -1),
        };

        
        int[] triIndices = new int[]
        {
            1,0,2,
            3,1,2
        };
        
        //generate UVs for mesh
        List<Vector2> uvs = new List<Vector2>()
        {
            new Vector2(1, 1),
            new Vector2(0,1),
            new Vector2(1,0),
            new Vector2(0,0)
        };

        //define the normals of our quad mesh in local space
        List<Vector3> normals = new List<Vector3>()
        {
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
            Vector3.forward
        };
        
        //calculates normals automatically
        //mesh.RecalculateNormals();
        
        mesh.SetVertices(points);
        mesh.triangles = triIndices;
        mesh.SetUVs(0, uvs );
        mesh.SetNormals(normals);

        GetComponent<MeshFilter>().sharedMesh = mesh;
        
    }
}
