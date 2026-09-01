using System;
using Unity.VisualScripting;
using UnityEngine;

public class Peatal : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log($"Mouse Down on {gameObject.name}");
        if (ColorPaletteManager.SelectedColor != Color.clear)
        {
            sr.color = ColorPaletteManager.SelectedColor;
        }

    }
}
