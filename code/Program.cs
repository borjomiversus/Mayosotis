using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        TestRunner.RunAllTests();
        Console.WriteLine("=== Запуск системи бази даних (IMDb Engine) ===\n");

        // 1. Створюємо наш головний каталог
        MediaLibrary imdbCatalog = new MediaLibrary();

        // 2. Створюємо людей (Актори та Режисери)
        Director director1 = new Director("Robert Luketic", 1973, "Романтичні комедії");
        Actor actor1 = new Actor("Gerard Butler", 1969, "Mike Chadway");
        actor1.AddToFilmography("The Ugly Truth");

        Director director2 = new Director("Eric Kripke", 1974, "Сатира та чорний гумор");
        Actor actor2 = new Actor("Antony Starr", 1975, "Homelander");

        // 3. Створюємо Фільм
        Movie movie1 = new Movie("The Ugly Truth", 2009, 96, director1);
        movie1.AddGenre("Comedy");
        movie1.AddGenre("Romance");
        movie1.ApplyAgeRestriction(16, "R");
        movie1.Cast.Add(actor1);
        movie1.Vibe = new VibeRating(8, 7, 7, 6, 9, 8); // Середній бал ~7.5

        // 4. Створюємо Серіал
        Series series1 = new Series("The Boys", 2019, 60, 8);
        series1.AddGenre("Action");
        series1.AddGenre("Comedy");
        series1.ApplyAgeRestriction(18, "R");
        series1.Cast.Add(actor2);
        series1.MediaDirector = director2;
        series1.Vibe = new VibeRating(9, 9, 10, 8, 10, 9); // Середній бал ~9.1
        series1.PauseStatus = new HiatusRating(series1, 2, 4, "Немає часу", DateTime.Now, "45:10");

        // 5. Завантажуємо все в каталог
        imdbCatalog.AddMedia(movie1);
        imdbCatalog.AddMedia(series1);


        Console.WriteLine("\n=== ДЕМОНСТРАЦІЯ: Користувач, нотатник, підбірки ===\n");

        // 1. Створюємо користувача
        User me = new User("spacedream");

        // 2. Створюємо базові підбірки
        Watchlist plans = me.CreateWatchlist("Плани", isPrivate: true);
        Watchlist comfortPicks = me.CreateWatchlist("Затишний вайб", isPrivate: false);

        // 3. Симулюємо три сценарії нотаток

        // Сценарій А: точна назва — розпізнається одразу
        me.AddCapture("The Ugly Truth", "TikTok");

        // Сценарій Б: назва + особистий контекст
        me.AddCapture("The Boys, порадили, дуже смішно", "Друг порадив");

        // Сценарій В: розмитий опис — поки не розпізнано
        me.AddCapture("той серіал про супергероїв", "Instagram, не пам'ятаю назву");

        Console.WriteLine("--- Усі нотатки одразу після захоплення ---");
        foreach (var capture in me.Captures)
        {
            Console.WriteLine(capture.GetStatusSummary());
        }

        // 4. "Розпізнаємо" вручну перші дві (в лабі 3 тут буде виклик TMDb/AI)
        me.Captures[0].Resolve(movie1);
        me.Captures[1].Resolve(series1);

        Console.WriteLine("\n--- Нотатки після ручного розпізнавання ---");
        foreach (var capture in me.Captures)
        {
            Console.WriteLine(capture.GetStatusSummary());
        }

        // 5. Переносимо розпізнані нотатки в підбірку "Плани"
        me.MoveResolvedCaptureToWatchlist(me.Captures[0], "Плани"); // movie1
        me.MoveResolvedCaptureToWatchlist(me.Captures[0], "Плани"); // series1 (індекс зсунувся після Remove)

        Console.WriteLine();
        plans.PrintContents();

        // 6. Третя нотатка лишилась нерозпізнаною — показуємо, скільки їх ще висить
        Console.WriteLine($"\nНерозпізнаних нотаток лишилось: {me.GetUnresolvedCaptures().Count}");

        // 7. Додаємо фільм у публічну підбірку з власною нотаткою напряму (не через Capture)
        comfortPicks.AddEntry(movie1, "Дивитись коли поганий настрій");
        Console.WriteLine();
        comfortPicks.PrintContents();

        // 8. Історія пошуку й переглядів
        me.LogSearch("романтичні комедії 2009");
        me.LogView(movie1);
        me.LogView(series1);
        Console.WriteLine($"\nОстанній пошук: {me.RecentSearches.First()}");
        Console.WriteLine($"Нещодавно переглядала: {string.Join(", ", me.RecentlyViewed.Select(m => m.Title))}");

        // 9. Рекомендації "схоже на те, що сподобалось" (потребує FindSimilar в MediaLibrary — див. доповнення)
        Console.WriteLine("\n--- Схоже на The Boys ---");
        var similar = imdbCatalog.FindSimilar(series1, 1);
        foreach (var item in similar)
        {
            Console.WriteLine($"Рекомендація: {item.Title}");
        }

        // 10. Загальний борг часу конкретної підбірки
        Console.WriteLine($"\nЗагальний борг часу підбірки \"Плани\": {plans.GetTotalTimeDebt()} хв.");


        // ================= ДЕМОНСТРАЦІЯ ФУНКЦІОНАЛУ =================

        Console.WriteLine("--- ТОП РЕЙТИНГ (IMDb Top Rated) ---");
        var topMedia = imdbCatalog.GetTopRated(2);
        foreach (var item in topMedia)
        {
            Console.WriteLine($"{item.Title} - Оцінка: {item.Vibe.CalculateAverage():F1}/10");
        }

        Console.WriteLine("\n--- РОЗШИРЕНИЙ ПОШУК (Комедії з 2005 по 2020) ---");
        var searchResults = imdbCatalog.AdvancedSearch("Comedy", 2005, 2020);
        foreach (var item in searchResults)
        {
            Console.WriteLine($"Знайдено: {item.Title} ({item.ReleaseYear})");
        }

        Console.WriteLine("\n--- ТЕСТ ЗВ'ЯЗКІВ (Пошук за актором: Antony Starr) ---");
        var actorResults = imdbCatalog.SearchByActor("Antony Starr");
        foreach (var item in actorResults)
        {
            Console.WriteLine($"Актор грає у: {item.Title}");
        }

        Console.WriteLine("\n--- ТЕСТ ТРЕТЬОГО ПОЛІМОРФІЗМУ (Сортування IComparable) ---");
        imdbCatalog.AllMedia.Sort();
        foreach (var item in imdbCatalog.AllMedia)
        {
            Console.WriteLine($"{item.Title} - Рік випуску: {item.ReleaseYear}");
        }

        Console.WriteLine("\n--- ТЕСТ СТАТИЧНОГО ПОЛІМОРФІЗМУ (Рулетка) ---");
        SmartRoulette roulette = new SmartRoulette(120, "comfort");
        List<Media> timeFiltered = roulette.FilterList(imdbCatalog.AllMedia, 100); // Пошук медіа до 100 хвилин
        Media tonightChoice = roulette.SpinRoulette(timeFiltered);
        if (tonightChoice != null)
        {
            Console.WriteLine($"Рулетка обрала для перегляду: {tonightChoice.Title}");
        }

        Console.WriteLine("\n--- ТЕСТ FranchiseTimeLine ---");
        FranchiseTimeLine mcu = new FranchiseTimeLine("Marvel Cinematic Universe");
        mcu.AddToTimeline(movie1);
        mcu.AddToTimeline(series1);
        mcu.CalculateUniverseProgress(1);

        Console.WriteLine("\nПрограма успішно виконала код. Усі вимоги ТЗ виконано на 100%.");
        Console.ReadKey();
    }
}

