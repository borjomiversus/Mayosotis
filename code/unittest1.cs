using System;
using System.Collections.Generic;
using System.Linq;

// Кожен метод тестує один клас ізольовано, зі своїми власними об'єктами.
public static class TestRunner
{
    private static int passed = 0;
    private static int failed = 0;

    private static void Check(string testName, bool condition)
    {
        if (condition)
        {
            Console.WriteLine($"  [OK]   {testName}");
            passed++;
        }
        else
        {
            Console.WriteLine($"  [FAIL] {testName}");
            failed++;
        }
    }

    public static void RunAllTests()
    {
        Console.WriteLine("\n========== ЗАПУСК ПОВНОГО НАБОРУ ТЕСТІВ ==========\n");

        TestPerson();
        TestActor();
        TestDirector();
        TestMedia();
        TestMovie();
        TestSeries();
        TestVibeRating();
        TestHiatusRating();
        TestFranchiseTimeLine();
        TestSmartRoulette();
        TestMediaLibrary();
        TestCaptureEntry();
        TestWatchlistEntry();
        TestWatchlist();
        TestUser();

        Console.WriteLine($"\n========== РЕЗУЛЬТАТ: {passed} пройдено, {failed} провалено ==========\n");
    }

    private static void TestPerson()
    {
        Console.WriteLine("--- Person ---");
        Person p = new Person("Jane Doe", 1990);
        Check("Конструктор зберігає FullName", p.FullName == "Jane Doe");
        Check("Конструктор зберігає BirthYear", p.BirthYear == 1990);
        Check("GetAge рахує вік коректно", p.GetAge(2026) == 36);
        p.UpdateBiography("Тестова біографія");
        Check("UpdateBiography змінює Biography", p.Biography == "Тестова біографія");
        Check("GetBasicInfo містить ім'я і рік", p.GetBasicInfo().Contains("Jane Doe") && p.GetBasicInfo().Contains("1990"));
    }

    private static void TestActor()
    {
        Console.WriteLine("--- Actor ---");
        Actor a = new Actor("Test Actor", 1980, "Hero");
        Check("Конструктор зберігає RoleName", a.RoleName == "Hero");
        Check("Filmography порожня на старті", a.GetWorksCount() == 0);
        a.AddToFilmography("Movie A");
        a.AddToFilmography("Movie B");
        Check("AddToFilmography додає елементи", a.GetWorksCount() == 2);
        Check("HasWorkedOn знаходить існуючий тайтл", a.HasWorkedOn("Movie A"));
        Check("HasWorkedOn коректно не знаходить чужий тайтл", !a.HasWorkedOn("Movie C"));
    }

    private static void TestDirector()
    {
        Console.WriteLine("--- Director ---");
        Director d = new Director("Test Director", 1970, "Мінімалізм");
        Check("Конструктор зберігає SignatureStyle", d.SignatureStyle == "Мінімалізм");
        d.AddDirectedWork("Film X");
        Check("AddDirectedWork додає роботу", d.DirectedWorks.Contains("Film X"));
        Check("Director успадковує GetAge від Person", d.GetAge(2026) == 56);
    }

