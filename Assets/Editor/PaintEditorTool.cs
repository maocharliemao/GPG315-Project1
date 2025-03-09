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

    [MenuItem("Tools/Ultimate Paint Tool")]
    public static void ShowWindow()
    {
        GetWindow<PaintEditorTool>("Ultimate Paint Tool");
    }

    private void OnGUI()

    {
        GUILayout.Label("Brush & Eraser Settings", EditorStyles.boldLabel);


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


        GUILayout.Label("Select a Platform", EditorStyles.boldLabel);

        if (GUILayout.Button("2D"))
        {
            StartDrawing2D();
        }

        if (GUILayout.Button("First Person (3D)"))
        {
            StartDrawing3D();
        }

        GUILayout.Space(10);

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

        GUILayout.Label("Select a Background", EditorStyles.boldLabel);

        if (GUILayout.Button("White Background"))
        {
            WhiteCanvas();
        }

        if (GUILayout.Button("Transparent Background"))
        {
            Trans();
        }

        GUILayout.Space(10);

        GUILayout.Label("Reset the Whole Canvas", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("White"))
        {
            WhiteCanvas();
        }

        if (GUILayout.Button("Transparent"))
        {
            Trans();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();

        GUILayout.Space(10);


        if (GUILayout.Button("Delete Canvas"))
        {
            RemoveCanvas();
        }
    }


    private void StartDrawing2D()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GPG315 Project Files/Paint2D.prefab");

        GameObject Paint = Instantiate(prefab);
        Paint.name = "Paint2DCanvas";
    }

    private void StartDrawing3D()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GPG315 Project Files/Paint3D.prefab");


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
            paintInstance.CurrentBrush = worldPoint => paintInstance.PenBrush(worldPoint);
        }
    }

    private void ApplyEraseTool()
    {
        PaintThings paintInstance = FindObjectOfType<PaintThings>();
        if (paintInstance != null)
        {
            paintInstance.CurrentBrush = worldPoint => paintInstance.EraserBrush(worldPoint);
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