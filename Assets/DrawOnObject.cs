using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshRenderer))]
public class DrawOnObject : MonoBehaviour
{}
// {
//     [SerializeField] private Material drawMeshMaterial;
//     private Texture2D texture;
//     private bool isDrawing;
//     public float lineThickness = 0.1f;
//     public Color lineColor = Color.green;
//     private Vector3 lastMouseWorldPosition;
//     private GameObject lastDrawingObject;
//
//     private void Start()
//     {
//         // Automatically fetch texture if material is assigned
//         texture = drawMeshMaterial?.mainTexture as Texture2D;
//         if (!texture) Debug.LogError("Texture not assigned.");
//     }
//
//     private void Update()
//     {
//         if (!isDrawing || IsPointerOverUI()) return;
//
//         Vector3 mouseWorldPosition = GetMouseWorldPosition();
//
//         if (Input.GetMouseButtonDown(0))
//         {
//             StartDrawing(mouseWorldPosition);
//         }
//
//         if (Input.GetMouseButton(0))
//         {
//             ContinueDrawing(mouseWorldPosition);
//         }
//
//         if (Input.GetMouseButtonUp(0))
//         {
//             FinalizeDrawing();
//         }
//     }
//
//     private void StartDrawing(Vector3 startPosition)
//     {
//         lastDrawingObject = CreateDrawingObject();
//         DrawLine(startPosition);
//     }
//
//     private void ContinueDrawing(Vector3 newPosition)
//     {
//         if (Vector3.Distance(lastMouseWorldPosition, newPosition) > 0.1f)
//         {
//             DrawLine(newPosition);
//             lastMouseWorldPosition = newPosition;
//         }
//     }
//
//     private void DrawLine(Vector3 position)
//     {
//         // Draw line on the mesh and apply texture changes
//         AddLinePoint(position);
//         if (texture) ApplyTextureAtPosition(position);
//     }
//
//     private void ApplyTextureAtPosition(Vector3 worldPosition)
//     {
//         Vector2 uv = MapWorldToUV(worldPosition);
//         SetTexturePixel(uv);
//     }
//
//     private void SetTexturePixel(Vector2 uv)
//     {
//         int x = Mathf.FloorToInt(uv.x * texture.width);
//         int y = Mathf.FloorToInt(uv.y * texture.height);
//         texture.SetPixel(x, y, Color.red);
//         texture.Apply();
//     }
//
//     private Vector2 MapWorldToUV(Vector3 worldPosition)
//     {
//         Vector3 localPosition = transform.InverseTransformPoint(worldPosition);
//         float u = Mathf.InverseLerp(-transform.localScale.x / 2, transform.localScale.x / 2, localPosition.x);
//         float v = Mathf.InverseLerp(-transform.localScale.y / 2, transform.localScale.y / 2, localPosition.y);
//         return new Vector2(Mathf.Clamp01(u), Mathf.Clamp01(v));
//     }
//
//     private GameObject CreateDrawingObject()
//     {
//         GameObject drawingObject = new GameObject("DrawingObject", typeof(MeshFilter), typeof(MeshRenderer));
//         MeshRenderer meshRenderer = drawingObject.GetComponent<MeshRenderer>();
//         meshRenderer.material = new Material(drawMeshMaterial) { color = lineColor };
//         return drawingObject;
//     }
//
//     private void AddLinePoint(Vector3 position)
//     {
//         Mesh mesh = lastDrawingObject.GetComponent<MeshFilter>().mesh ?? new Mesh();
//         mesh.MarkDynamic();
//
//         List<Vector3> vertices = new List<Vector3>
//         {
//             position + Vector3.up * lineThickness,
//             position - Vector3.up * lineThickness
//         };
//
//         mesh.vertices = vertices.ToArray();
//         mesh.triangles = new[] { 0, 1, 2 };
//         mesh.RecalculateNormals();
//
//         lastDrawingObject.GetComponent<MeshFilter>().mesh = mesh;
//     }
//
//     private void FinalizeDrawing() => lastDrawingObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
//
//     public void SetDrawing(bool value) => isDrawing = value;
//
//     private Vector3 GetMouseWorldPosition()
//     {
//         Vector3 mousePos = Input.mousePosition;
//         mousePos.z = 10f; // Adjust depth as needed
//         return Camera.main.ScreenToWorldPoint(mousePos);
//     }
//
//     private bool IsPointerOverUI() => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
// }