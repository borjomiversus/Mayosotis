using System.Collections.Generic;

public class Actor : Person
{
    public string RoleName; 
    public List<string> Filmography;

    public Actor(string fullName, int birthYear, string roleName) : base(fullName, birthYear)
    {
        RoleName = roleName;
        Filmography = new List<string>();
    }

    public void AddToFilmography(string title)
    {
        Filmography.Add(title);
    }

    public int GetWorksCount()
    {
        return Filmography.Count;
    }

    public bool HasWorkedOn(string title)
    {
        return Filmography.Contains(title);
    }
}