using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private readonly string[] levelNames = {"Game", "Game 1 ", "Game 2"};
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        /*if (JasonDataLoader.instance != null && JasonDataLoader.instance.appData.game.Count > 0)
        {
            GameData gameData = JasonDataLoader.instance.appData.game[0];
            gameData.currentLevel = 1;
            JasonDataLoader.instance.SaveData();
            
            LoadCurrentLevel(gameData);
        }
        else
        {
            SceneManager.LoadScene(1);
        }*/
    }

    public static void LoadNextLevel()
    {
        if (JasonDataLoader.instance == null || JasonDataLoader.instance.appData.game.Count == 0)
        {
            return;
        }
        GameData gameData = JasonDataLoader.instance.appData.game[0];
        gameData.currentLevel++;

        if (gameData.currentLevel < gameData.meadows.Length)
        {
            JasonDataLoader.instance.SaveData();
            LoadCurrentLevel(gameData);
        }
        else
        {
            Debug.Log("Levels finished");
            gameData.currentLevel = 1;
            JasonDataLoader.instance.SaveData();
            SceneManager.LoadScene(0);
        }
        
    }

    private static void LoadCurrentLevel(GameData gameData)
    {
        string[] sceneName = new string[] { "Game", "Game 1", "Game 2" };

        Debug.Log(gameData.currentLevel);
        
        int tagetMeadowIndex = gameData.meadows[gameData.currentLevel];

        if (tagetMeadowIndex >= 0 && tagetMeadowIndex < sceneName.Length)
        {
            string nextScene = sceneName[tagetMeadowIndex];
            Debug.Log($"Loading level {nextScene}");
            SceneManager.LoadScene(nextScene);
            /*
            MainMenu.LoadNextLevel();
        */
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }

}
