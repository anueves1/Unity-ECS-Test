using System.Diagnostics;
using Unity.Entities;
using UnityEngine;

namespace Hybrid
{
    public class TerrainNoiseSystem : ComponentSystem
    {
        private struct Components
        {
            public MeshFilter Filter;
            
            public MeshCollider Collider;

            public TerrainNoiseComponent TerrainNoise;
        }
        
        protected override void OnStartRunning()
        {
            //Make a stopwatch and start it.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            foreach (var entity in GetEntities<Components>())
            {
                //Get a random seed.
                var seed = UnityEngine.Random.value;
            
                //Get all the vertices.
                var vertices = entity.Filter.mesh.vertices;

                //Go trough the vertices.
                for (var i = 0; i < vertices.Length; i++)
                {
                    //Get the vertice's position.
                    var value = vertices[i];

                    //Get the x position.
                    var xPos = (value.x * entity.TerrainNoise.Frequency) + seed;
                    //Get the z pos.
                    var zPos = (value.z * entity.TerrainNoise.Frequency) + seed;

                    //Get the noise value.
                    var noise = Mathf.PerlinNoise(xPos, zPos);

                    //Get a new y value.
                    value.y = noise * entity.TerrainNoise.Amplitude;
                
                    //Set it back.
                    vertices[i] = value;
                }
            
                //Assign the vertices.
                entity.Filter.mesh.vertices = vertices;
            
                //Recalculate normals.
                entity.Filter.mesh.RecalculateNormals();

                //Recalculate collisions.
                entity.Collider.sharedMesh = entity.Filter.mesh;
            }
                                      
            //Stop the stopwatch.
            stopwatch.Stop();

            //Debug the results.
            UnityEngine.Debug.Log("Hybrid ECS time: " + stopwatch.ElapsedMilliseconds + "ms");
        }

        protected override void OnUpdate()
        {
            
        }
    }
}