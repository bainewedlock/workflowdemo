namespace WorkerDemo.Pages.Workflows
{
    public class LogEntry : IComparable<LogEntry>
    {
        public string Time { get; set; } = "";
        public string Message { get; set; } = "";

        public int CompareTo(LogEntry? other)
        {
            return Time.CompareTo(other?.Time);
        }
    }
}
