using System;
using System.Collections.Generic;

public static class TestRunner
{
    private static int passed = 0;
    private static int failed = 0;

    private static void Check(string testName, bool condition)
    {
        if (condition) { Console.WriteLine($"  [OK]   {testName}"); passed++; }
        else { Console.WriteLine($"  [FAIL] {testName}"); failed++; }
    }

    public static void RunAllTests()
    {
        Console.WriteLine("========== ЗАПУСК ТЕСТІВ ==========\n");
        TestPerson();
        TestActorDirector();
        TestMedia();
        TestMovieSeries();
        TestSeasonEpisode();
        TestVibeRating();
        TestReviewSong();
        TestWatchCheckpoint();
        TestFranchiseTimeLine();
        TestCaptureEntry();
        TestWatchlist();
        TestUser();
        TestUserMediaState();
        TestServices();
        Console.WriteLine($"\nРЕЗУЛЬТАТ: {passed} пройдено, {failed} провалено\n");
    }

    private static void TestPerson()
    {
        Console.WriteLine("--- Person ---");
        Person p = new Person("Jane Doe", 1990);
        Check("GetAge рахує вік коректно", p.GetAge(2026) == 36);
        p.UpdateBiography("Тест");
        Check("UpdateBiography змінює Biography", p.Biography == "Тест");
        Check("GetBasicInfo містить ім'я", p.GetBasicInfo().Contains("Jane Doe"));
        Check("Person з порожнім ім'ям", ThrowsException(() => new Person("", 1990)));
        Check("Person з некоректним роком", ThrowsException(() => new Person("John", 1800)));
    }

    private static void TestActorDirector()
    {
        Console.WriteLine("--- Actor / Director ---");
        Actor a = new Actor("Test Actor", 1980, "Hero");
        a.AddToFilmography("Movie A");
        Check("AddToFilmography додає елемент", a.GetWorksCount() == 1);
        Check("HasWorkedOn знаходить існуючий тайтл", a.HasWorkedOn("Movie A"));
        Check("Actor без ролі", ThrowsException(() => new Actor("Test", 1990, "")));

        Director d = new Director("Test Director", 1970, "Style");
        d.AddDirectedWork("Film X");
        Check("AddDirectedWork додає роботу", d.DirectedWorks.Contains("Film X"));
        Check("Director успадковує GetAge", d.GetAge(2026) == 56);
    }

    private static void TestMedia()
    {
        Console.WriteLine("--- Media ---");
        Movie m1 = new Movie("Test 1", 2010, 100, null);
        Movie m2 = new Movie("Test 2", 2015, 90, null);
        m1.AddGenre("Comedy"); m1.AddGenre("Drama");
        m2.AddGenre("Comedy");

        Check("Порожня назва кидає виняток", ThrowsException(() => new Movie("", 2020, 100, null)));
        Check("Рік до 1888 кидає виняток", ThrowsException(() => new Movie("X", 1800, 100, null)));

        m1.ApplyAgeRestriction(16, "R");
        Check("IsAgeAppropriate: 18>=16 -> true", m1.IsAgeAppropriate(18));
        Check("IsAgeAppropriate: 10>=16 -> false", !m1.IsAgeAppropriate(10));
        Check("CountSharedGenres знаходить спільний жанр", m1.CountSharedGenres(m2) == 1);
        Check("CompareTo: старіший рік менший", m1.CompareTo(m2) < 0);
        Check("CompareTo з null повертає 1", m1.CompareTo(null) == 1);
    }

    private static void TestMovieSeries()
    {
        Console.WriteLine("--- Movie / Series ---");
        Movie m = new Movie("Movie Test", 2020, 120, null);
        Check("Movie.CalculateTimeDebt = тривалість", m.CalculateTimeDebt() == 120);
        Check("Тривалість <=0 кидає виняток", ThrowsException(() => new Movie("X", 2020, 0, null)));

        Series s = new Series("Series Test", 2018, 45, 6, null);
        Check("Series.CalculateTimeDebt = серії*тривалість (270)", s.CalculateTimeDebt() == 270);
        Check("IsCaughtUp false, коли є непереглянуті", !s.IsCaughtUp());
        Check("GetWatchProgressSummary містить кількість серій", s.GetWatchProgressSummary().Contains("6"));
    }

    private static void TestSeasonEpisode()
    {
        Console.WriteLine("--- Season / Episode ---");
        Season season = new Season(1);
        Episode ep1 = new Episode(1, "Pilot", 45);
        Episode ep2 = new Episode(2, "Second", 50);
        season.AddEpisode(ep1);
        season.AddEpisode(ep2);
        Check("AddEpisode додає епізоди", season.Episodes.Count == 2);
        Check("GetTotalRuntime рахує суму (45+50=95)", season.GetTotalRuntime() == 95);
    }

    private static void TestVibeRating()
    {
        Console.WriteLine("--- VibeRating ---");
        VibeRating v = new VibeRating(8, 8, 8, 8, 8, 8);
        Check("CalculateAverage = 8.0", Math.Abs(v.CalculateAverage() - 8.0) < 0.001);
        Check("IsHighlyRated true для порогу 7", v.IsHighlyRated(7));
        Check("Значення поза 0..10 кидає виняток", ThrowsException(() => new VibeRating(11, 5, 5, 5, 5, 5)));
    }

