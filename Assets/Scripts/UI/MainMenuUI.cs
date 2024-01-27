using Newtonsoft.Json;
using System;
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

    public string baseUrl = "https://krowce.bieda.it/api";
    string GetSessionsEndpoint => $"{baseUrl}/sessions/?format=json";
    string GetUsersEndpoint => $"{baseUrl}/users/?format=json";
    float timeoutLimit = 5;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        username.text = PlayerPrefs.GetString("username", "Player");

        LoadSessionOptions(sessionSelection);
        LoadUserOptions(teamSelection);
    }
    void LoadUserOptions(TMP_Dropdown dropdown)
    {
        // Parse the response and update the dropdown options
        var users = GetUsers();
        List<string> options = new();
        foreach (var user in users)
        {
            options.Add(user.name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    List<User> GetUsers()
    {
        try
        {
            var result = UnityWebRequest.Get(GetUsersEndpoint).SendWebRequest();
            var time = Time.time;
            while (!result.isDone)
            {
                if (Time.time - time > timeoutLimit) return new();
            }
            var json = JsonConvert.DeserializeObject<List<User>>(result.webRequest.downloadHandler.text);
            return json;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return new();
        }
    }

    void LoadSessionOptions(TMP_Dropdown dropdown)
    {
        // Parse the response and update the dropdown options
        List<Session> sessions = GetSessions();
        List<string> options = new();
        foreach (var session in sessions)
        {
            options.Add(session.name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    List<Session> GetSessions()
    {
        try
        {
            var result = UnityWebRequest.Get(GetSessionsEndpoint).SendWebRequest();
            var time = Time.time;
            while (!result.isDone)
            {
                if (Time.time - time > timeoutLimit) return new();
            }
            var json = JsonConvert.DeserializeObject<SessionCollection>(result.webRequest.downloadHandler.text);
            return json.sessions;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return new();
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("username", teamSelection.options[teamSelection.value].text); // TODO: Change to username
        PlayerPrefs.SetString("team", teamSelection.options[teamSelection.value].text);
        PlayerPrefs.SetString("session", sessionSelection.options[sessionSelection.value].text);
    }
    
    public void StartGame()
    {
        SaveSettings();
        SceneManager.LoadScene(1);
    }
}