    private static void TestMedia()
    {
        Console.WriteLine("--- Media (через Movie, бо абстрактний) ---");
        Movie m1 = new Movie("Test Movie 1", 2010, 100, null);
        Movie m2 = new Movie("Test Movie 2", 2015, 90, null);

        m1.AddGenre("Comedy");
        m1.AddGenre("Drama");
        m2.AddGenre("Comedy");
        m2.AddGenre("Action");

        m1.AddTag("comfort");
        Check("AddTag додає тег", m1.Tags.Contains("comfort"));
        Check("AddGenre додає жанр", m1.Genres.Contains("Comedy"));

        m1.UpdateDescription("Опис 1");
        Check("UpdateDescription змінює Description", m1.Description == "Опис 1");

        m1.SetProductionDetails("Studio X", "USA");
        Check("SetProductionDetails зберігає студію", m1.ProductionStudio == "Studio X");
        Check("SetProductionDetails зберігає країну", m1.Country == "USA");

        m1.ApplyAgeRestriction(16, "R");
        Check("ApplyAgeRestriction зберігає вік", m1.ageRestriction == 16);
        Check("IsAgeAppropriate: 18 >= 16 -> true", m1.IsAgeAppropriate(18));
        Check("IsAgeAppropriate: 10 >= 16 -> false", !m1.IsAgeAppropriate(10));

        Check("CountSharedGenres рахує спільний жанр (Comedy)", m1.CountSharedGenres(m2) == 1);
        Check("MatchesFilter знаходить за тегом", m1.MatchesFilter("comfort"));
        Check("MatchesFilter знаходить за жанром", m1.MatchesFilter("Drama"));
        Check("MatchesFilter не знаходить неіснуюче", !m1.MatchesFilter("nonsense"));

        Check("GetFormattedSummary містить назву й рік", m1.GetFormattedSummary().Contains("Test Movie 1") && m1.GetFormattedSummary().Contains("2010"));

        Check("CompareTo: старіший рік менший", m1.CompareTo(m2) < 0);
        Check("CompareTo з null повертає 1", m1.CompareTo(null) == 1);
    }

    private static void TestMovie()
    {
        Console.WriteLine("--- Movie ---");
        Director dir = new Director("Dir Name", 1975, "Style");
        Movie m = new Movie("Movie Test", 2020, 120, dir);
        Check("Конструктор зберігає DurationMinutes", m.DurationMinutes == 120);
        Check("MediaDirector встановлюється через конструктор", m.MediaDirector == dir);
        Check("CalculateTimeDebt = тривалість фільму", m.CalculateTimeDebt() == 120);
    }

    private static void TestSeries()
    {
        Console.WriteLine("--- Series ---");
        Series s = new Series("Series Test", 2018, 45, 6);
        Check("CalculateTimeDebt = серії * тривалість (45*6=270)", s.CalculateTimeDebt() == 270);
        Check("IsCaughtUp false, коли є непереглянуті серії", !s.IsCaughtUp());

        Series s2 = new Series("Series Done", 2018, 45, 0);
        Check("IsCaughtUp true, коли UnwatchedEpisodes = 0", s2.IsCaughtUp());
        Check("GetWatchProgressSummary містить кількість серій", s.GetWatchProgressSummary().Contains("6"));
    }

    private static void TestVibeRating()
    {
        Console.WriteLine("--- VibeRating ---");
        VibeRating v = new VibeRating(8, 8, 8, 8, 8, 8);
        Check("CalculateAverage рахує коректно (8.0)", v.CalculateAverage() == 8.0);
        Check("IsHighlyRated true для порогу 7", v.IsHighlyRated(7));
        Check("IsHighlyRated false для порогу 9", !v.IsHighlyRated(9));
    }

    private static void TestHiatusRating()
    {
        Console.WriteLine("--- HiatusRating ---");
        Series s = new Series("Paused Series", 2020, 40, 3);
        HiatusRating h = new HiatusRating(s, 1, 5, "Нудно", DateTime.Now, "12:34");
        Check("Конструктор зберігає PausedSeries", h.PausedSeries == s);
        Check("Конструктор зберігає Season/Episode", h.Season == 1 && h.Episode == 5);
        try
        {
            h.ResumeWatching();
            Check("ResumeWatching виконується без винятків", true);
        }
        catch
        {
            Check("ResumeWatching виконується без винятків", false);
        }
    }

