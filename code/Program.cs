using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        TestRunner.RunAllTests();
        Console.WriteLine("Запуск\n");

        // головний каталог
        MediaLibrary imdbCatalog = new MediaLibrary();

        Director director1 = new Director("Robert Luketic", 1973, "Романтичні комедії");
        Actor actor1 = new Actor("Gerard Butler", 1969, "Mike Chadway");
        actor1.AddToFilmography("The Ugly Truth");

        Director director2 = new Director("Eric Kripke", 1974, "Сатира та чорний гумор");
        Actor actor2 = new Actor("Antony Starr", 1975, "Homelander");

        Movie movie1 = new Movie("The Ugly Truth", 2009, 96, director1);
        movie1.AddGenre("Comedy");
        movie1.AddGenre("Romance");
        movie1.ApplyAgeRestriction(16, "R");
        movie1.Cast.Add(actor1);
        movie1.Vibe = new VibeRating(8, 7, 7, 6, 9, 8); // Середній бал ~7.5

        Series series1 = new Series("The Boys", 2019, 60, 8);
        series1.AddGenre("Action");
        series1.AddGenre("Comedy");
        series1.ApplyAgeRestriction(18, "R");
        series1.Cast.Add(actor2);
        series1.MediaDirector = director2;
        series1.Vibe = new VibeRating(9, 9, 10, 8, 10, 9); // Середній бал ~9.1
        series1.PauseStatus = new HiatusRating(series1, 2, 4, "Немає часу", DateTime.Now, "45:10");

        imdbCatalog.AddMedia(movie1);
        imdbCatalog.AddMedia(series1);


        Console.WriteLine("\nКористувач, нотатник, підбірки\n");

        User me = new User("spacedream");

        // базові підбірки
        Watchlist plans = me.CreateWatchlist("Плани", isPrivate: true);
        Watchlist comfortPicks = me.CreateWatchlist("Затишні фільми", isPrivate: false);

        // три сценарії нотаток

        // точна назва — розпізнається одразу
        me.AddCapture("The Ugly Truth", "TikTok");

        // назва + особистий контекст
        me.AddCapture("The Boys, порадили, дуже смішно", "Друг порадив");

        // розмитий опис — поки не розпізнано
        me.AddCapture("той серіал про супергероїв", "Instagram, не пам'ятаю назву");

        Console.WriteLine("Усі нотатки");
        foreach (var capture in me.Captures)
        {
            Console.WriteLine(capture.GetStatusSummary());
        }

        // вручну перші дві
        me.Captures[0].Resolve(movie1);
        me.Captures[1].Resolve(series1);

        Console.WriteLine("\nНотатки після ручного розпізнавання");
        foreach (var capture in me.Captures)
        {
            Console.WriteLine(capture.GetStatusSummary());
        }

        // розпізнані нотатки в підбірку "Плани"
        me.MoveResolvedCaptureToWatchlist(me.Captures[0], "Плани"); 
        me.MoveResolvedCaptureToWatchlist(me.Captures[0], "Плани"); 

        Console.WriteLine();
        plans.PrintContents();

        // Третя нотатка лишилась нерозпізнаною, скільки їх ще висить
        Console.WriteLine($"\nНерозпізнаних нотаток лишилось: {me.GetUnresolvedCaptures().Count}");

        // фільм у публічну підбірку з власною нотаткою напряму 
        comfortPicks.AddEntry(movie1, "Дивитись коли поганий настрій");
        Console.WriteLine();
        comfortPicks.PrintContents();

        // Історія пошуку й переглядів
        me.LogSearch("романтичні комедії 2009");
        me.LogView(movie1);
        me.LogView(series1);
        Console.WriteLine($"\nОстанній пошук: {me.RecentSearches.First()}");
        Console.WriteLine($"Нещодавно переглядала: {string.Join(", ", me.RecentlyViewed.Select(m => m.Title))}");

        // схоже на те, що сподобалось
        Console.WriteLine("\nСхоже на The Boys");
        var similar = imdbCatalog.FindSimilar(series1, 1);
        foreach (var item in similar)
        {
            Console.WriteLine($"Рекомендація: {item.Title}");
        }

        // Загальний борг часу конкретної підбірки
        Console.WriteLine($"\nЗагальний борг часу підбірки \"Плани\": {plans.GetTotalTimeDebt()} хв.");


        // ================= ДЕМОНСТРАЦІЯ ФУНКЦІОНАЛУ =================

        Console.WriteLine("--- ТОП РЕЙТИНГ (IMDb Top Rated) ---");
        var topMedia = imdbCatalog.GetTopRated(2);
        foreach (var item in topMedia)
        {
            Console.WriteLine($"{item.Title} - Оцінка: {item.Vibe.CalculateAverage():F1}/10");
        }

        Console.WriteLine("\nРОЗШИРЕНИЙ ПОШУК (Комедії з 2005 по 2020)");
        var searchResults = imdbCatalog.AdvancedSearch("Comedy", 2005, 2020);
        foreach (var item in searchResults)
        {
            Console.WriteLine($"Знайдено: {item.Title} ({item.ReleaseYear})");
        }

        Console.WriteLine("\nПошук за актором: Antony Starr");
        var actorResults = imdbCatalog.SearchByActor("Antony Starr");
        foreach (var item in actorResults)
        {
            Console.WriteLine($"Актор грає у: {item.Title}");
        }

        Console.WriteLine("\nСортування IComparable");
        imdbCatalog.AllMedia.Sort();
        foreach (var item in imdbCatalog.AllMedia)
        {
            Console.WriteLine($"{item.Title} - Рік випуску: {item.ReleaseYear}");
        }

        Console.WriteLine("\nРулетка");
        SmartRoulette roulette = new SmartRoulette(120, "comfort");
        List<Media> timeFiltered = roulette.FilterList(imdbCatalog.AllMedia, 100); // Пошук медіа до 100 хвилин
        Media tonightChoice = roulette.SpinRoulette(timeFiltered);
        if (tonightChoice != null)
        {
            Console.WriteLine($"Рулетка обрала для перегляду: {tonightChoice.Title}");
        }

        Console.WriteLine("\nFranchiseTimeLine");
        FranchiseTimeLine mcu = new FranchiseTimeLine("Marvel Cinematic Universe");
        mcu.AddToTimeline(movie1);
        mcu.AddToTimeline(series1);
        mcu.CalculateUniverseProgress(1);

        Console.WriteLine("\nвиконано");
        Console.ReadKey();
    }
}

