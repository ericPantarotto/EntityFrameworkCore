namespace EntityFrameworkNet6.ConsoleApp;
public record struct TeamDetail
{
    public string TeamName { get; set; }
    public string CoachName { get; set; }
    public string LeagueName { get; set; }

    public TeamDetail(string teamName, string coachName, string leagueName)
    {
        TeamName = teamName;
        CoachName = coachName;
        LeagueName = leagueName;
    }

}
