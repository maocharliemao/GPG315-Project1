using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawMeshFull : MonoBehaviour
{
    public static DrawMeshFull Instance { get; private set; }

    [SerializeField] private Material drawMeshMaterial;

    private GameObject lastGameObject;
    private int lastSortingOrder;
    private Mesh mesh;
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Vector2> uvs;
    private Vector3 lastMouseWorldPosition;
    private float lineThickness = 0.1f;
    private Color lineColor = Color.green;
    private bool isDrawing = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isDrawing || IsPointerOverUI()) return;

        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0))
        {
            CreateMeshObject();
            InitializeMesh(mouseWorldPosition);
        }

        if (Input.GetMouseButton(0))
        {
            float minDistance = 0.1f;
            if (Vector3.Distance(lastMouseWorldPosition, mouseWorldPosition) > minDistance)
            {
                AddLinePoint(mouseWorldPosition);
                lastMouseWorldPosition = mouseWorldPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            FinalizeMesh();
        }
    }

    public void SetDrawing(bool value)
    {
        isDrawing = value;
    }

    private void CreateMeshObject()
    {
        lastGameObject = new GameObject("DrawMeshSingle", typeof(MeshFilter), typeof(MeshRenderer));
        lastSortingOrder++;
        lastGameObject.GetComponent<MeshRenderer>().sortingOrder = lastSortingOrder;

        Material material = new Material(drawMeshMaterial);
        material.color = lineColor;
        lastGameObject.GetComponent<MeshRenderer>().material = material;
    }

    private void InitializeMesh(Vector3 startPosition)
    {
        mesh = new Mesh();
        mesh.MarkDynamic();

        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();

        AddLinePoint(startPosition);

        lastGameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    private void AddLinePoint(Vector3 newPosition)
    {
        if (vertices.Count < 2)
        {
            vertices.Add(newPosition + Vector3.up * lineThickness);
            vertices.Add(newPosition - Vector3.up * lineThickness);
        }
        else
        {
            int lastIndex = vertices.Count;
            vertices.Add(newPosition + Vector3.up * lineThickness);
            vertices.Add(newPosition - Vector3.up * lineThickness);

            triangles.Add(lastIndex - 2);
            triangles.Add(lastIndex);
            triangles.Add(lastIndex - 1);

            triangles.Add(lastIndex - 1);
            triangles.Add(lastIndex);
            triangles.Add(lastIndex + 1);
        }

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }

    private void FinalizeMesh()
    {
        mesh.RecalculateBounds();
    }

    public void SetThickness(float thickness)
    {
        lineThickness = thickness;
    }

    public void SetColor(Color color)
    {
        lineColor = color;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Adjust depth if necessary
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}