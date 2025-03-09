using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintThings : MonoBehaviour, IPaintable
{
    public enum PaintMode
    {
        Mode2D,
        Mode3D
    }

    public PaintMode CurrentMode;

    public static Color PenColor = Color.red;
    public static int PenWidth = 3;

    public delegate void BrushFunction(Vector2 worldPosition);

    public BrushFunction CurrentBrush;

    public LayerMask DrawingLayers;
    public Color ResetColor = new Color(0, 0, 0, 0);

    private static PaintThings instance;
    private Sprite drawableSprite;
    private Texture2D drawableTexture;

    private Vector2 previousDragPosition;
    private Color[] cleanColorsArray;
    private Color32[] currentColors;
    private bool wasMouseHeldDown = false;
    private bool noDrawingOnCurrentDrag = false;

    void Update()
    {
        bool mouseHeldDown = Input.GetMouseButton(0);
        if (mouseHeldDown && !noDrawingOnCurrentDrag)
        {
            Vector2? worldPos = GetPaintPosition();

            if (worldPos.HasValue)
            {
                CurrentBrush(worldPos.Value);
            }
            else
            {
                previousDragPosition = Vector2.zero;
                if (!wasMouseHeldDown) noDrawingOnCurrentDrag = true;
            }
        }
        else if (!mouseHeldDown)
        {
            previousDragPosition = Vector2.zero;
            noDrawingOnCurrentDrag = false;
        }

        wasMouseHeldDown = mouseHeldDown;
    }

    // select which mode
    private Vector2? GetPaintPosition()
    {
        if (CurrentMode == PaintMode.Mode2D)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, DrawingLayers);
            if (hit != null) return mouseWorldPos;
        }
        else if (CurrentMode == PaintMode.Mode3D)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, DrawingLayers))
            {
                return hit.point;
            }
        }

        return null;
    }
    // brush to draw
    public void PenBrush(Vector2 worldPoint)
    {
        Vector2 pixelPos = WorldToPixelCoordinates(worldPoint);
        currentColors = drawableTexture.GetPixels32();

        if (previousDragPosition == Vector2.zero)
        {
            MarkPixelsToColor(pixelPos, PenWidth, PenColor);
        }
        else
        {
            DrawLine(previousDragPosition, pixelPos, PenWidth, PenColor);
        }

        ApplyPixelChanges();
        previousDragPosition = pixelPos;
    }
    // paint math
    public void DrawLine(Vector2 startPoint, Vector2 endPoint, int width, Color color)
    {
        float distance = Vector2.Distance(startPoint, endPoint);
        Vector2 direction = (startPoint - endPoint).normalized;
        Vector2 currentPosition = startPoint;
        float stepSize = 1 / distance;

        for (float t = 0; t <= 1; t += stepSize)
        {
            currentPosition = Vector2.Lerp(startPoint, endPoint, t);
            MarkPixelsToColor(currentPosition, width, color);
        }
    }

    public void MarkPixelsToColor(Vector2 centerPixel, int penThickness, Color color)
    {
        int centerX = (int)centerPixel.x;
        int centerY = (int)centerPixel.y;

        for (int x = centerX - penThickness; x <= centerX + penThickness; x++)
        {
            if (x < 0 || x >= drawableSprite.rect.width) continue;

            for (int y = centerY - penThickness; y <= centerY + penThickness; y++)
            {
                MarkPixel(x, y, color);
            }
        }
    }

    public void MarkPixel(int x, int y, Color color)
    {
        int arrayPos = y * (int)drawableSprite.rect.width + x;
        if (arrayPos < 0 || arrayPos >= currentColors.Length) return;

        currentColors[arrayPos] = color;
    }

    public void ApplyPixelChanges()
    {
        drawableTexture.SetPixels32(currentColors);
        drawableTexture.Apply();
    }

    public Vector2 WorldToPixelCoordinates(Vector2 worldPosition)
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPosition);
        float unitsToPixels = drawableSprite.rect.width / drawableSprite.bounds.size.x * transform.localScale.x;
        float centeredX = localPos.x * unitsToPixels + drawableSprite.rect.width / 2;
        float centeredY = localPos.y * unitsToPixels + drawableSprite.rect.height / 2;

        return new Vector2(Mathf.RoundToInt(centeredX), Mathf.RoundToInt(centeredY));
    }

    void Awake()
    {
        instance = this;
        CurrentBrush = PenBrush;

        drawableSprite = GetComponent<SpriteRenderer>().sprite;
        drawableTexture = drawableSprite.texture;

        cleanColorsArray = new Color[(int)drawableSprite.rect.width * (int)drawableSprite.rect.height];
        for (int i = 0; i < cleanColorsArray.Length; i++)
            cleanColorsArray[i] = ResetColor;
    }
    // ereaser
    public void EraserBrush(Vector2 worldPoint)
    {
        Vector2 pixelPos = WorldToPixelCoordinates(worldPoint);
        currentColors = drawableTexture.GetPixels32();

        if (previousDragPosition == Vector2.zero)
        {
            MarkPixelsToColor(pixelPos, PenWidth, ResetColor);
        }
        else
        {
            DrawLine(previousDragPosition, pixelPos, PenWidth, ResetColor);
        }

        ApplyPixelChanges();
        previousDragPosition = pixelPos;
    }
    
    // background stuff
    public void WhiteBackground()
    {
        if (drawableTexture == null)
        {
            return;
        }

        Color[] whitePixels = new Color[drawableTexture.width * drawableTexture.height];

        for (int i = 0; i < whitePixels.Length; i++)
        {
            whitePixels[i] = Color.white;
        }

        drawableTexture.SetPixels(whitePixels);
        drawableTexture.Apply();
    }

    public void TransparentBackground()
    {
        if (drawableTexture == null)
        {
            return;
        }

        Color[] transparentPixels = new Color[drawableTexture.width * drawableTexture.height];

        for (int i = 0; i < transparentPixels.Length; i++)
        {
            transparentPixels[i] = new Color(0, 0, 0, 0);
        }

        drawableTexture.SetPixels(transparentPixels);
        drawableTexture.Apply();
    }
}