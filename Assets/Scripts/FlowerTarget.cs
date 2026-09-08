using System;
using UnityEngine;

public class FlowerTarget : MonoBehaviour
{
    [SerializeField] private Transform zoomPos;
    public Sprite targetPreview;
    private Game gameManager;

    public bool _isSelected = false;
    public bool IsCompleted { get; private set; } = false;

    public bool IsSelected
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
        if (gameManager != null && !IsSelected && !IsCompleted)
        {
            gameManager.SelectFlower(zoomPos, targetPreview);
            IsSelected = true;
        }
    }

    public void CheckIfFlowerIsComplete()
    {
        Debug.Log($"jippppi");

        if (IsCompleted)
        {
            return;
        }
        Peatal[] allPeatals = GetComponentsInChildren<Peatal>();
        foreach (Peatal p in allPeatals)
        {
            if (!p.IsCorrectlyColored)
            {
                return;
            }
        }
        IsCompleted = true;

        if (gameManager != null)
        {
            gameManager.OnFlowerCompleted();
        }
    }

    public void TriggerSelected()
    {
        if (gameManager != null && !IsSelected && !IsCompleted)
        {
            IsSelected = true;
            gameManager.SelectFlower(transform, targetPreview);
            
        }
    }
}