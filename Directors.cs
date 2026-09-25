using System.Collections.Generic;

public class Director : Person
{
    public string SignatureStyle; // Наприклад: "Любить симетричні кадри та темний похмурий тон"
    public List<string> DirectedWorks; // Фільми та серіали, які він зняв

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