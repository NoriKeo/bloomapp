using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private readonly string[] levelNames = { "Game", "Game 1 ", "Game 2" };

    public void PlayGame()
    {
        if (JsonDataLoader.Instance != null && JsonDataLoader.Instance.appData.game.Count > 0)
        {
            GameData gameData = JsonDataLoader.Instance.appData.game[0];
            gameData.currentLevelIndex = 0;
            JsonDataLoader.Instance.SaveData();

            LoadCurrentLevel(gameData);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public static void LoadNextLevel()
    {
        if (!JsonDataLoader.Instance || JsonDataLoader.Instance.appData.game.Count == 0)
        {
            return;
        }

        GameData gameData = JsonDataLoader.Instance.appData.game[0];
        gameData.currentLevelIndex++;

        if (gameData.currentLevelIndex < gameData.meadows.Length)
        {
            JsonDataLoader.Instance.SaveData();
            LoadCurrentLevel(gameData);
        }
        else
        {
            Debug.Log("Levels finished");
            gameData.currentLevelIndex = 0;
            JsonDataLoader.Instance.SaveData();
            SceneManager.LoadScene(0);
        }
    }

    private static void LoadCurrentLevel(GameData gameData)
    {
        Debug.Log(gameData.currentLevelIndex);

        int targetMeadowIndex = gameData.meadows[gameData.currentLevelIndex];

        Debug.Log($"Loading level at index {targetMeadowIndex}");
        SceneManager.LoadScene(targetMeadowIndex);
    }

    public void ReturnToStartScreen()
    {
        SceneManager.LoadScene(0);
    }
}
