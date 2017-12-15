using UnityEngine;

namespace Facehead
{
    public class ClothBehaviour : MonoBehaviour
    {
        // fields
        public GameObject prefab;
        public int width = 10;
        public int length = 10;
        public float padding = 5f;
        public float tightness = 1f;
        public float dampingFactor = 1f;
        public float airDensity = 1f;
        public float airDrag = 1f;

        public ClothSystem cloth;

        // Unity methods
        private void Awake()
        {
            cloth = new ClothSystem(width, length, padding, tightness, dampingFactor, airDensity, airDrag);            
        }

        private void Start()
        {            
            int counter = 0;
            foreach (Particle p in cloth.particles)
            {
                var a = Instantiate(prefab);
                //var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                a.name = string.Format("Particle::{0}", counter);
                var pb = a.AddComponent<ParticleBehaviour>();
                pb.Particle = p;
                counter++;
            }

            GeneratePlane();
        }

        private void FixedUpdate()
        {
            cloth.Update_Data();
        }

        // methods 
        void GeneratePlane()
        {
            // You can change that line to provide another MeshFilter
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            Mesh mesh = filter.mesh;
            mesh.Clear();

            float length = 1f;
            float width = 1f;
            int resX = 2; // 2 minimum
            int resZ = 2;

            #region Vertices
            
            //// default code
            //Vector3[] vertices = new Vector3[resX * resZ];
            //for (int z = 0; z < resZ; z++)
            //{
            //    // [ -length / 2, length / 2 ]
            //    float zPos = ((float)z / (resZ - 1) - .5f) * length;
            //    for (int x = 0; x < resX; x++)
            //    {
            //        // [ -width / 2, width / 2 ]
            //        float xPos = ((float)x / (resX - 1) - .5f) * width;
            //        vertices[x + z * resX] = new Vector3(xPos, 0f, zPos);
            //    }
            //}

            // custom code
            Vector3[] vertices = new Vector3[cloth.particles.Count];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = cloth.particles[i].position;
            }
            #endregion

            #region Normales
            Vector3[] normales = new Vector3[vertices.Length];
            for (int n = 0; n < normales.Length; n++)
                normales[n] = Vector3.up;
            #endregion

            #region UVs		
            Vector2[] uvs = new Vector2[vertices.Length];
            for (int v = 0; v < resZ; v++)
            {
                for (int u = 0; u < resX; u++)
                {
                    uvs[u + v * resX] = new Vector2((float)u / (resX - 1), (float)v / (resZ - 1));
                }
            }
            #endregion

            #region Triangles
            int nbFaces = (resX - 1) * (resZ - 1);
            int[] triangles = new int[nbFaces * 6];
            int t = 0;
            for (int face = 0; face < nbFaces; face++)
            {
                // Retrieve lower left corner from face ind
                int i = face % (resX - 1) + (face / (resZ - 1) * resX);

                triangles[t++] = i + resX;
                triangles[t++] = i + 1;
                triangles[t++] = i;

                triangles[t++] = i + resX;
                triangles[t++] = i + resX + 1;
                triangles[t++] = i + 1;
            }
            #endregion

            
            mesh.vertices = vertices;
            mesh.normals = normales;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
        }
    }
}
