using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class QuadRingGeo : MonoBehaviour
{
    public enum UvProjection
    {
        AngularRadial,
        ProjectZ
    };
    
    private Mesh mesh;
    [Range(0.01f, 2)]
    [SerializeField] float radiusInner;
    
    [Range(0.01f, 2)]
    [SerializeField] float thickness;
    
    [Range(3,32)]
    [SerializeField] int angularSegments = 3;

    [SerializeField] private UvProjection uvProjection = UvProjection.AngularRadial;

    float RadiusOuter => radiusInner + thickness;
    private int vertexCount => angularSegments * 2;

    private void OnDrawGizmosSelected()
    {
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation, radiusInner, angularSegments);
        Gizmosfs.DrawWireCircle(transform.position, transform.rotation, RadiusOuter, angularSegments);
        
    }


    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "QuadRing";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void Update() => GenerateMesh();
    

    void GenerateMesh()
    {
        mesh.Clear();

        int vCount = vertexCount;
        
        List<Vector3> vertices = new List<Vector3>();

        //Define UVs
        List<Vector2> uvs = new List<Vector2>();
        
        //create our normals
        List<Vector3> normals = new List<Vector3>();

        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        

        for (int i = 0; i < angularSegments + 1; i++)
        {
            float t = i / (float)angularSegments;
            float angRad = t * Mathfs.TAU;

            Vector2 dir = Mathfs.GetUnitVectorByAngle(angRad);
            
            vertices.Add(dir * RadiusOuter);
            vertices.Add(dir * radiusInner);

            switch (uvProjection)
            {
                case UvProjection.AngularRadial:
                    uvs.Add(new Vector2(t, 1));
                    uvs.Add(new Vector2(t, 0));
                break;
                
                case UvProjection.ProjectZ:
                    uvs.Add(dir * 0.5f + Vector2.one * 0.5f);
                    uvs.Add(dir * ((radiusInner / RadiusOuter) * 0.5f) + Vector2.one * 0.5f);
                break;
                
                default:
                    break;
            }
            
        }


        List<int> triangleIndices = new List<int>();
        for (int i = 0; i < angularSegments; i++)
        {
            int indexRoot = i * 2;

            int indexInnerRoot = indexRoot + 1;
            int indexOuterNext = indexRoot + 2;
            int indexInnerNext = indexRoot + 3;
            
            
            triangleIndices.Add(indexRoot );
            triangleIndices.Add(indexOuterNext);
            triangleIndices.Add(indexInnerNext);
            
            triangleIndices.Add(indexRoot);
            triangleIndices.Add(indexInnerNext);
            triangleIndices.Add(indexInnerRoot);
            
        }
        
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangleIndices, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);

        
    }
}
