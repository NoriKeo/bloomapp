using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoginManager : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public Button loginButton;
    public GameObject loginPanel;
    public TMP_Text feedbackText;

    public int maxAttemptsBeforeCooldown = 3;
    public float cooldownTime = 120f;

    private int attempts = 0;
    private bool isLocked = false;

    private void Start()
    {
        if (password != null)
        {
            password.contentType = TMP_InputField.ContentType.Password;
            password.ForceLabelUpdate();
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    public void LoginButtonClicked()
    {
       if (isLocked) return;

       string usernames = username != null ? username.text.Trim() : "";
       string passwords = password != null ? password.text : "";

       if (JsonDataLoader.instance != null && JsonDataLoader.instance.ValidateLogin(usernames, passwords))
       {
           attempts = 0;
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
        attempts++;

        if (loginPanel != null)
        {
            loginPanel.SetActive(true);
        }

        if (attempts >= maxAttemptsBeforeCooldown)
        {
            StartCoroutine(CooldownRoutine());
        }
        else if (feedbackText != null)
        {
            int remaining = maxAttemptsBeforeCooldown - attempts;
            feedbackText.text = $"{message} ({remaining} Attempts leading up to the ban)";
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isLocked = true;
        if (loginButton != null) loginButton.interactable = false;

        float timer = cooldownTime;
        while (timer > 0f)
        {
            if (feedbackText != null)
            {
                feedbackText.text = $"Too many attempts. Please wait {Mathf.CeilToInt(timer)}m before trying again.";
            }
            timer -= Time.deltaTime;
            yield return null;
        }

        attempts = 0;
        isLocked = false;
        if (loginButton != null) loginButton.interactable = true;
        if (feedbackText != null) feedbackText.text = "";
    }

}
