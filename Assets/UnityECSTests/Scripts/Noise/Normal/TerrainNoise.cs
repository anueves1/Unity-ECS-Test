using System.Diagnostics;
using UnityEngine;

namespace Normal
{
    public class TerrainNoise : MonoBehaviour
    {
        [SerializeField]
        private float m_Frequency = 0.8f;

        [SerializeField] 
        private float m_Amplitude = 2f;      

        private MeshFilter m_Filter;
        private MeshCollider m_MeshCollider;

        private void Start()
        {
            //Get the filter.
            m_Filter = GetComponent<MeshFilter>();
            
            //Get the mesh collider.
            m_MeshCollider = GetComponent<MeshCollider>();

            //Make a stopwatch and start it.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            //Get a random seed.
            var seed = UnityEngine.Random.value;
            
           //Get all the vertices.
            var vertices = m_Filter.mesh.vertices;

            //Go trough the vertices.
            for (var i = 0; i < vertices.Length; i++)
            {
                //Get the vertice's position.
                var value = vertices[i];

                //Get the x position.
                var xPos = (value.x * m_Frequency) + seed;
                //Get the z pos.
                var zPos = (value.z * m_Frequency) + seed;

                //Get the noise value.
                var noise = Mathf.PerlinNoise(xPos, zPos);

                //Get a new y value.
                value.y = noise * m_Amplitude;
                
                //Set it back.
                vertices[i] = value;
            }
            
            //Assign the vertices.
            m_Filter.mesh.vertices = vertices;
            
            //Recalculate normals.
            m_Filter.mesh.RecalculateNormals();

            //Recalculate collisions.
            m_MeshCollider.sharedMesh = m_Filter.mesh;
                                    
            //Stop the stopwatch.
            stopwatch.Stop();

            //Debug the results.
            UnityEngine.Debug.Log("NO ECS time: " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}
