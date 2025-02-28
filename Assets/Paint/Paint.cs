using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Paint : MonoBehaviour
    {
        // Static properties
        public static Color PenColor = Color.red;
        public static int PenWidth = 3;

        // Delegate for brush functions
        public delegate void BrushFunction(Vector2 worldPosition);

        public BrushFunction CurrentBrush;

        // Drawing settings
        public LayerMask DrawingLayers;
        public bool ResetCanvasOnPlay = true;
        public Color ResetColor = new Color(0, 0, 0, 0);
        public bool ResetToTextureOnPlay = false;
        public Texture2D ResetTexture;

        // Instance and sprite-related properties
        private static Paint instance;
        private Sprite drawableSprite;
        private Texture2D drawableTexture;

        // Variables for tracking drawing state
        private Vector2 previousDragPosition;
        private Color[] cleanColorsArray;
        private Color32[] currentColors;
        private bool wasMouseHeldDown = false;
        private bool noDrawingOnCurrentDrag = false;

        // Drawing brushes
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



        void Update()
        {
            bool mouseHeldDown = Input.GetMouseButton(0);
            if (mouseHeldDown && !noDrawingOnCurrentDrag)
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos, DrawingLayers.value);
                if (hit != null && hit.transform != null)
                {
                    CurrentBrush(mouseWorldPos);
                }
                else
                {
                    previousDragPosition = Vector2.zero;
                    if (!wasMouseHeldDown)
                    {
                        noDrawingOnCurrentDrag = true;
                    }
                }
            }
            else if (!mouseHeldDown)
            {
                previousDragPosition = Vector2.zero;
                noDrawingOnCurrentDrag = false;
            }

            wasMouseHeldDown = mouseHeldDown;
        }

        // Draw a line between two points
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

        // Mark a region of pixels to a specified color
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

        // Mark a single pixel to a specified color
        public void MarkPixel(int x, int y, Color color)
        {
            int arrayPos = y * (int)drawableSprite.rect.width + x;
            if (arrayPos < 0 || arrayPos >= currentColors.Length) return;

            currentColors[arrayPos] = color;
        }

        // Apply all marked pixel changes
        public void ApplyPixelChanges()
        {
            drawableTexture.SetPixels32(currentColors);
            drawableTexture.Apply();
        }

        // Convert world coordinates to pixel coordinates
        public Vector2 WorldToPixelCoordinates(Vector2 worldPosition)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPosition);
            float unitsToPixels = drawableSprite.rect.width / drawableSprite.bounds.size.x * transform.localScale.x;
            float centeredX = localPos.x * unitsToPixels + drawableSprite.rect.width / 2;
            float centeredY = localPos.y * unitsToPixels + drawableSprite.rect.height / 2;

            return new Vector2(Mathf.RoundToInt(centeredX), Mathf.RoundToInt(centeredY));
        }

        // Reset the canvas to its initial state
        public void ResetCanvas()
        {
            drawableTexture.SetPixels(cleanColorsArray);
            drawableTexture.Apply();
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

            if (ResetCanvasOnPlay)
                ResetCanvas();
            else if (ResetToTextureOnPlay)
            {
                Graphics.CopyTexture(ResetTexture, drawableTexture);
                Debug.Log("Reset texture");
            }
        }
        
        
        
    }
    
    
    
    
    
    
