using System;

public class Person
{
    public string FullName;
    public int BirthYear;
    public string Biography;
    public string PhotoUrl;

    // Конструктор
    public Person(string fullName, int birthYear)
    {
        FullName = fullName;
        BirthYear = birthYear;
    }

    // Метод для розрахунку віку (або перевірки, чи жива людина / скільки років)
    public int GetAge(int currentYear)
    {
        return currentYear - BirthYear;
    }

    public void UpdateBiography(string bio)
    {
        Biography = bio;
    }

    public string GetBasicInfo()
    {
        return $"{FullName}, народився у {BirthYear}";
    }
}