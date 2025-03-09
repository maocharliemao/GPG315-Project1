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

        if (GUILayout.Button("Start Drawing 2D"))
        {
            StartDrawing2D();
        }

        if (GUILayout.Button("Start Drawing 3D"))
        {
            StartDrawing3D();
        }

        if (GUILayout.Button("Erase Tool"))
        {
            ApplyEraseTool();
        }


        if (GUILayout.Button("Fill Canvas White"))
        {
            WhiteCanvas();
        }
        
        if (GUILayout.Button("transparent"))
        {
           Trans();
        }
        if (GUILayout.Button("remove"))
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
    private void DeleteCanvas(string canvasName)
    {
        GameObject existingCanvas = GameObject.Find(canvasName);
        if (existingCanvas != null)
        {
            Debug.Log("Destroying " + canvasName);
            DestroyImmediate(existingCanvas);
        }
    }
    
    private void RemoveCanvas()
    {
        DeleteCanvas("Paint2DCanvas");
        DeleteCanvas("Paint3DCanvas");
    }

}