using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        TestRunner.RunAllTests();

        Console.WriteLine("\n========== ДЕМОНСТРАЦІЯ ==========\n");

        // просто список у пам'яті
        List<Media> library = new List<Media>();

        var searchService = new MediaSearchService();
        var recommendationService = new RecommendationService();
        var rouletteService = new RouletteService();
        var captureService = new CaptureService();

        Director dir = new Director("Eric Kripke", 1974, "Satire");
        Series theBoys = new Series("The Boys", 2019, 60, 8, dir);
        theBoys.AddGenre("Action");
        library.Add(theBoys);

        Movie ugly = new Movie("The Ugly Truth", 2009, 96, null);
        ugly.AddGenre("Comedy");
        ugly.SetVibeRating(new VibeRating(8, 7, 7, 6, 9, 8));
        library.Add(ugly);

        User me = new User("spacedream");
        Watchlist plans = me.CreateWatchlist("Плани", isPrivate: true);

        // Нотатник
        me.AddCapture("The Boys, порадили", "Друг");
        var myCapture = me.GetUnresolvedCaptures().First();
        Console.WriteLine($"[1] Створено нотатку: '{myCapture.RawNote}'. Статус: {myCapture.Status}");

        captureService.ResolveCapture(myCapture, theBoys);
        Console.WriteLine($"[2] Нотатку розпізнано. Статус: {myCapture.Status}");

        captureService.MoveToWatchlist(myCapture, plans);
        Console.WriteLine($"[3] Нотатку перенесено. У списку '{plans.Name}' тепер {plans.Entries.Count} записів. Статус нотатки: {myCapture.Status}");

        // Пошук
        var found = searchService.SearchByTitle(library, "boys");
        Console.WriteLine($"\n[Пошук] За словом 'boys' знайдено: {string.Join(", ", found.Select(m => m.Title))}");

        // Рекомендації
        var similar = recommendationService.FindSimilar(library, theBoys, 1);
        if (similar.Any())
            Console.WriteLine($"[Рекомендації] Схоже на The Boys: {similar[0].Title}");

        // Рулетка
        var choice = rouletteService.Choose(library, new RouletteCriteria { MaxMinutes = 1000, Tag = "Action" });
        if (choice != null)
            Console.WriteLine($"[Рулетка] Обрано: {choice.Title}");

        // Перегляд + статистика
        me.RecordView(ugly);
        me.MarkAsWatched(ugly);
        int now = DateTime.Now.Year;
        int thisMonth = DateTime.Now.Month;
        int minutesThisMonth = me.GetMonthlyStats(now, thisMonth);
        Console.WriteLine($"\n[Статистика] Переглянуто за цей місяць: {minutesThisMonth} хв.");

        // Пауза серіалу
        WatchCheckpoint checkpoint = new WatchCheckpoint(theBoys, 2, 4, "45:10", "Немає часу");
        Console.WriteLine($"[Пауза] {checkpoint.GetStatusDescription()}");
        Console.WriteLine(checkpoint.ResumeWatching());

        Console.WriteLine("\nГотово. Натисніть будь-яку клавішу...");
        Console.ReadKey();
    }
}
