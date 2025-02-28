using System.Collections;
using System.Collections.Generic;
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
        if (GUILayout.Button("Fill Canvas White"))
        {
            WhiteCanvas();
        }
        if (GUILayout.Button("Remove Canvas"))
        {
            RemoveCanvas();
        }
    }

    private void ApplySettings()
    {
        Paint.PenColor = new Color(penColor.r, penColor.g, penColor.b, transparency);
        Paint.PenWidth = (int)penWidth;
        Debug.Log("Drawing settings applied");
    }

    private void ApplyEraseTool()
    {
        Paint.PenColor = new Color(1f, 1f, 1f, 1f);
        Paint.PenWidth = (int)penWidth;
        Debug.Log("Erase tool activated");
    }

    private void ResetPage()
    {
        Paint drawable = FindObjectOfType<Paint>();
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
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Paint/Paint.prefab");

        if (prefab == null)
        {
            Debug.LogError("DrawingCanvasPrefab not found! Make sure it's in Assets/Prefabs/");
            return;
        }

        GameObject Paint = Instantiate(prefab);
        Paint.name = "PaintCanvas";

        Debug.Log("Drawing canvas instantiated!");
    }
    
    private void WhiteCanvas()
    {
        Paint paintInstance = FindObjectOfType<Paint>();
        if (paintInstance != null)
        {
            paintInstance.WhiteBackground();
        }

    }
    private void RemoveCanvas()
    {
        Paint paintInstance = FindObjectOfType<Paint>();
        if (paintInstance != null)
        {
            paintInstance.TransparentBackground();
        }

    }
}