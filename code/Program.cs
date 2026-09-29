using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        TestRunner.RunAllTests();

        Console.WriteLine("\n========== ДЕМОНСТРАЦІЯ ==========\n");

        // просто список у пам'яті
        List<Media> library = new List<Media>();

        MediaLibrary mediaLibrary = new MediaLibrary();

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

        me.AddCapture("The Boys, порадили", "Друг");
        var unresolved = me.GetUnresolvedCaptures();
        var myCapture = unresolved[0];

        mediaLibrary.ResolveCapture(myCapture, theBoys);
        mediaLibrary.MoveToWatchlist(myCapture, plans);
        Console.WriteLine($"[1] Нотатку оброблено та перенесено в '{plans.Name}'.");

        // Пошук 
        var found = mediaLibrary.SearchByTitle(library, "boys");
        Console.WriteLine($"[2] Пошук 'boys' знайшов: {found[0].Title}");

        // Рекомендації
        var similar = mediaLibrary.FindSimilar(library, theBoys, 1);
        if (similar.Count > 0)
            Console.WriteLine($"[3] Рекомендація: {similar[0].Title}");

        // Рулетка 
        var choice = mediaLibrary.ChooseByRoulette(library, new RouletteCriteria(1000, "Action"));
        if (choice != null)
            Console.WriteLine($"[4] Рулетка обрала: {choice.Title}");
  
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

        Console.WriteLine("\nГотово");
        Console.ReadKey();
    }
}
