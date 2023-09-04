using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private Material[] OceanMaterial;
    //[SerializeField]private int PlanetNumber;
    public static int ConstantSubdivisions = 100;
    [SerializeField]  private Material[] material;

    //private Texture2D[] perlinTexture;
    [SerializeField] Texture2D[] textures;
    [SerializeField] float perlinTextursFreqency;
    private void Start()
    {
        //GenerateCube(6, ConstantSubdivisions, 5);
        //BuildPlanetAndSmall(5, 6);
        generatePlanets(Random.Range(2,10));

        //GenerateCube(6, ConstantSubdivisions, 5);
        //BuildPlanet(5, 6).transform.position = new Vector3(10,0,0);
    }
    public void generatePlanets(float PlanetNumber)
    {
        for (int i = 0; i < PlanetNumber; i++)
        {
            float MaxSpace = Mathf.Pow(PlanetNumber, 1f / 3f) * 1000;
            float r = Random.Range(1,10f);
            GenerateCube((int)Mathf.Pow(r, 1f / 2f), ConstantSubdivisions, r,true);
            BuildPlanetAndSmall(r, 6, Random.value < 0.5f).transform.position = new Vector3(Random.Range(MaxSpace*-1, MaxSpace), Random.Range(MaxSpace * -1, MaxSpace), Random.Range(MaxSpace * -1, MaxSpace));     
        }
    }

    public struct PlaneInf
    {
        public Vector3[] vertices;
        public Vector2[] uv ;
        public int[] triangles;
        //public Texture2D[] perlinTextures;
    }
    public List<PlaneInf> PlanesInf = new List<PlaneInf>();

    public void GenerateCube(int NumberOfPlanes, int subdivisions,float SphereSize,bool DoMointions)
    {
        //randomSize = Random.Range(1f, 2f);
        //perlin2Size = Random.Range(1f, 2f);
        float halflenghtOfSide = (NumberOfPlanes * subdivisions-NumberOfPlanes);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0, halflenghtOfSide,0) , false, 0, SphereSize, DoMointions);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0,0, 0),true,0, SphereSize, DoMointions);

        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0, 0, 0), false,1, SphereSize, DoMointions);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(0, 0, halflenghtOfSide), true,1, SphereSize, DoMointions);

        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3(halflenghtOfSide, 0,0), true, 2, SphereSize, DoMointions);
        GenerateCustomPlanes(NumberOfPlanes, subdivisions, new Vector3( 0, 0,0), false, 2, SphereSize, DoMointions);


    }
    [SerializeField] float BiomFrom;
    void GenerateCustomPlanes(int NumberOfPlanes ,  int subdivisions, Vector3 Position, bool PlaneBackwards ,int side,float SphereSize, bool DoMointions)
    {
        for (int x = 0; x < NumberOfPlanes; x++)
        {
            for (int y = 0; y < NumberOfPlanes; y++)
            {
                Vector3 CubeCenter = new Vector3(1, 1, 1) * ((NumberOfPlanes * subdivisions - NumberOfPlanes) / 2);
                if (side == 0) GenerateCustomPlane(new Vector3(x, 0, y) * subdivisions + Position - new Vector3(x, 0, y), subdivisions, PlaneBackwards, side, CubeCenter, SphereSize, DoMointions);
                if (side == 1) GenerateCustomPlane(new Vector3(x, y, 0) * subdivisions + Position - new Vector3(x, y, 0), subdivisions, PlaneBackwards, side, CubeCenter, SphereSize, DoMointions);
                if (side == 2) GenerateCustomPlane(new Vector3(0, x, y) * subdivisions + Position - new Vector3(0, x, y), subdivisions, PlaneBackwards, side, CubeCenter, SphereSize, DoMointions);
            }
        }
    }
    void GenerateCustomPlane(Vector3 Position , int subdivisions, bool PlaneBackwards, int side, Vector3 CubeCenter, float SphereSize,bool DoMointions)
    {
        PlaneInf Plane = new PlaneInf();
        Plane.vertices = new Vector3[subdivisions * subdivisions];
        Plane.uv = new Vector2[Plane.vertices.Length];
        Plane.triangles = new int[subdivisions * subdivisions * 6];

        //Plane.perlinTextures = new Texture2D[textures.Length];
       // Vector3[] Ofsets = new Vector3[textures.Length]; ;
       // for (int i = 0; i < textures.Length; i++)
       // {
       //     Ofsets[i] = new Vector3(Random.Range(-10000000000,100000000000), Random.Range(-10000000000, 100000000000), Random.Range(-10000000000, 100000000000));
       //     Plane.perlinTextures[i] = new Texture2D(subdivisions, subdivisions); 
       // }

        for (int row = 0; row < subdivisions; row++)
        {
            for (int col = 0; col < subdivisions; col++)
            {
                int index = row * subdivisions + col;
                if (side == 0) Plane.vertices[index] = new Vector3(col, 0, row) + Position;
                if (side == 1) Plane.vertices[index] = new Vector3(col, row,0 ) + Position;
                if (side == 2) Plane.vertices[index] = new Vector3(0, col, row) + Position;
                Plane.vertices[index] = SpherePoint(Plane.vertices[index], CubeCenter, SphereSize, DoMointions);
                Plane.uv[index] = new Vector2((float)col / subdivisions, (float)row / subdivisions);

               // int bigestOne = 0;
                //float bigestOneValue= -1000000000000000000000000000000000000f;
               // for (int i = 0; i < textures.Length; i++)
               // {
                 //   float perlinNOise = perlinNoise.get3DPerlinNoise(Plane.vertices[index] + Ofsets[i], perlinTextursFreqency ) ;
                //    if (bigestOneValue < perlinNOise){ bigestOne = i; bigestOneValue = perlinNOise; }
                //    Plane.perlinTextures[i].SetPixel(row, col,new Color(1f,1f,1f) * perlinNOise);
                //}
               // if (bigestOneValue> BiomFrom)
               // {
               //     Plane.perlinTextures[bigestOne].SetPixel(row, col, Color.white);
               //     for (int i = 0; i < textures.Length; i++)
                //    {
               //         if(i != bigestOne) Plane.perlinTextures[i].SetPixel(row, col,Color.black);
               //     }
               // }
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
    [SerializeField]private float mointainfreqenci;
    [SerializeField] private float perlinIntesnsity;
    //private float randomSize;
    //private float perlin2Size;
    private Vector3 SpherePoint( Vector3 point, Vector3 CubeCenter,float SphereSize, bool DoPerlin)
    {
        point -= CubeCenter;
        Vector3 Normalized = point.normalized;
        Vector3 pointOnSphere = Normalized * SphereSize;
        if (DoPerlin ==true)
        {
            float perlin1 = perlinNoise.get3DPerlinNoise(pointOnSphere, mointainfreqenci * 10);
            float perlin2 = perlinNoise.get3DPerlinNoise(pointOnSphere, mointainfreqenci );
            float perlin =( perlin1/2)  * perlin2  ;
            return Normalized * SphereSize + Normalized * perlin;
        }

        return pointOnSphere;
    }
    public GameObject BuildPlanetAndSmall(float r, int planes,bool GenerateOcean)
    {
        Material localMaterial = material[Random.Range(0, material.Length)];
        GameObject BigPlanet = BuildPlanet(r, planes,true, localMaterial);
        GenerateCube(1, 10, r,false);
        GameObject SmallPlanet = BuildPlanet(r, planes,false, localMaterial);
        SmallPlanet.transform.parent = BigPlanet.transform;

        BigPlanet.transform.GetChild(0).gameObject.SetActive(false);
        if (GenerateOcean == true)
        {
            GenerateCube(1, 10, r, false);
            GameObject Ocean = BuildPlanet(r+Random.Range(0f,3f), planes, false, OceanMaterial[Random.Range(0,OceanMaterial.Length)]);
            Ocean.transform.parent = BigPlanet.transform;
            Ocean.name = "Ocean";
            Ocean.SetActive(false);
        }
        else
        {
            GameObject Ocean = new GameObject();
            Ocean.transform.parent = BigPlanet.transform;
            Ocean.name = "NotOcean";
        }
        BigPlanet.transform.GetChild(0).gameObject.SetActive(false);
        return BigPlanet;

    }
    public GameObject BuildPlanet(float r,int planes,bool buildComponents, Material localMaterial)
    {
        GameObject Parent = new GameObject();
        Parent.transform.parent = transform;
        if (buildComponents == true)
        {
            Rigidbody rg = Parent.AddComponent<Rigidbody>();
            rg.useGravity = false;
            rg.mass = 4.188685f * r * r * r;
            SphereCollider col = Parent.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 1000000000000000000;
            Gravity g = Parent.AddComponent<Gravity>();
            g.r = r;
            g.rStart = r;
            g.planes = planes;
        }
        else
        {
            SphereCollider col2 = Parent.AddComponent<SphereCollider>();
            col2.radius = r;
        }
        Parent.name = "Celestial Body";
        GameObject Chunks = new GameObject();
        Chunks.transform.parent = Parent.transform;
        Chunks.name = "Chunks";


        for (int i = 0; i < PlanesInf.Count; i++)
        {
            GameObject Plane = new GameObject();
            if (buildComponents == true)
            {
                Plane.transform.parent = Chunks.transform;
            }
            else Plane.transform.parent = Parent.transform;

            PlaneInf planeinf = PlanesInf[i];
            MeshFilter meshFilter = Plane.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = Plane.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = planeinf.vertices;
            mesh.triangles = planeinf.triangles;
            mesh.uv = planeinf.uv;

            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            meshRenderer.material = localMaterial;
           // meshRenderer.material.SetTexture("_PerlinTexture1", planeinf.perlinTextures[0]);
           //  meshRenderer.material.SetTexture("_PerlinTexture2", planeinf.perlinTextures[1]);
           // meshRenderer.material.SetTexture("_PerlinTexture3", planeinf.perlinTextures[2]);

            if (buildComponents == true)
            {
                Plane.AddComponent<MeshCollider>().convex = true;
            }
            //Plane.AddComponent<Gravity>().mass = ;
            //int verticesPerRow = subdivisions + 1;
        }
        PlanesInf.Clear();
        return Parent;
    }
}