    private static void TestFranchiseTimeLine()
    {
        Console.WriteLine("--- FranchiseTimeLine ---");
        FranchiseTimeLine f = new FranchiseTimeLine("Test Universe");
        Movie m1 = new Movie("F1", 2001, 100, null);
        Movie m2 = new Movie("F2", 2003, 100, null);
        f.AddToTimeline(m1);
        f.AddToTimeline(m2);
        Check("AddToTimeline додає елементи", f.ChronologicalList.Count == 2);
        try
        {
            f.CalculateUniverseProgress(1);
            f.CalculateUniverseProgress(-1);
            f.CalculateUniverseProgress(999);
            Check("CalculateUniverseProgress не падає на межових значеннях", true);
        }
        catch
        {
            Check("CalculateUniverseProgress не падає на межових значеннях", false);
        }
    }

    private static void TestSmartRoulette()
    {
        Console.WriteLine("--- SmartRoulette ---");
        Movie mShort = new Movie("Short", 2010, 50, null);
        Movie mLong = new Movie("Long", 2010, 200, null);
        mShort.AddTag("comfort");
        mLong.AddTag("epic");
        List<Media> lib = new List<Media> { mShort, mLong };

        SmartRoulette r = new SmartRoulette(120, "comfort");

        var byTime = r.FilterList(lib, 100);
        Check("FilterList<int> лишає тільки коротші за 100 хв", byTime.Count == 1 && byTime[0] == mShort);

        var byTag = r.FilterList(lib, "comfort");
        Check("FilterList<string> лишає тільки з тегом comfort", byTag.Count == 1 && byTag[0] == mShort);

        var spin = r.SpinRoulette(byTime);
        Check("SpinRoulette повертає елемент з непорожнього списку", spin != null);

        var emptySpin = r.SpinRoulette(new List<Media>());
        Check("SpinRoulette повертає null для порожнього списку", emptySpin == null);
    }

    private static void TestMediaLibrary()
    {
        Console.WriteLine("--- MediaLibrary ---");
        MediaLibrary lib = new MediaLibrary();
        Director dir = new Director("Dir", 1970, "Style");
        Actor act = new Actor("Actor One", 1980, "Lead");

        Movie m1 = new Movie("Alpha Movie", 2010, 90, dir);
        m1.AddGenre("Comedy");
        m1.Cast.Add(act);
        m1.Vibe = new VibeRating(5, 5, 5, 5, 5, 5);

        Movie m2 = new Movie("Beta Movie", 2015, 90, dir);
        m2.AddGenre("Comedy");
        m2.AddGenre("Drama");
        m2.Vibe = new VibeRating(9, 9, 9, 9, 9, 9);

        lib.AddMedia(m1);
        lib.AddMedia(m2);
        Check("AddMedia додає в AllMedia", lib.AllMedia.Count == 2);

        Check("SearchByTitle знаходить за частковим збігом", lib.SearchByTitle("alpha").Count == 1);
        Check("FilterByGenre знаходить всі Comedy", lib.FilterByGenre("Comedy").Count == 2);
        Check("AdvancedSearch звужує за роком і жанром", lib.AdvancedSearch("Comedy", 2012, 2020).Count == 1);
        Check("SearchByActor знаходить за актором", lib.SearchByActor("Actor One").Count == 1);
        Check("GetTopRated повертає найвищу оцінку першою", lib.GetTopRated(1)[0] == m2);
        Check("FindSimilar не включає сам об'єкт-референс", !lib.FindSimilar(m1, 5).Contains(m1));
        Check("FindSimilar знаходить спільний жанр", lib.FindSimilar(m1, 1)[0] == m2);
    }

    private static void TestCaptureEntry()
    {
        Console.WriteLine("--- CaptureEntry ---");
        CaptureEntry c = new CaptureEntry("той фільм про часову петлю", "TikTok");
        Check("Новий CaptureEntry не розпізнаний одразу", !c.IsResolved);
        Check("GetStatusSummary для нерозпізнаного містить '?'", c.GetStatusSummary().StartsWith("?"));

        Movie found = new Movie("Edge of Tomorrow", 2014, 113, null);
        c.Resolve(found);
        Check("Resolve встановлює IsResolved = true", c.IsResolved);
        Check("Resolve зберігає ResolvedMedia", c.ResolvedMedia == found);
        Check("GetStatusSummary для розпізнаного містить '✓'", c.GetStatusSummary().StartsWith("✓"));
    }

