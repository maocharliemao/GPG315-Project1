using System.Collections;
using System.Collections.Generic;
using FreeDraw;
using UnityEditor;
using UnityEngine;

public class PaintEditorTool : EditorWindow
{
    private Color penColor = Color.red;
    private float penWidth = 5f;
    private float transparency = 1f;

    [MenuItem("Tools/Drawing Settings")]
    public static void ShowWindow()
    {
        GetWindow<PaintEditorTool>("Drawing Settings");
    }

    private void OnGUI()
    {
        GUILayout.Label("Drawing Settings", EditorStyles.boldLabel);

        penColor = EditorGUILayout.ColorField("Pen Color", penColor);
        penWidth = EditorGUILayout.Slider("Pen Width", penWidth, 0f, 25f);
        transparency = EditorGUILayout.Slider("Transparency", transparency, 0f, 1f);

        if (GUILayout.Button("Apply Settings"))
        {
            ApplySettings();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Start Drawing"))
        {
            StartDrawing();
        }

        if (GUILayout.Button("Erase Tool"))
        {
            ApplyEraseTool();
        }

        if (GUILayout.Button("Reset Page"))
        {
            ResetPage();
        }
    }

    private void ApplySettings()
    {
        Drawable.PenColor = new Color(penColor.r, penColor.g, penColor.b, transparency);
        Drawable.PenWidth = (int)penWidth;
        Debug.Log("Drawing settings applied");
    }

    private void ApplyEraseTool()
    {
        Drawable.PenColor = new Color(1f, 1f, 1f, 1f);
        Drawable.PenWidth = (int)penWidth;
        Debug.Log("Erase tool activated");
    }

    private void ResetPage()
    {
        Drawable drawable = FindObjectOfType<Drawable>();
        if (drawable != null)
        {
            drawable.ResetCanvas();
            Debug.Log("Canvas reset!");
        }
        else
        {
            Debug.LogWarning("No Drawable object found in the scene.");
        }
    }

    private void StartDrawing()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PaintCanvas.prefab");

        if (prefab == null)
        {
            Debug.LogError("DrawingCanvasPrefab not found! Make sure it's in Assets/Prefabs/");
            return;
        }

        GameObject drawingCanvas = Instantiate(prefab);
        drawingCanvas.name = "DrawingCanvas_Instance";

        Debug.Log("Drawing canvas instantiated!");
    }
}