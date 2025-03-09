using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PaintEditorTool : EditorWindow
{
    private Color penColor = Color.red;
    private float penWidth = 5f;
    private float transparency = 1f;
    private bool currentBackgroundIsTransparent = false;

    [MenuItem("Tools/Drawing Settings")]
    public static void ShowWindow()
    {
        GetWindow<PaintEditorTool>("Drawing Settings");
    }

    private void OnGUI()

    {
// Drawing Settings Header
        GUILayout.Label("Drawing Settings", EditorStyles.boldLabel);

        // Pen Settings
        Color newPenColor = EditorGUILayout.ColorField("Colour", penColor);
        if (newPenColor != penColor)
        {
            penColor = newPenColor;
            PaintThings.PenColor = new Color(penColor.r, penColor.g, penColor.b, transparency);
        }

        float newPenWidth = EditorGUILayout.Slider("Size", penWidth, 0f, 25f);
        if (!Mathf.Approximately(newPenWidth, penWidth))
        {
            penWidth = newPenWidth;
            PaintThings.PenWidth = (int)penWidth;
        }

        float newTransparency = EditorGUILayout.Slider("Transparency", transparency, 0f, 1f);
        if (!Mathf.Approximately(newTransparency, transparency))
        {
            transparency = newTransparency;
            PaintThings.PenColor = new Color(penColor.r, penColor.g, penColor.b, transparency);
        }

        GUILayout.Space(10);

        // Start Drawing Section
        GUILayout.Label("Start Drawing", EditorStyles.boldLabel);

        if (GUILayout.Button("Start Drawing 2D"))
        {
            StartDrawing2D();
        }

        if (GUILayout.Button("Start Drawing 3D"))
        {
            StartDrawing3D();
        }

        GUILayout.Space(10);

        // Erase Tool Section
        GUILayout.Label("Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Brush"))
        {
            ApplyBrushTool();
        }


        if (GUILayout.Button("Eraser"))
        {
            ApplyEraseTool();
        }

        GUILayout.Space(10);

        GUILayout.Label("Canvas Background", EditorStyles.boldLabel);

        if (GUILayout.Button("White Background"))
        {
            WhiteCanvas();
        }

        if (GUILayout.Button("Transparent Background"))
        {
            Trans();
        }

        GUILayout.Space(10);
        // Canvas Reset Section (White and Transparent)
        GUILayout.Label("Canvas Reset", EditorStyles.boldLabel);

        // Horizontal Layout for White and Transparent Buttons
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Reset Canvas to White"))
        {
            WhiteCanvas();
        }

        if (GUILayout.Button("Reset Canvas to Transparent"))
        {
            Trans();
        }

        GUILayout.EndHorizontal();

        // Fill Canvas Section
        GUILayout.BeginHorizontal();


        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        // Remove Canvas Section
        if (GUILayout.Button("Remove Canvas"))
        {
            RemoveCanvas();
        }
    }


    private void ApplySettings()
    {
        PaintThings.PenColor = new Color(penColor.r, penColor.g, penColor.b, transparency);
        PaintThings.PenWidth = (int)penWidth;
    }


    private void StartDrawing2D()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Paint/Paint2D.prefab");

        GameObject Paint = Instantiate(prefab);
        Paint.name = "Paint2DCanvas";
    }

    private void StartDrawing3D()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Paint/Paint3D.prefab");


        GameObject Paint = Instantiate(prefab);
        Paint.name = "Paint3DCanvas";
    }


    private void WhiteCanvas()
    {
        IPaintable paintInstance = FindObjectOfType<MonoBehaviour>() as IPaintable;
        if (paintInstance != null)
        {
            paintInstance.WhiteBackground();
        }
    }

    private void Trans()
    {
        IPaintable paintInstance = FindObjectOfType<MonoBehaviour>() as IPaintable;
        if (paintInstance != null)
        {
            paintInstance.TransparentBackground();
        }
    }

    private void ApplyBrushTool()
    {
        PaintThings paintInstance = FindObjectOfType<PaintThings>();
        if (paintInstance != null)
        {
            paintInstance.CurrentBrush = worldPoint => paintInstance.PenBrush(worldPoint); // Switch to Brush
        }
    }

    private void ApplyEraseTool()
    {
        PaintThings paintInstance = FindObjectOfType<PaintThings>();
        if (paintInstance != null)
        {
            paintInstance.CurrentBrush = worldPoint => paintInstance.EraserBrush(worldPoint); // Switch to Eraser
        }
    }


    private void DeleteCanvas(string canvasName)
    {
        GameObject existingCanvas = GameObject.Find(canvasName);
        if (existingCanvas != null)
        {
            DestroyImmediate(existingCanvas);
        }
    }

    private void RemoveCanvas()
    {
        DeleteCanvas("Paint2DCanvas");
        DeleteCanvas("Paint3DCanvas");
    }
}