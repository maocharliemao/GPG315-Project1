using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint3D : MonoBehaviour
{
    // public Camera drawingCamera; // A camera that renders to the RenderTexture
    // public RenderTexture renderTexture; // The texture we paint on
    // public Material drawingMaterial; // Material of the object to apply the texture
    // public static Color penColor = Color.red;
    // public static int penSize = 1;
    //
    // private Texture2D drawableTexture;
    // private Vector2 previousUV;
    //
    // void Start()
    // {
    //     // Convert the RenderTexture to a Texture2D for modification
    //     RenderTexture.active = renderTexture;
    //     drawableTexture = new Texture2D(renderTexture.width, renderTexture.height);
    //     drawableTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
    //     drawableTexture.Apply();
    //     RenderTexture.active = null;
    //
    //     // Assign the drawable texture to the material
    //     drawingMaterial.mainTexture = drawableTexture;
    // }
    //
    // void Update()
    // {
    //     if (Input.GetMouseButton(0))
    //     {
    //         Ray ray = drawingCamera.ScreenPointToRay(Input.mousePosition);
    //         if (Physics.Raycast(ray, out RaycastHit hit))
    //         {
    //             Vector2 uv = hit.textureCoord;
    //             PaintOnTexture(uv);
    //         }
    //     }
    // }
    //
    // void PaintOnTexture(Vector2 uv)
    // {
    //     int x = (int)(uv.x * drawableTexture.width);
    //     int y = (int)(uv.y * drawableTexture.height);
    //
    //     Color[] colors = drawableTexture.GetPixels(x - penSize, y - penSize, penSize * 2, penSize * 2);
    //     for (int i = 0; i < colors.Length; i++) colors[i] = penColor;
    //
    //     drawableTexture.SetPixels(x - penSize, y - penSize, penSize * 2, penSize * 2, colors);
    //     drawableTexture.Apply();
    // }
}