using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PaintTool : EditorWindow
{
    private Color brushColor = Color.red; // Brush color
    private bool isDrawing = false;       // Toggle for drawing
    private int brushSize = 5;            // Brush size
    private Texture2D texture;            // The texture to draw on
    private Material material;            // The material of the object

    [MenuItem("Tools/Simple 3D Drawing Tool")]
    public static void ShowWindow()
    {
        GetWindow<PaintTool>("Simple 3D Drawing Tool");
    }

    private void OnGUI()
    {
        // Simple UI for brush color and drawing toggle
        brushColor = EditorGUILayout.ColorField("Brush Color", brushColor);
        brushSize = EditorGUILayout.IntSlider("Brush Size", brushSize, 1, 20);
        isDrawing = GUILayout.Toggle(isDrawing, "Enable Drawing");

        // Refresh the window
        Repaint();
    }

    private void OnSceneGUI(UnityEditor.SceneView sceneView)
    {
        if (!isDrawing) return;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Get the material and texture
            Renderer renderer = hit.collider.GetComponent<Renderer>();

            if (renderer != null && renderer.sharedMaterial != null)
            {
                // Use sharedMaterial to avoid instantiating a new material
                material = renderer.sharedMaterial;

                // Only proceed if the texture is valid and it's a Texture2D
                if (material.mainTexture is Texture2D)
                {
                    texture = (Texture2D)material.mainTexture;

                    // Get the UV coordinates from the hit point on the object
                    Vector2 uv = hit.textureCoord;

                    // Call the DrawTexture method to draw on the texture
                    DrawTexture(uv, brushColor, brushSize);
                }
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

    // Register the Scene GUI callback
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    // Unregister the Scene GUI callback
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}