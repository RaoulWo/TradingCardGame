namespace BusinessObjects.Game;

public class Log
{
    public List<string> Logs => _logs;

    private List<string> _logs = new List<string>();

    public void AddEntry(string entry)
    {
        _logs.Add(entry);
    }
}