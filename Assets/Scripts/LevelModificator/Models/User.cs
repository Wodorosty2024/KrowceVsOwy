using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string id;
    public string name;
    public string team;
}

[System.Serializable]
public class UserCollection
{
    public List<User> users;
}