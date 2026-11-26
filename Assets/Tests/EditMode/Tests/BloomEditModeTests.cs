using System.IO;
using NUnit.Framework;
using UnityEngine;

public class BloomEditModeTests
{
    private string tempJsonPath;

    [SetUp]
    public void SetUp()
    {
        tempJsonPath = Path.Combine(Path.GetTempPath(), $"test_appData_{System.Guid.NewGuid()}.json");
        ColorPaletteManager.SelectedColor = Color.clear;
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(tempJsonPath))
        {
            File.Delete(tempJsonPath);
        }
    }

    [Test]
    public void GameData_DefaultValues_AreInitializedAsExpected()
    {
        var gameData = new GameData();

        Assert.AreEqual(0, gameData.flowerCount);
        Assert.AreEqual(0f, gameData.totalFlowerCompletionTime);
        Assert.AreEqual(new int[] { 1, 3, 4 }, gameData.meadows);
        Assert.AreEqual(1, gameData.currentLevelIndex);
    }

    [Test]
    public void ColorPaletteManager_HexToColor_SetsExpectedColor()
    {
        var go = new GameObject("Palette");
        var palette = go.AddComponent<ColorPaletteManager>();

        palette.SetColorKorallePink();
        ColorUtility.TryParseHtmlString("#FA6982", out Color expected);

        Assert.AreEqual(expected.r, ColorPaletteManager.SelectedColor.r, 0.001f);
        Assert.AreEqual(expected.g, ColorPaletteManager.SelectedColor.g, 0.001f);
        Assert.AreEqual(expected.b, ColorPaletteManager.SelectedColor.b, 0.001f);

        Object.DestroyImmediate(go);
    }

    [Test]
    public void AppData_JsonSerializationRoundTrip_PreservesUserAndGameData()
    {
        var appData = new AppData();
        appData.users.Add(new UserData { username = "Namie", password = "123" });
        appData.game.Add(new GameData { flowerCount = 5, totalFlowerCompletionTime = 12.5f });

        string json = JsonUtility.ToJson(appData, true);
        File.WriteAllText(tempJsonPath, json);

        string loadedJson = File.ReadAllText(tempJsonPath);
        var loaded = JsonUtility.FromJson<AppData>(loadedJson);

        Assert.AreEqual(1, loaded.users.Count);
        Assert.AreEqual("Namie", loaded.users[0].username);
        Assert.AreEqual("123", loaded.users[0].password);
        Assert.AreEqual(1, loaded.game.Count);
        Assert.AreEqual(5, loaded.game[0].flowerCount);
        Assert.AreEqual(12.5f, loaded.game[0].totalFlowerCompletionTime, 0.001f);
    }
}
