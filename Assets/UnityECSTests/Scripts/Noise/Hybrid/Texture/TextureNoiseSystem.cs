using System.Diagnostics;
using Unity.Entities;
using UnityEngine;

namespace  Hybrid
{
     public class TextureNoiseSystem : ComponentSystem
      {
        private struct Components
        {
            public MeshRenderer Renderer;

            public TextureNoiseComponent Noise;
        }

        protected override void OnStartRunning()
        {         
            //Make a stopwatch and start it.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            foreach (var entity in GetEntities<Components>())
            {               
                //Save the resolution in a variable for quick access.
                var resolution = entity.Noise.Resolution;

                //Do the same for the frequency.
                var frequency = entity.Noise.Frequency;
                
                //Create a new texture.
                var perlinTexture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);

                //Go trough the horizontal pixels.
                for (var x = 0; x < resolution; x++)
                {
                    //Go trough the vertical pixels.
                    for (var y = 0; y < resolution; y++)
                    {
                        //Get the perlin value for this pixel.
                        var perlin = Mathf.PerlinNoise(x * frequency, y * frequency);

                        //Create a color out of the perlin value.
                        var color = new Color(perlin, perlin, perlin, 1);

                        //Set the pixel.
                        perlinTexture.SetPixel(x, y, color);
                    }
                }              

                //Apply the texture.
                perlinTexture.Apply();

                //Assign the texture.
                entity.Renderer.material.mainTexture = perlinTexture;
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
