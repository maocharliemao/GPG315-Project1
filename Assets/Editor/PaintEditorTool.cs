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
    }

    private void ApplyEraseTool()
    {
        Paint.PenColor = new Color(1f, 1f, 1f, 1f);
        Paint.PenWidth = (int)penWidth;
    }



    private void StartDrawing()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Paint/Paint2D.prefab");

        if (prefab == null)
        {
            return;
        }

        GameObject Paint = Instantiate(prefab);
        Paint.name = "PaintCanvas";
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