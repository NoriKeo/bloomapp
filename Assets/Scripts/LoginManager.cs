using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class LoginManager : MonoBehaviour
{
    [FormerlySerializedAs("username")] public TMP_InputField usernameInput;
    [FormerlySerializedAs("password")] public TMP_InputField passwordInput;
    public Button loginButton;
    public GameObject loginPanel;
    public TMP_Text feedbackText;

    public int maxAttemptsBeforeCooldown = 3;
    public float cooldownTime = 120f;

    private int failedAttempts = 0;
    private bool isLocked = false;

    private void Start()
    {
        if (passwordInput != null)
        {
            passwordInput.contentType = TMP_InputField.ContentType.Password;
            passwordInput.ForceLabelUpdate();
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    public void OnLoginButtonClicked()
    {
       if (isLocked) return;

       string enteredUsername = usernameInput != null ? usernameInput.text.Trim() : "";
       string enteredPassword = passwordInput != null ? passwordInput.text : "";

       if (JsonDataLoader.Instance != null && JsonDataLoader.Instance.ValidateLogin(enteredUsername, enteredPassword))
       {
           failedAttempts = 0;
           if (feedbackText != null) feedbackText.text = "";
           SceneManager.LoadScene(2);
       }
       else
       {
           HandleFailedAttempt("Login failed.");
       }

    }

    private void HandleFailedAttempt(string message)
    {
        failedAttempts++;

        if (loginPanel != null)
        {
            loginPanel.SetActive(true);
        }

        if (failedAttempts >= maxAttemptsBeforeCooldown)
        {
            StartCoroutine(CooldownRoutine());
        }
        else if (feedbackText != null)
        {
            int remaining = maxAttemptsBeforeCooldown - failedAttempts;
            feedbackText.text = $"{message} ({remaining} Attempts leading up to the ban)";
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isLocked = true;
        if (loginButton != null) loginButton.interactable = false;

        float remainingTime = cooldownTime;
        while (remainingTime > 0f)
        {
            if (feedbackText != null)
            {
                feedbackText.text = $"Too many attempts. Please wait {Mathf.CeilToInt(remainingTime)}m before trying again.";
            }
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        failedAttempts = 0;
        isLocked = false;
        if (loginButton != null) loginButton.interactable = true;
        if (feedbackText != null) feedbackText.text = "";
    }

}
