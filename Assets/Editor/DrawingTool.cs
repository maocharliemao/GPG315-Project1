using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class DrawingTool : EditorWindow
{
}
// {
//     private Color brushColor = Color.red;
//     private int brushSize = 5;
//     private bool isDrawing = false;
//     private Vector3 lastHitPoint;
//     private bool isHovering = false; // Detect if hovering over a valid object
//
//     [MenuItem("Tools/3D Drawing Tool")]
//     public static void ShowWindow()
//     {
//         GetWindow<DrawingTool>("3D Drawing Tool");
//     }
//
//     private void OnGUI()
//     {
//         GUILayout.Label("Drawing Settings", EditorStyles.boldLabel);
//         brushColor = EditorGUILayout.ColorField("Brush Color", brushColor);
//         brushSize = EditorGUILayout.IntSlider("Brush Size", brushSize, 1, 20);
//
//         isDrawing = GUILayout.Toggle(isDrawing, "Enable Drawing");
//
//         if (GUILayout.Button("Clear Drawing"))
//         {
//             ClearDrawings();
//         }
//
//         // Disable or enable drawing during scene view
//         SceneView.duringSceneGui -= OnSceneGUI;
//         if (isDrawing) SceneView.duringSceneGui += OnSceneGUI;
//     }
//
//     private void OnSceneGUI(SceneView sceneView)
//     {
//         Event e = Event.current;
//         Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
//         RaycastHit hit;
//
//         isHovering = false; // Reset hovering flag
//
//         if (Physics.Raycast(ray, out hit))
//         {
//             lastHitPoint = hit.point;
//             isHovering = true;
//
//             if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
//             {
//                 DrawOnObject drawScript = hit.collider.GetComponent<DrawOnObject>();
//                 if (drawScript == null)
//                 {
//                     drawScript = hit.collider.gameObject.AddComponent<DrawOnObject>();
//                     drawScript.SetDrawing(true); // Start drawing when new DrawOnObject is added
//                     drawScript.lineColor = brushColor; // Set the brush color from the editor tool
//                     drawScript.lineThickness = brushSize * 0.1f; // Adjust thickness based on brush size
//                 }
//
//                 e.Use(); // Mark event as used
//             }
//         }
//
//         // Draw Scene View Overlay
//         Handles.BeginGUI();
//         if (isDrawing)
//         {
//             GUIStyle style = new GUIStyle();
//             style.normal.textColor = Color.white;
//             style.fontSize = 14;
//             Handles.Label(lastHitPoint, "Drawing Mode Active", style);
//         }
//         Handles.EndGUI();
//
//         // Draw Brush Preview
//         if (isHovering)
//         {
//             Handles.color = new Color(brushColor.r, brushColor.g, brushColor.b, 0.5f);
//             Handles.DrawSolidDisc(lastHitPoint, Vector3.up, brushSize * 0.01f);
//         }
//     }
//
//     private void ClearDrawings()
//     {
//         foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
//         {
//             DrawOnObject drawScript = obj.GetComponent<DrawOnObject>();
//             if (drawScript != null)
//             {
//                 DestroyImmediate(drawScript); // Remove the DrawOnObject component
//             }
//         }
//
//         EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); // Mark the scene as dirty to trigger save
//     }
// }