using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Paint : MonoBehaviour
{
    public delegate void BrushFunction(Vector2 worldPosition);

    public BrushFunction CurrentBrush;
    public Camera mainCamera;

    public static Color PenColor = Color.red;
    public static int PenWidth = 3;

    private Sprite drawableSprite;
    private Texture2D drawableTexture;
    private Vector2 previousDragPosition;
    private Color32[] currentColors;

    // void Start()
    // {
    //     if (mainCamera == null)
    //     {
    //         mainCamera = Camera.main; // Try to get Camera.main after scene setup
    //     }
    //
    //     if (mainCamera == null)
    //     {
    //         Debug.LogError("Main Camera not found. Please ensure a camera is tagged 'MainCamera'.");
    //     }
    // }

    void Awake()
    {
        CurrentBrush = ApplyPenBrush;
        drawableSprite = GetComponent<SpriteRenderer>().sprite;
        drawableTexture = drawableSprite.texture;

        // // Initialize camera reference
        // mainCamera = Camera.main;
        //
        // if (mainCamera == null)
        // {
        //     Debug.LogError("Main Camera not found. Please ensure there's a camera tagged as 'MainCamera'.");
        // }
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseWorldPos = GetMouseWorldPosition();
            Collider2D hitCollider = GetColliderAtWorldPosition(mouseWorldPos);

            if (hitCollider != null)
            {
                ApplyBrush(mouseWorldPos);
            }
            else
            {
                ResetPreviousDragPosition();
            }
        }
        else
        {
            ResetPreviousDragPosition();
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private Collider2D GetColliderAtWorldPosition(Vector2 worldPosition)
    {
        return Physics2D.OverlapPoint(worldPosition);
    }

    private void ApplyBrush(Vector2 worldPosition)
    {
        CurrentBrush(worldPosition);
    }

    private void ResetPreviousDragPosition()
    {
        previousDragPosition = Vector2.zero;
    }

    public void ApplyPenBrush(Vector2 worldPosition)
    {
        Vector2 pixelPos = ConvertWorldPosition(worldPosition);
        currentColors = drawableTexture.GetPixels32();

        if (previousDragPosition == Vector2.zero)
        {
            DrawOnCanvas(pixelPos, PenWidth, PenColor);
        }
        else
        {
            Drawing(previousDragPosition, pixelPos, PenWidth, PenColor);
        }

        ApplyToTexture();
        previousDragPosition = pixelPos;
    }


    private void Drawing(Vector2 start, Vector2 end, int width, Color colour)
    {
        float distance = Vector2.Distance(start, end);
        for (float progressAlongLine = 0; progressAlongLine <= 1; progressAlongLine += 1 / distance)
        {
            Vector2 currentPoint = Vector2.Lerp(start, end, progressAlongLine);
            DrawOnCanvas(currentPoint, width, colour);
        }
    }

    private void DrawOnCanvas(Vector2 center, int width, Color color)
    {
        int centerDotX = (int)center.x;
        int centerDotY = (int)center.y;

        for (int horizontalDot = centerDotX - width; horizontalDot <= centerDotX + width; horizontalDot++)
        {
            // if (horizontalPixel < 0 || horizontalPixel >= drawableSprite.rect.width) continue; // make it have bounds

            for (int verticalDot = centerDotY - width; verticalDot <= centerDotY + width; verticalDot++)
            {
                DrawColour(horizontalDot, verticalDot, color);
            }
        }
    }

    private void ApplyToTexture()
    {
        drawableTexture.SetPixels32(currentColors);
        drawableTexture.Apply();
    }

    private void DrawColour(int xCoordinate, int yCoordinate, Color color)
    {
        int colourNumber = yCoordinate * (int)drawableSprite.rect.width + xCoordinate;

        if (colourNumber >= 0 && colourNumber < currentColors.Length)
        {
            currentColors[colourNumber] = color;
        }
    }

    private Vector2 ConvertWorldPosition(Vector2 worldPosition)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);
        float unitsToPixels = drawableSprite.rect.width / drawableSprite.bounds.size.x * transform.localScale.x;
        float centeredX = localPos.x * unitsToPixels + drawableSprite.rect.width / 2;
        float centeredY = localPos.y * unitsToPixels + drawableSprite.rect.height / 2;

        return new Vector2(Mathf.RoundToInt(centeredX), Mathf.RoundToInt(centeredY));
    }

    // break

    public void WhiteBackground()
    {
        Color[] whitePixels = new Color[drawableTexture.width * drawableTexture.height];
        System.Array.Fill(whitePixels, Color.white);
        drawableTexture.SetPixels(whitePixels);
        drawableTexture.Apply();
    }

    public void TransparentBackground()
    {
        Color[] transparentPixels = new Color[drawableTexture.width * drawableTexture.height];
        System.Array.Fill(transparentPixels, Color.clear);
        drawableTexture.SetPixels(transparentPixels);
        drawableTexture.Apply();
    }
}