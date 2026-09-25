using System;

public class VibeRating
{
    public int Characters;
    public int Plot;
    public int Visuals;
    public int Pacing;
    public int Vibe;
    public int Soundtrack;
    public List<string> ExternalReviews;


    public VibeRating(int characters, int plot, int visuals, int pacing, int vibe, int soundtrack)
    {
        Characters = characters;
        Plot = plot;
        Visuals = visuals;
        Pacing = pacing;
        Vibe = vibe;
        Soundtrack = soundtrack;
    }
    public double CalculateAverage()
    {
        return (Characters + Plot + Visuals + Pacing + Vibe + Soundtrack) / 6.0;
    }

    public bool IsHighlyRated(double threshold)
    {
        return CalculateAverage() >= threshold;
    }
}