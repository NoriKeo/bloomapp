using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int flowerCountNeededForLevel;
    public Camera mainCamera;
    [FormerlySerializedAs("zoomedIn")] public float zoomedInCameraSize = 2.5f;
    private float defaultCameraSize;
    private Vector3 defaultCameraPosition;

    public GameObject colorPaletteUI;
    public Image targetImage;
    public Sprite flowerTarget;

    private Transform currentFlower;
    private bool isZoomedIn = false;

    public AppData data = new AppData();

    private int currentLevelFlowerCount;

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
        StartCoroutine(ZoomIn(targetPreview));
    }

    private IEnumerator ZoomIn(Sprite targetPreview)
    {
        Vector3 targetPosition = new Vector3(currentFlower.position.x, currentFlower.position.y,
            mainCamera.transform.position.z);
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            mainCamera.transform.position =
                Vector3.Lerp(mainCamera.transform.position, targetPosition, elapsedTime / duration);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomedInCameraSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = zoomedInCameraSize;

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

    //stores data in JSON
    public void OnFlowerCompleted(float completionTime)
    {
        if (data.game.Count > 0)
        {
            data.game[0].flowerCount++;
            data.game[0].totalFlowerCompletionTime += completionTime;
        }

        Debug.Log($"HIIIIII: {data.game[0].flowerCount}");

        if (JsonDataLoader.Instance != null && JsonDataLoader.Instance.appData.game.Count > 0)
        {
            JsonDataLoader.Instance.appData.game[0].flowerCount++;
            JsonDataLoader.Instance.appData.game[0].totalFlowerCompletionTime += completionTime;

            JsonDataLoader.Instance.SaveData();
            Debug.Log($"HIIIIII FLower Count: {JsonDataLoader.Instance.appData.game[0].flowerCount}");
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
        Vector3 startPosition = mainCamera.transform.position;
        float startSize = mainCamera.orthographicSize;
        while (elapsedTime < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, defaultCameraPosition, elapsedTime / duration);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, defaultCameraSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = defaultCameraPosition;
        mainCamera.orthographicSize = defaultCameraSize;

        isZoomedIn = false;
        ++currentLevelFlowerCount;

        if (currentLevelFlowerCount >= flowerCountNeededForLevel)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        MainMenu.LoadNextLevel();
    }
}