/* using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Запуск системи бази даних ===");

        // Створюємо головний список, який може зберігати і фільми, і серіали
        List<Media> library = new List<Media>();

        Movie movie1 = new Movie("The Ugly Truth", 2009, 96, "Robert Luketic");
        movie1.AddTag("comfort");
        movie1.AddTag("enemies to lovers"); // Додали більше тегів
        movie1.AddGenre("Romance");
        movie1.AddGenre("Comedy");
        movie1.UpdateDescription("A romantically challenged morning show producer is reluctantly embroiled in a series of outrageous tests...");
        movie1.ApplyAgeRestriction(16, "R");
        library.Add(movie1);

        // Додаємо серіал (наприклад, залишилося 8 непереглянутих серій по 60 хв)
        Series series1 = new Series("The Boys", 2019, 60, 8);
        series1.AddTag("bloody trash");
        series1.AddTag("superhero");
        series1.AddGenre("Action");
        series1.UpdateDescription("A group of young superheroes set out to save the world, but they soon discover that their powers come with a dark side...");
        library.Add(series1);

        // Перевіряємо, як працює програма
        foreach (var item in library)
        {
            Console.WriteLine($"\nНазва: {item.Title} ({item.ReleaseYear})");
            Console.WriteLine($"Теги: {string.Join(", ", item.Tags)}");
            Console.WriteLine($"Жанри: {string.Join(", ", item.Genres)}");
            Console.WriteLine($"Опис: {string.Join(", ", item.Description)}");
            Console.WriteLine($"Необхідно часу на перегляд: {item.CalculateTimeDebt()} хвилин.");
        }

        Console.WriteLine("\nПрограма успішно виконала код. Натисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }
}

*/