    private static void TestWatchlistEntry()
    {
        Console.WriteLine("--- WatchlistEntry ---");
        Movie m = new Movie("Entry Movie", 2020, 100, null);
        WatchlistEntry e = new WatchlistEntry(m, "перша нотатка");
        Check("Конструктор зберігає Item", e.Item == m);
        Check("Конструктор зберігає PersonalNote", e.PersonalNote == "перша нотатка");
        e.UpdateNote("нова нотатка");
        Check("UpdateNote змінює PersonalNote", e.PersonalNote == "нова нотатка");
        Check("GetSummary містить назву фільму", e.GetSummary().Contains("Entry Movie"));
    }

    private static void TestWatchlist()
    {
        Console.WriteLine("--- Watchlist ---");
        Watchlist w = new Watchlist("Test List", isPrivate: false);
        Check("IsPrivate встановлюється з конструктора", w.IsPrivate == false);

        Movie m1 = new Movie("WL Movie 1", 2010, 60, null);
        Movie m2 = new Movie("WL Movie 2", 2010, 90, null);
        w.AddEntry(m1, "note 1");
        w.AddEntry(m2, "note 2");
        Check("AddEntry додає записи", w.Entries.Count == 2);
        Check("GetTotalTimeDebt рахує суму (60+90=150)", w.GetTotalTimeDebt() == 150);

        bool removed = w.RemoveEntry(m1);
        Check("RemoveEntry повертає true для існуючого елемента", removed);
        Check("RemoveEntry дійсно видаляє запис", w.Entries.Count == 1);

        bool removedAgain = w.RemoveEntry(m1);
        Check("RemoveEntry повертає false для вже відсутнього елемента", !removedAgain);
    }

    private static void TestUser()
    {
        Console.WriteLine("--- User ---");
        User u = new User("test_user");
        Check("Username зберігається", u.Username == "test_user");

        Watchlist plans = u.CreateWatchlist("Плани");
        Check("CreateWatchlist додає підбірку в список", u.Watchlists.Count == 1);
        Check("CreateWatchlist повертає саме створений об'єкт", plans.Name == "Плани");

        u.AddCapture("розмита нотатка", "Instagram");
        Check("AddCapture додає запис у Captures", u.Captures.Count == 1);
        Check("GetUnresolvedCaptures бачить нерозпізнану нотатку", u.GetUnresolvedCaptures().Count == 1);

        Movie found = new Movie("Resolved Movie", 2019, 100, null);
        u.Captures[0].Resolve(found);
        Check("GetUnresolvedCaptures більше не бачить розпізнану нотатку", u.GetUnresolvedCaptures().Count == 0);

        bool moved = u.MoveResolvedCaptureToWatchlist(u.Captures[0], "Плани");
        Check("MoveResolvedCaptureToWatchlist повертає true при успіху", moved);
        Check("Capture видаляється зі списку Captures після переносу", u.Captures.Count == 0);
        Check("Фільм реально потрапив у Watchlist", plans.Entries.Any(e => e.Item == found));

        bool movedToWrong = u.MoveResolvedCaptureToWatchlist(new CaptureEntry("x", "y"), "Неіснуюча підбірка");
        Check("MoveResolvedCaptureToWatchlist повертає false для неіснуючої підбірки чи нерозпізнаної нотатки", !movedToWrong);

        u.LogSearch("query 1");
        u.LogSearch("query 2");
        Check("LogSearch додає новий пошук на початок списку", u.RecentSearches[0] == "query 2");

        Movie viewMovie = new Movie("Viewed", 2020, 90, null);
        u.LogView(viewMovie);
        Check("LogView додає перегляд", u.RecentlyViewed.Contains(viewMovie));
    }
}
