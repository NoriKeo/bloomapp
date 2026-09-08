using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private int flowerCountNeededForLevel;
    public Camera mainCamera;
    public float zoomedIn = 2.5f;
    private float defaultCameraSize;
    private Vector3 defaultCameraPosition;

    public GameObject colorPaletteUI;
    public Image targetImage;
    public Sprite flowerTarget;

    private Transform currentFlower;
    private bool isZoomedIn = false;

    public AppData data = new AppData();

    private int _currentLevelFlowerCount;

    private void Start()
    {
        defaultCameraSize = mainCamera.orthographicSize;
        defaultCameraPosition = mainCamera.transform.position;

        if (colorPaletteUI)
        {
            colorPaletteUI.SetActive(false);
        }

        if (targetImage)
        {
            targetImage.gameObject.SetActive(false);
        }

        if (data.game.Count == 0)
        {
            data.game.Add(new GameData());
        }
    }

    public void SelectFlower(Transform flower, Sprite targetPreview)
    {
        if (isZoomedIn) return;
        currentFlower = flower;
        StartCoroutine(Zoom(targetPreview));
    }

    private IEnumerator Zoom(Sprite targetPreview)
    {
        Vector3 targetPos = new Vector3(currentFlower.position.x, currentFlower.position.y,
            mainCamera.transform.position.z);
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position =
                Vector3.Lerp(mainCamera.transform.position, targetPos, elapsedTime / duration);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomedIn, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPos;
        mainCamera.orthographicSize = zoomedIn;

        if (targetImage != null && targetPreview != null)
        {
            targetImage.sprite = targetPreview;
            targetImage.gameObject.SetActive(true);

            yield return new WaitForSeconds(2.0f);
            Debug.Log($"TargetImage wirklich sichtbar im Canvas: {targetImage.gameObject.activeInHierarchy}");
            Debug.Log($"TargetImage aktiv: {targetImage.gameObject.activeSelf}");
            targetImage.gameObject.SetActive(false);
        }


        if (colorPaletteUI)
        {
            colorPaletteUI.SetActive(true);
            isZoomedIn = true;
        }
    }

    public void OnFlowerCompleted()
    {
        if (data.game.Count > 0)
        {
            data.game[0].flowerCount++;
        }

        Debug.Log($"HIIIIII: {data.game[0].flowerCount}");

        if (JasonDataLoader.instance != null && JasonDataLoader.instance.appData.game.Count > 0)
        {
            JasonDataLoader.instance.appData.game[0].flowerCount++;
            JasonDataLoader.instance.SaveData();
            Debug.Log($"HIIIIII FLower Count: {JasonDataLoader.instance.appData.game[0].flowerCount}");
        }


        StartCoroutine(ZoomOut());
    }

    private IEnumerator ZoomOut()
    {
        yield return new WaitForSeconds(0.8f);

        if (colorPaletteUI)
        {
            colorPaletteUI.SetActive(false);
        }

        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector3 startPos = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPos, defaultCameraPosition, elapsedTime / duration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, defaultCameraSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = defaultCameraPosition;
        mainCamera.orthographicSize = defaultCameraSize;

        isZoomedIn = false;
        ++_currentLevelFlowerCount;

        if (_currentLevelFlowerCount >= flowerCountNeededForLevel)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        MainMenu.LoadNextLevel();
    }
}