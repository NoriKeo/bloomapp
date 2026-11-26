using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.EventSystems;

public class FlowerTarget : MonoBehaviour
{
    [FormerlySerializedAs("zoomPos")]
    [SerializeField] private Transform zoomPosition;
    public Sprite targetPreview;
    private GameManager gameManager;
    private float completionTimer;

    public bool _isSelected = false;
    public bool IsCompleted { get; private set; } = false;

    //reports that a flower has been selected
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            flowerCollider.enabled = !value;
            _isSelected = value;
        }
    }

    private Collider2D flowerCollider;


    private void Awake()
    {
        flowerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        IsSelected = false;
    }

    private void Update()
    {
        if (IsSelected && !IsCompleted)
        {
            completionTimer += Time.deltaTime;
        }

    }

    private void OnMouseDown()
    {
        Select();
    }

    //This is where we check whether the flower has been fully coloured in
    public void CheckIfFlowerIsComplete()
    {
        Debug.Log($"jippppi");

        if (IsCompleted)
        {
            return;
        }

        Petal[] allPetals = GetComponentsInChildren<Petal>();
        foreach (Petal p in allPetals)
        {
            if (!p.IsCorrectlyColored)
            {
                return;
            }
        }

        IsCompleted = true;


        if (gameManager != null)
        {
            gameManager.OnFlowerCompleted(completionTimer);
        }
    }

    public void Select()
    {
        if (gameManager != null && !IsSelected && !IsCompleted)
        {
            Debug.Log($"{gameObject.name} at {zoomPosition.position} was selected");
            gameManager.SelectFlower(zoomPosition, targetPreview);
            IsSelected = true;
        }
    }
}
