using System;
using UnityEngine;

public class FlowerTarget : MonoBehaviour
{
    [SerializeField] private Transform zoomPos;
    public Sprite targetPreview;
    private Game gameManager;
    private float _completionTimer;

    public bool _isSelected = false;
    public bool IsCompleted { get; private set; } = false;

    //reports that a flower has been selected
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

    private void Update()
    {
        if (IsSelected && !IsCompleted)
        {
            _completionTimer += Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        TriggerSelected();
    }
   
    //This is where we check whether the flower has been fully coloured in 
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
            gameManager.OnFlowerCompleted(_completionTimer);
        }
    }

    public void TriggerSelected()
    {
        if (gameManager != null && !IsSelected && !IsCompleted)
        {
            Debug.Log($"{gameObject.name} at {zoomPos.position} was selected");
            gameManager.SelectFlower(zoomPos, targetPreview);
            IsSelected = true;
        }
    }
}