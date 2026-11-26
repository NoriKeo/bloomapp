using System;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class Petal : MonoBehaviour
{
    public string targetColorHex = "#FFFFFF";

    private SpriteRenderer spriteRenderer;
    private Color targetColor;
    private GameManager gameManager;
    private FlowerTarget parentFlower;

    public bool IsCorrectlyColored { get; private set; } = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        parentFlower = GetComponentInParent<FlowerTarget>();

        if (!ColorUtility.TryParseHtmlString(targetColorHex, out targetColor))
        {
            targetColor = Color.white;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log($"Mouse Down on {gameObject.name}");

        if (parentFlower != null && !parentFlower.IsCompleted)
        {
            if (!parentFlower.IsSelected)
            {
                parentFlower.Select();
            }

        }
        if (ColorPaletteManager.SelectedColor != Color.clear)
        {
            spriteRenderer.color = ColorPaletteManager.SelectedColor;

            IsCorrectlyColored = ColorsMatch(spriteRenderer.color, targetColor);
            Debug.Log($"testi {targetColorHex} correctly colored");


            if (parentFlower != null)
            {
                parentFlower.CheckIfFlowerIsComplete();
            }
        }

    }

    private bool ColorsMatch(Color colorA, Color colorB)
    {
        return Mathf.Abs(colorA.r - colorB.r) < 0.05f &&
               Mathf.Abs(colorA.g - colorB.g) < 0.05f &&
               Mathf.Abs(colorA.b - colorB.b) < 0.05f;
    }
}
