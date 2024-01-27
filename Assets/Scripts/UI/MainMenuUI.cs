using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField username;
    public TMP_Dropdown teamSelection;
    public TMP_Dropdown sessionSelection;

    void Start()
    {
        username.text = PlayerPrefs.GetString("username", "Player");

        // Load dropdown options asynchronously
        StartCoroutine(LoadDropdownOptions("https://your-api-url/team_options", teamSelection));
        StartCoroutine(LoadDropdownOptions("https://your-api-url/session_options", sessionSelection));
    }

    // Coroutine to load dropdown options asynchronously
    IEnumerator LoadDropdownOptions(string url, TMP_Dropdown dropdown)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading dropdown options: " + www.error);
            }
            else
            {
                // Parse the response and update the dropdown options
                string[] options = www.downloadHandler.text.Split('\n');
                dropdown.ClearOptions();
                dropdown.AddOptions(new List<string>(options));
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
