using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public static int ConstantSubdivisions = 50;
    [SerializeField]  private Material material;
    private void Start()
    {
        GenerateCube(6, ConstantSubdivisions, 5);
        BuildPlanet(5, 6);

    }
    

    public struct PlaneInf
    {
        public Vector3[] vertices;
        public Vector2[] uv ;
        public int[] triangles;
    }
    public List<PlaneInf> PlanesInf = new List<PlaneInf>();

    public void GenerateCube(int NumberOfPlanes, int subdivisions,float SphereSize)
    {
        float halflenghtOfSide = (NumberOfPlanes * subdivisions-NumberOfPlanes);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0, halflenghtOfSide,0) , false, 0, SphereSize);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0,0, 0),true,0, SphereSize);

        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0, 0, 0), false,1, SphereSize);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0, 0, halflenghtOfSide), true,1, SphereSize);

        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(halflenghtOfSide, 0,0), true, 2, SphereSize);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3( 0, 0,0), false, 2, SphereSize);


    }
    void GenerateCustomPlanes(int NumberOfPlanes ,  int subdivisions, Vector3 Position, bool PlaneBackwards ,int side,float SphereSize)
    {
        for (int x = 0; x < NumberOfPlanes; x++)
        {
            for (int y = 0; y < NumberOfPlanes; y++)
            {
                Vector3 CubeCenter = new Vector3(1, 1, 1) * ((NumberOfPlanes * subdivisions - NumberOfPlanes) / 2);
                if (side == 0) GenerateCustomPlane(new Vector3(x, 0, y) * subdivisions + Position - new Vector3(x, 0, y), subdivisions, PlaneBackwards, side, CubeCenter, SphereSize);
                if (side == 1) GenerateCustomPlane(new Vector3(x, y, 0) * subdivisions + Position - new Vector3(x, y, 0), subdivisions, PlaneBackwards, side, CubeCenter, SphereSize);
                if (side == 2) GenerateCustomPlane(new Vector3(0, x, y) * subdivisions + Position - new Vector3(0, x, y), subdivisions, PlaneBackwards, side, CubeCenter, SphereSize);
            }
        }
    }
    void GenerateCustomPlane(Vector3 Position , int subdivisions, bool PlaneBackwards, int side, Vector3 CubeCenter, float SphereSize)
    {
        PlaneInf Plane = new PlaneInf();
        Plane.vertices = new Vector3[subdivisions * subdivisions];
        Plane.uv = new Vector2[Plane.vertices.Length];
        Plane.triangles = new int[subdivisions * subdivisions * 6];

        for (int row = 0; row < subdivisions; row++)
        {
            for (int col = 0; col < subdivisions; col++)
            {
                int index = row * subdivisions + col;
                if (side == 0) Plane.vertices[index] = new Vector3(col, 0, row) + Position;
                if (side == 1) Plane.vertices[index] = new Vector3(col, row,0 ) + Position;
                if (side == 2) Plane.vertices[index] = new Vector3(0, col, row) + Position;
                Plane.vertices[index] = SpherePoint(Plane.vertices[index], CubeCenter, SphereSize);
                Plane.uv[index] = new Vector2((float)col / subdivisions, (float)row / subdivisions);

            }
        }

        if (PlaneBackwards == true)
        {
            int triangleIndex = 0;
            for (int row = 0; row < subdivisions - 1; row++)
            {
                for (int col = 0; col < subdivisions - 1; col++)
                {
                    int topLeft = row * subdivisions + col;
                    int topRight = topLeft + 1;
                    int bottomLeft = (row + 1) * subdivisions + col;
                    int bottomRight = bottomLeft + 1;

                    Plane.triangles[triangleIndex++] = topRight;
                    Plane.triangles[triangleIndex++] = bottomLeft;
                    Plane.triangles[triangleIndex++] = topLeft;

                    Plane.triangles[triangleIndex++] = bottomRight;
                    Plane.triangles[triangleIndex++] = bottomLeft;                 
                    Plane.triangles[triangleIndex++] = topRight;

                }
            }
        }
        else
        {
            int triangleIndex = 0;
            for (int row = 0; row < subdivisions - 1; row++)
            {
                for (int col = 0; col < subdivisions - 1; col++)
                {
                    int topLeft = row * subdivisions + col;
                    int topRight = topLeft + 1;
                    int bottomLeft = (row + 1) * subdivisions + col;
                    int bottomRight = bottomLeft + 1;

                    Plane.triangles[triangleIndex++] = topLeft;
                    Plane.triangles[triangleIndex++] = bottomLeft;
                    Plane.triangles[triangleIndex++] = topRight;
                    Plane.triangles[triangleIndex++] = topRight;
                    Plane.triangles[triangleIndex++] = bottomLeft;
                    Plane.triangles[triangleIndex++] = bottomRight;
                }
            }
        }
        PlanesInf.Add(Plane);
    }
    private Vector3 SpherePoint( Vector3 point, Vector3 CubeCenter,float SphereSize)
    {
        point -= CubeCenter;
        return point.normalized * SphereSize;

    }
    public GameObject BuildPlanet(float r,int planes)
    {
        GameObject Parent = new GameObject();
        Parent.transform.parent = transform;
        Rigidbody rg = Parent.AddComponent<Rigidbody>();
        rg.useGravity = false;
        rg.mass = 4.188685f * r * r * r;
        SphereCollider col = Parent.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = 1000000000000000000;
        Gravity g = Parent.AddComponent<Gravity>();
        g.r = r;
        g.planes = planes;
        Parent.name = "Celestial Body";



        for (int i = 0; i < PlanesInf.Count; i++)
        {
            GameObject Plane = new GameObject();
            Plane.transform.parent = Parent.transform;
            PlaneInf planeinf = PlanesInf[i];
            MeshFilter meshFilter = Plane.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = Plane.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = planeinf.vertices;
            mesh.triangles = planeinf.triangles;
            mesh.uv = planeinf.uv;

            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            meshRenderer.material = material;
            Plane.AddComponent<MeshCollider>().convex =true;
            //Plane.AddComponent<Gravity>().mass = ;
            //int verticesPerRow = subdivisions + 1;
        }
        PlanesInf.Clear();
        return Parent;
    }
}

