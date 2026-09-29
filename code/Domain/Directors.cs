using System.Collections.Generic;

public class Director : Person
{
    public string SignatureStyle { get; private set; }
    public List<string> DirectedWorks { get; private set; }

    public Director(string fullName, int birthYear, string signatureStyle) : base(fullName, birthYear)
    {
        if (string.IsNullOrWhiteSpace(signatureStyle))
            throw new ArgumentException("Стиль режисера має бути вказаний.");

        SignatureStyle = signatureStyle;
        DirectedWorks = new List<string>();
    }

    public void AddDirectedWork(string title)
    {
        if (!string.IsNullOrWhiteSpace(title))
            DirectedWorks.Add(title);
    }
}