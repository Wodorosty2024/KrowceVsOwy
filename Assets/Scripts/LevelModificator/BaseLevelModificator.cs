using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLevelModificator : ScriptableObject
{
    public abstract List<MapElementModel> LoadLevelElements();

    public abstract void SaveLevelElements(List<MapElementModel> elements);
}