using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Paint3DEditor : EditorWindow
{
    private Color penColor = Color.red;
    private float penWidth = 0.1f;
    private float transparency = 1f;

    [MenuItem("Tools/3D Drawing Settings")]
    public static void ShowWindow()
    {
        GetWindow<Paint3DEditor>("3D Drawing Settings");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("3D Drawing Settings", EditorStyles.boldLabel);

        penColor = EditorGUILayout.ColorField("Pen Color", penColor);
        penWidth = EditorGUILayout.Slider("Pen Width", penWidth, 0.01f, 1f);
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

        // if (GUILayout.Button("Clear Canvas"))
        // {
        //     ClearCanvas();
        // }
    }

    private void ApplySettings()
    {
        Paint3D.penColor = new Color(penColor.r, penColor.g, penColor.b, transparency);

    }

    private void ApplyEraseTool()
    {
        Paint3D.penColor = new Color(1f, 1f, 1f, 1f);

    }

    // private void ClearCanvas()
    // {
    //     Paint3D paintInstance = FindObjectOfType<Paint3D>();
    //     if (paintInstance != null)
    //     {
    //         paintInstance.ClearCanvas();
    //     }
    // }

    private void StartDrawing()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Paint/Paint3D.prefab");

        if (prefab == null)
        {
            Debug.LogError("Paint3D prefab not found at Assets/Paint3D/Paint3D.prefab");
            return;
        }

        GameObject paintCanvas = Instantiate(prefab);
        paintCanvas.name = "Paint3DCanvas";
    }
}
