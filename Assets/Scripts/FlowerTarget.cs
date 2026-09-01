using System;
using UnityEngine;

public class FlowerTarget : MonoBehaviour
{
    public Sprite targetPreview;
    private Game gameManager;

    private bool _isSelected = false;

    private bool IsSelected
    {
        get => _isSelected;
        set
        {
            _collider2D.enabled = !value;
            _isSelected = value;
        }
    }

    private Collider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<Game>();
        IsSelected = false;
    }

    private void OnMouseDown()
    {
        if (gameManager != null && !IsSelected)
        {
            gameManager.SelectFlower(transform, targetPreview);
            IsSelected = true;
        }
    }
}