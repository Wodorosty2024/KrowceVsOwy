using System.Collections.Generic;

[System.Serializable]
public class Session {
    public string id;
    public string name;
    public List<string> users;
    public List<MapElementModel> items;
}

[System.Serializable]
public class SessionCollection {
    public List<Session> sessions;
}