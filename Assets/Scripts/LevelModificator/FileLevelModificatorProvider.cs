using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[CreateAssetMenu]
public class FileLevelModificatorProvider : BaseLevelModificator
{
    public string filename = "level.json";

    string FullPath => $"{Application.persistentDataPath}/{filename}";

    public override List<MapElementModel> LoadLevelElements()
    {
        Debug.Log(FullPath);
        if (!File.Exists(FullPath))
        {
            Debug.LogError("File doesn't exists");
        }

        var txt = File.ReadAllText(FullPath);
         var json = JsonConvert.DeserializeObject<List<MapElementModel>>(txt);

        return json;
    }

    public override void SaveLevelElements(List<MapElementModel> elements)
    {   
        var json = JsonConvert.SerializeObject(elements);
        File.WriteAllText(FullPath, json);
    }
}