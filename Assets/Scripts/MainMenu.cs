using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private readonly string[] levelNames = { "Game", "Game 1 ", "Game 2" };

    public void PlayGame()
    {
        if (JsonDataLoader.instance != null && JsonDataLoader.instance.appData.game.Count > 0)
        {
            GameData gameData = JsonDataLoader.instance.appData.game[0];
            gameData.currentLevelIndex = 0;
            JsonDataLoader.instance.SaveData();

            LoadCurrentLevel(gameData);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public static void LoadNextLevel()
    {
        if (!JsonDataLoader.instance || JsonDataLoader.instance.appData.game.Count == 0)
        {
            return;
        }

        GameData gameData = JsonDataLoader.instance.appData.game[0];
        gameData.currentLevelIndex++;

        if (gameData.currentLevelIndex < gameData.meadows.Length)
        {
            JsonDataLoader.instance.SaveData();
            LoadCurrentLevel(gameData);
        }
        else
        {
            Debug.Log("Levels finished");
            gameData.currentLevelIndex = 0;
            JsonDataLoader.instance.SaveData();
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

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}