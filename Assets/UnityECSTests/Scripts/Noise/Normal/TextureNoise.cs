using System.Diagnostics;
using UnityEngine;

namespace Normal
{
    public class TextureNoise : MonoBehaviour
    {
        [SerializeField]
        private int m_Resolution = 256;

        [SerializeField]
        private float m_Frequency = 0.1f;

        private MeshRenderer m_Renderer;

        private void Start()
        {
            //Get the renderer component.
            m_Renderer = GetComponent<MeshRenderer>();

            //Make a stopwatch and start it.
            var stopwatch = new Stopwatch();
            stopwatch.Start();         
           
            //Create a new texture.
            var perlinTexture = new Texture2D(m_Resolution, m_Resolution, TextureFormat.ARGB32, false);

            //Go trough the horizontal pixels.
            for (var x = 0; x < m_Resolution; x++)
            {
                //Go trough the vertical pixels.
                for (var y = 0; y < m_Resolution; y++)
                {
                    //Get the perlin value for this pixel.
                    var perlin = Mathf.PerlinNoise(x * m_Frequency, y * m_Frequency);

                    //Create a color out of the perlin value.
                    var color = new Color(perlin, perlin, perlin, 1);

                    //Set the pixel.
                    perlinTexture.SetPixel(x, y, color);
                }
            }
            
            //Stop the stopwatch.
            stopwatch.Stop();

            //Debug the results.
            UnityEngine.Debug.Log("NO ECS time: " + stopwatch.ElapsedMilliseconds + "ms");

            //Apply the texture.
            perlinTexture.Apply();

            //Assign the texture.
            m_Renderer.material.mainTexture = perlinTexture;
        }
    }
}