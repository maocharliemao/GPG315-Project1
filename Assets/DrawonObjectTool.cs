using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawonObjectTool : MonoBehaviour
{
    public LineRenderer lineRenderer; // Reference to LineRenderer
    private Camera _camera; // Camera to detect mouse position
    private Vector3 _previousPosition;
    private bool _drawing = false;

    void Start()
    {
        _camera = Camera.main; // Get the main camera
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>(); // Add LineRenderer if not assigned
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = 0; // No points initially
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Start drawing on click
        {
            StartDrawing();
        }
        if (Input.GetMouseButton(0) && _drawing) // Continue drawing
        {
            ContinueDrawing();
        }
        if (Input.GetMouseButtonUp(0)) // Stop drawing
        {
            StopDrawing();
        }
    }

    void StartDrawing()
    {
        Vector3 worldPosition = GetMouseWorldPosition();
        _previousPosition = worldPosition;

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, worldPosition);
        _drawing = true;
    }

    void ContinueDrawing()
    {
        Vector3 worldPosition = GetMouseWorldPosition();

        if (worldPosition != _previousPosition)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPosition);
            _previousPosition = worldPosition;
        }
    }

    void StopDrawing()
    {
        _drawing = false;
    }

    // Convert mouse position to world position on a 3D object
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero; // Return zero if no object was hit
    }
}