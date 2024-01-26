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
    public string baseUrl = "https://krowce.bieda.it";

    string GetMapElementsEndpoint => $"{baseUrl}/test";
    string PostMapElementsEndpoint => $"{baseUrl}/test";
    public override List<MapElementModel> LoadLevelElements()
    {
        try
        {
            var result = UnityWebRequest.Get(GetMapElementsEndpoint).SendWebRequest();
            while (!result.isDone) { }
            var json = JsonConvert.DeserializeObject<List<MapElementModel>>(result.webRequest.downloadHandler.text);
            return json;
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
            while (!result.isDone) {}
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}