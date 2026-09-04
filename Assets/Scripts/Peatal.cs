using System;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class Peatal : MonoBehaviour
{
    public string targetColorHex = "#FFFFFF";
    
    private SpriteRenderer sr;
    private Color targetColor;
    private Game gameManager;
    private FlowerTarget parentFlower;
    
    public bool IsCorrectlyColored { get; private set; } = false;
    

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<Game>();
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
                parentFlower.TriggerSelected();
            }
            
        }
        if (ColorPaletteManager.SelectedColor != Color.clear)
        {
            sr.color = ColorPaletteManager.SelectedColor;
            
            IsCorrectlyColored = ColorsMatch(sr.color, targetColor);
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