    private static void TestReviewSong()
    {
        Console.WriteLine("--- Review / Song ---");
        User author = new User("reviewer");
        Movie m = new Movie("Reviewed Movie", 2020, 100, null);
        Review r = new Review(author, m, "Головний герой помирає", isSpoiler: true);
        Check("GetDisplayText приховує спойлер за замовчуванням", !r.GetDisplayText().Contains("помирає"));
        Check("GetDisplayText показує спойлер, якщо попросили", r.GetDisplayText(true).Contains("помирає"));
        Check("Song без назви", ThrowsException(() => new Song("", "Artist")));
        Check("Song без виконавця", ThrowsException(() => new Song("Title", "")));

        Song s1 = new Song("Song One", "Artist A");
        Song s2 = new Song("Song Two", "Artist B");
        m.AddSong(s1);
        m.AddSong(s2);
        Check("Soundtracks містить декілька пісень (не одну)", m.Soundtracks.Count == 2);
        Check("GetDisplayName форматує назву й виконавця", s1.GetDisplayName().Contains("Artist A"));
    }

    private static void TestWatchCheckpoint()
    {
        Console.WriteLine("--- WatchCheckpoint ---");
        Series s = new Series("Paused Series", 2020, 40, 3, null);
        WatchCheckpoint wc = new WatchCheckpoint(s, 1, 5, "12:34", "Нудно");
        Check("Новий чекпоінт активний", wc.IsActive);
        Check("GetStatusDescription містить серію", wc.GetStatusDescription().Contains("S1E5"));
        string result = wc.ResumeWatching();
        Check("Після ResumeWatching чекпоінт неактивний", !wc.IsActive);
        Check("ResumeWatching повертає опис, не друкує сам", result.Contains("Paused Series"));
    }

    private static void TestFranchiseTimeLine()
    {
        Console.WriteLine("--- FranchiseTimeLine ---");
        FranchiseTimeLine f = new FranchiseTimeLine("Test Universe");
        f.AddToTimeline(new Movie("F1", 2001, 100, null));
        f.AddToTimeline(new Movie("F2", 2003, 100, null));
        Check("AddToTimeline додає елементи", f.ChronologicalList.Count == 2);
    }

    private static void TestCaptureEntry()
    {
        Console.WriteLine("--- CaptureEntry ---");
        CaptureEntry c = new CaptureEntry("Нотатка", "TikTok");
        Check("Статус нової нотатки - Unresolved", c.Status == CaptureStatus.Unresolved);
        Movie found = new Movie("Found Movie", 2019, 100, null);
        c.Resolve(found);
        Check("Статус після розпізнавання - Resolved", c.Status == CaptureStatus.Resolved);
        Check("ResolvedMedia встановлено", c.ResolvedMedia == found);
        c.Archive();
        Check("Статус після архівації - Archived", c.Status == CaptureStatus.Archived);
    }

    private static void TestWatchlist()
    {
        Console.WriteLine("--- Watchlist ---");
        Watchlist w = new Watchlist("Test List", isPrivate: false);
        Movie m = new Movie("WL Movie", 2010, 60, null);
        w.AddEntry(m, "нотатка");
        Check("Запис додано", w.Entries.Count == 1);
        Check("GetTotalTimeDebt рахує час", w.GetTotalTimeDebt() == 60);
        bool removed = w.RemoveEntry(m);
        Check("Запис видалено", removed && w.Entries.Count == 0);
    }

    private static void TestUser()
    {
        Console.WriteLine("--- User ---");
        User u = new User("test_user");
        u.AddCapture("Розмита нотатка", "Instagram");
        Check("Нерозпізнана нотатка знайдена", u.GetUnresolvedCaptures().Count == 1);

        Movie m1 = new Movie("Jan", 2020, 100, null);
        m1.AddGenre("Comedy");
        u.WatchHistory.Add(new WatchHistoryEntry(m1, new DateTime(2026, 1, 10)));
        Check("GetMonthlyStats рахує за рік+місяць", u.GetMonthlyStats(2026, 1) == 100);
        Check("GetMonthlyStats не бачить інший місяць", u.GetMonthlyStats(2026, 2) == 0);

        var breakdown = u.GetMonthlyGenreStats(2026, 1);
        Check("GetMonthlyGenreStats бачить жанр", breakdown.ContainsKey("Comedy"));
    }

    private static void TestUserMediaState()
    {
        Console.WriteLine("--- UserMediaState ---");
        Movie m = new Movie("Status Test", 2020, 90, null);
        UserMediaState state = new UserMediaState(m, WatchStatus.Planned);
        Check("Planned не активний", !state.IsActive());
        state.ChangeStatus(WatchStatus.Watching);
        Check("Watching активний", state.IsActive());
        Check("StartedAt встановлено при Watching", state.StartedAt != null);
        state.ChangeStatus(WatchStatus.Watched);
        Check("CompletedAt встановлено при Watched", state.CompletedAt != null);
    }

    private static void TestServices()
    {
        Console.WriteLine("--- Services ---");
        List<Media> lib = new List<Media>();
        Movie a = new Movie("Alpha", 2010, 90, null);
        a.AddGenre("Comedy");
        Movie b = new Movie("Beta", 2015, 90, null);
        b.AddGenre("Comedy");
        b.SetVibeRating(new VibeRating(9, 9, 9, 9, 9, 9));
        lib.Add(a); lib.Add(b);

        var search = new MediaSearchService();
        Check("SearchByTitle знаходить за частковим збігом", search.SearchByTitle(lib, "alpha").Count == 1);
        Check("GetTopRated повертає найвищу оцінку першою", search.GetTopRated(lib, 1)[0] == b);

        var rec = new RecommendationService();
        Check("FindSimilar не включає сам об'єкт", !rec.FindSimilar(lib, a, 5).Contains(a));

        var roulette = new RouletteService();
        var picked = roulette.Choose(lib, new RouletteCriteria { MaxMinutes = 100 });
        Check("RouletteService повертає елемент у межах критерію", picked != null && picked.CalculateTimeDebt() <= 100);
    }

    private static bool ThrowsException(Action action)
    {
        try { action(); return false; }
        catch { return true; }
    }
}