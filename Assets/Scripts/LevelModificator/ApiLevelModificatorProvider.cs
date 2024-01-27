using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using UnityEngine.Networking;

[CreateAssetMenu]
public class ApiLevelModificatorProvider : BaseLevelModificator
{
    public string baseUrl = "https://krowce.bieda.it/api";

    string GetMapElementsEndpoint => $"{baseUrl}/test";
    string PostMapElementsEndpoint => $"{baseUrl}/test";

    float timeoutLimit = 5;

    string GetSessionsEndpoint => $"{baseUrl}/sessions/?format=json";
    public override List<MapElementModel> LoadLevelElements()
    {
        try
        {
            // var result = UnityWebRequest.Get(GetMapElementsEndpoint).SendWebRequest();
            // while (!result.isDone) { }
            // var json = JsonConvert.DeserializeObject<List<MapElementModel>>(result.webRequest.downloadHandler.text);
            // return json;
            var sessions = GetSessions();
            string session = PlayerPrefs.GetString("session", "not-found");
            return sessions.FirstOrDefault(s => s.name == session)?.items ?? new();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return new();
        }
    }

    List<Session> GetSessions()
    {
        try
        {
            var result = UnityWebRequest.Get(GetSessionsEndpoint).SendWebRequest();
            var time = Time.time;
            while (!result.isDone) { 
                if (Time.time -time > timeoutLimit) return new();
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

    public override void SaveLevelElements(List<MapElementModel> elements)
    {
        var json = JsonConvert.SerializeObject(elements);

        try
        {
            var result = UnityWebRequest.Post(PostMapElementsEndpoint, json, "application/json");
            var time = Time.time;
            while (!result.isDone) {
                if (Time.time-time > timeoutLimit) return;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public override void SaveNewElement(MapElementModel model)
    {
        
    }
}