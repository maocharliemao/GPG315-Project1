using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOnCube : MonoBehaviour
{
    private Texture2D texture;  // The texture to draw on
    private Material material;   // The material on the object

    private void Start()
    {
        // Get material from the object
        material = GetComponent<Renderer>().material;

        // Check if the material has a texture
        if (material != null)
        {
            texture = material.mainTexture as Texture2D;
            if (texture == null)
            {
                Debug.LogError("The material does not have a Texture2D.");
            }
        }
    }

    // Method to draw on the texture at a specific UV coordinate
    public void DrawTexture(Vector2 uv, Color color, int brushSize)
    {
        if (texture != null)
        {
            // Convert UV to pixel coordinates
            Vector2Int pixelPos = new Vector2Int((int)(uv.x * texture.width), (int)(uv.y * texture.height));

            // Draw on the texture by modifying the pixels
            for (int x = -brushSize; x < brushSize; x++)
            {
                for (int y = -brushSize; y < brushSize; y++)
                {
                    int px = pixelPos.x + x;
                    int py = pixelPos.y + y;

                    // Ensure we stay within the bounds of the texture
                    if (px >= 0 && py >= 0 && px < texture.width && py < texture.height)
                    {
                        texture.SetPixel(px, py, color);
                    }
                }
            }

            texture.Apply();  // Apply the changes to the texture
        }
    }
}