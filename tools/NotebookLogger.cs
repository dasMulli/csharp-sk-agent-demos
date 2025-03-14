
class NotebookLoggerProvider(IEnumerable<ConsoleFormatter> consoleFormatters,
    IOptions<ConsoleLoggerOptions> consoleLoggingOptions
    ) : ILoggerProvider, ISupportExternalScope
{
    private readonly ConsoleFormatter consoleFormatter = consoleFormatters.FirstOrDefault(f => f.Name == consoleLoggingOptions.Value.FormatterName)
        ??  consoleFormatters.First();
    private IExternalScopeProvider scopeProvider;
    public ILogger CreateLogger(string categoryName) => new NotebookLogger(categoryName, consoleFormatter, scopeProvider);
    public void Dispose() { }
    public void SetScopeProvider(IExternalScopeProvider scopeProvider) => this.scopeProvider = scopeProvider;
}
class NotebookLogger(string categoryName, ConsoleFormatter consoleFormatter, IExternalScopeProvider scopeProvider) : ILogger
{
    public IDisposable BeginScope<TState>(TState state) => scopeProvider.Push(state);
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var logStringWriter = new System.IO.StringWriter();
        consoleFormatter.Write(new Microsoft.Extensions.Logging.Abstractions.LogEntry<TState>(
            logLevel, categoryName, eventId, state, exception, formatter),
            scopeProvider,
            logStringWriter);
        Console.WriteLine(logStringWriter.ToString());
    }
}