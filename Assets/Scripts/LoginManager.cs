using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public GameObject loginPanel;

    public void LoginButtonClicked()
    {
        string usernames = username.text.Trim();
        string passwords = password.text.Trim();

        if (JsonDataLoader.instance.ValidateLogin(usernames, passwords))
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            if (loginPanel != null)
            {
                loginPanel.SetActive(true);
            }
        }
        
    }
    
}
