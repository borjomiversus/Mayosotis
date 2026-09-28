using System.Collections.Generic;

public class Director : Person
{
    public string SignatureStyle; 
    public List<string> DirectedWorks; 

    public Director(string fullName, int birthYear, string signatureStyle) : base(fullName, birthYear)
    {
        SignatureStyle = signatureStyle;
        DirectedWorks = new List<string>();
    }

    public void AddDirectedWork(string title)
    {
        DirectedWorks.Add(title);
    }
}