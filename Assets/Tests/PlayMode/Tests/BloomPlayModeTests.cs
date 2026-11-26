using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class BloomPlayModeTests
{
    private GameObject loaderGo;
    private JsonDataLoader loader;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        loaderGo = new GameObject("JsonDataLoader");
        loader = loaderGo.AddComponent<JsonDataLoader>();
        loader.appData = new AppData();
        loader.appData.users.Add(new UserData { username = "Namie", password = "123" });
        loader.appData.game.Add(new GameData { flowerCount = 2, totalFlowerCompletionTime = 10f });
        loader.appData.mias.Add(new MiasData { bugExperiments = 3, bugTrapped = 1, bugEscape = 2 });
        JsonDataLoader.instance = loader;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (loaderGo != null)
        {
            Object.Destroy(loaderGo);
        }
        ColorPaletteManager.SelectedColor = Color.clear;
        yield return null;
    }

    [UnityTest]
    public IEnumerator LoginManager_ThreeFailedAttempts_ActivatesCooldownAndDisablesButton()
    {
        var go = new GameObject("Login");
        var login = go.AddComponent<LoginManager>();

        login.username = CreateInputField(go, "user");
        login.password = CreateInputField(go, "pass");
        login.loginButton = go.AddComponent<Button>();
        login.feedbackText = go.AddComponent<TextMeshProUGUI>();
        login.maxAttemptsBeforeCooldown = 3;
        login.cooldownTime = 5f;

        login.username.text = "Namie";
        login.password.text = "falsch";

        yield return null;

        login.LoginButtonClicked();
        login.LoginButtonClicked();
        login.LoginButtonClicked();

        yield return null;

        Assert.IsFalse(login.loginButton.interactable);
        Assert.IsTrue(login.feedbackText.text.Contains("Too many attempts"));

        Object.Destroy(go);
    }

    [UnityTest]
    public IEnumerator DashboardUI_UpdatesTextsFromJsonDataLoader()
    {
        var go = new GameObject("Dashboard");
        var dashboard = go.AddComponent<DashboardUI>();

        dashboard.flowerCountText = CreateTmpText(go, "Flower");
        dashboard.reactionTimeText = CreateTmpText(go, "Reaction");
        dashboard.bugExperimentsText = CreateTmpText(go, "Exp");
        dashboard.bugTrappedText = CreateTmpText(go, "Trapped");
        dashboard.bugEscapeText = CreateTmpText(go, "Escape");

        yield return null;

        Assert.AreEqual("Final flowers: 2", dashboard.flowerCountText.text);
        Assert.AreEqual("Bug experiments: 3", dashboard.bugExperimentsText.text);
        Assert.AreEqual("Trapped: 1", dashboard.bugTrappedText.text);
        Assert.AreEqual("Escape: 2", dashboard.bugEscapeText.text);

        Object.Destroy(go);
    }

    [UnityTest]
    public IEnumerator FlowerAndPeatal_WhenColorMatches_CompletesFlowerAndUpdatesGameData()
    {
        var camGo = new GameObject("MainCamera");
        camGo.tag = "MainCamera";
        var cam = camGo.AddComponent<Camera>();
        cam.orthographic = true;

        var gameGo = new GameObject("GameManager");
        var game = gameGo.AddComponent<Game>();
        game.mainCamera = cam;

        var flowerGo = new GameObject("Flower");
        flowerGo.AddComponent<BoxCollider2D>();
        var zoomGo = new GameObject("ZoomPos");
        zoomGo.transform.SetParent(flowerGo.transform);

        var flower = flowerGo.AddComponent<FlowerTarget>();
        SetPrivateField(flower, "zoomPos", zoomGo.transform);

        var petalGo = new GameObject("Petal_1");
        petalGo.transform.SetParent(flowerGo.transform);
        petalGo.AddComponent<SpriteRenderer>();
        var petal = petalGo.AddComponent<Peatal>();
        petal.targetColorHex = "#FF0000";

        yield return null;

        ColorUtility.TryParseHtmlString("#FF0000", out Color red);
        ColorPaletteManager.SelectedColor = red;

        // Simuliert das Ausmalen und Prüfen über Reflection/Aufruf wie bei OnMouseDown
        petalGo.GetComponent<SpriteRenderer>().color = ColorPaletteManager.SelectedColor;
        var matchMethod = typeof(Peatal).GetMethod("ColorsMatch", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        bool matched = (bool)matchMethod.Invoke(petal, new object[] { red, red });
        typeof(Peatal).GetProperty("IsCorrectlyColored").SetValue(petal, matched);
        flower.CheckIfFlowerIsComplete();

        yield return null;

        Assert.IsTrue(flower.IsCompleted);
        Assert.AreEqual(3, JsonDataLoader.instance.appData.game[0].flowerCount);

        Object.Destroy(flowerGo);
        Object.Destroy(gameGo);
        Object.Destroy(camGo);
    }

    private TMP_InputField CreateInputField(GameObject parent, string name)
    {
        var obj = new GameObject(name);
        obj.transform.SetParent(parent.transform);
        var image = obj.AddComponent<Image>();
        var textObj = new GameObject("Text");
        textObj.transform.SetParent(obj.transform);
        var tmp = textObj.AddComponent<TextMeshProUGUI>();
        var field = obj.AddComponent<TMP_InputField>();
        field.targetGraphic = image;
        field.textComponent = tmp;
        return field;
    }

    private TMP_Text CreateTmpText(GameObject parent, string name)
    {
        var obj = new GameObject(name);
        obj.transform.SetParent(parent.transform);
        return obj.AddComponent<TextMeshProUGUI>();
    }

    private void SetPrivateField(object target, string fieldName, object value)
    {
        var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(target, value);
    }
}
