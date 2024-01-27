using System.Collections.Generic;

[System.Serializable]
public class Team
{
    public string id;
    public string name;
    public List<string> users;
}

[System.Serializable]
public class TeamCollection
{
    public List<Team> teams;
}