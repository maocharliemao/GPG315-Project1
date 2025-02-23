using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawTool : EditorWindow

{
    private Color brushColor = Color.red; // Brush color
    private GameObject targetObject;
    private bool isDrawing = false; // Flag for drawing mode
    private bool isBrushToolActive = false; // Flag for enabling brush tool
    private float brushSize = 1f; // Brush size, default to 1
    private RenderTexture renderTexture; // RenderTexture for painting

    [MenuItem("Tools/Brush Tool")]
    public static void ShowWindow()
    {
        GetWindow<DrawTool>("Brush Tool");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Paint Tool", EditorStyles.boldLabel);

        // Toggle for activating the Brush Tool
        isBrushToolActive = GUILayout.Toggle(isBrushToolActive, "Activate Brush Tool");

        // Target object selection (should be a 3D object like a Cube)
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        // Brush color selection
        brushColor = EditorGUILayout.ColorField("Brush Color", brushColor);

        // Brush size slider - updated range from 1 to 300
        GUILayout.Label("Brush Size", EditorStyles.boldLabel);
        brushSize = EditorGUILayout.Slider(brushSize, 1f, 300f); // Range set to 1 to 300
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (targetObject == null || !isBrushToolActive) return;

        // Handle tool behavior when Brush Tool is active
        Event e = Event.current;

        // Handle mouse events for drawing
        if (e.type == EventType.MouseDown && e.button == 0 && isBrushToolActive) // Left click to start drawing
        {
            isDrawing = true;
            Event.current.Use(); // Use the event to prevent it from propagating
        }
        else if (e.type == EventType.MouseUp && e.button == 0) // Stop drawing on mouse release
        {
            isDrawing = false;
            Event.current.Use(); // Use the event
        }

        // Start painting when the mouse is held down
        if (isDrawing)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            // Perform a raycast to detect the target object
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == targetObject)
                {
                    Paint(hit.point, hit.normal); // Call the Paint function when target object is hit
                }
            }
        }

        HandleUtility.Repaint(); // Update the Scene view
    }

    private void Paint(Vector3 hitPoint, Vector3 normal)
    {
        // Ensure the object has a MeshRenderer
        MeshRenderer renderer = targetObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogError("Target object does not have a MeshRenderer!");
            return;
        }

        // Check if the object has a material with a RenderTexture
        Material material = renderer.sharedMaterial;
        if (material == null)
        {
            Debug.LogError("Material on the target object is null!");
            return;
        }

        // Check if the material has a RenderTexture for painting
        if (!material.HasProperty("_MainTex"))
        {
            Debug.LogError("Material does not have a valid _MainTex (texture)!");
            return;
        }

        // Create a RenderTexture if it doesn't exist
        if (renderTexture == null)
        {
            renderTexture = new RenderTexture(1024, 1024, 0); // You can adjust this size as needed
            material.SetTexture("_MainTex", renderTexture);
        }

        // Paint on the RenderTexture
        Graphics.SetRenderTarget(renderTexture);
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);

        // Set brush color
        GL.Begin(GL.QUADS);
        GL.Color(brushColor);

        // Adjust the brush size and position based on hit point and normal
        Vector2 position = new Vector2(hitPoint.x, hitPoint.y); // Convert hitPoint to 2D position
        float size = brushSize * 10; // Brush size multiplier

        GL.Vertex3(position.x - size, position.y - size, 0);
        GL.Vertex3(position.x + size, position.y - size, 0);
        GL.Vertex3(position.x + size, position.y + size, 0);
        GL.Vertex3(position.x - size, position.y + size, 0);

        GL.End();
        GL.PopMatrix();

        Graphics.SetRenderTarget(null); // Reset the render target

        // Apply the changes
        renderTexture.DiscardContents(); // Ensure the texture is updated
    }
}