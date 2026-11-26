using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DashboardUI : MonoBehaviour
{
    public TMP_Text flowerCountText;
    [FormerlySerializedAs("reactionTimeText")] public TMP_Text averageCompletionTimeText;
    public TMP_Text bugExperimentsText;
    public TMP_Text bugTrappedText;
    public TMP_Text bugEscapeText;

    public TMP_Dropdown levelDropdown;

    private void Start()
    {
        UpdateDashboard();

        if (levelDropdown != null)
        {
            levelDropdown.onValueChanged.AddListener(OnLevelChanged);
        }
    }

    private void OnEnable()
    {
        UpdateDashboard();
    }


//Update Mia’s details when new data has been recorded
    private void UpdateDashboard()
    {
        if (JsonDataLoader.Instance == null)
        {
            Debug.LogWarning("Jason Data Loader is missing");
            return;
        }

        AppData currentData = JsonDataLoader.Instance != null ? JsonDataLoader.Instance.appData : null;
        if (currentData == null)
        {
            Debug.Log("No data loaded");
            return;
        }

        if (currentData.game != null && currentData.game.Count > 0)
        {
            Debug.Log($"Current game: {currentData.game.Count}");
            if (flowerCountText != null)
            {
                flowerCountText.text = $"Final flowers: {currentData.game[0].flowerCount}";
            }

            if (averageCompletionTimeText != null)
            {
                averageCompletionTimeText.text =
                    $"Average flower completion time: {FormatAverageCompletionTime(currentData.game[0].totalFlowerCompletionTime, currentData.game[0].flowerCount)}";
            }
        }

        if (currentData.mias != null && currentData.mias.Count > 0)
        {
            Debug.Log($"Current mias: {currentData.mias.Count}");
            MiasData mia = currentData.mias[0];


            if (bugExperimentsText != null)
            {
                bugExperimentsText.text = $"Bug experiments: {mia.bugExperiments}";
            }

            if (bugTrappedText != null)
            {
                bugTrappedText.text = $"Trapped: {mia.bugTrapped}";
            }

            if (bugEscapeText != null)
            {
                bugEscapeText.text = $"Escape: {mia.bugEscape}";
            }
        }
    }

    //Choose from various level layouts
    public void OnLevelChanged(int optionIndex)
    {
        if (JsonDataLoader.Instance || JsonDataLoader.Instance.appData.game.Count > 0)
        {
            GameData data = JsonDataLoader.Instance.appData.game[0];

            switch (optionIndex)
            {
                case 0:
                    data.meadows = new[] { 1, 3, 4 };
                    break;
                case 1:
                    data.meadows = new[] { 4, 3, 1 };
                    break;
                case 2:
                    data.meadows = new[] { 3, 4, 1 };
                    break;
            }

            data.currentLevelIndex = 0;
            Debug.Log($"Changed to {data.meadows}");
            JsonDataLoader.Instance.SaveData();
            Debug.Log("new Lavel stuff");
        }
    }

    //calculates the average response time
    private string FormatAverageCompletionTime(float totalTime, int flowerCount)
    {
        var average = totalTime / flowerCount;
        TimeSpan timeSpan = TimeSpan.FromSeconds(average);

        return timeSpan.ToString("m\\:ss\\.fff");
    }
}
