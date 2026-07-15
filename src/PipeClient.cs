
using System.IO.Pipes;

public class TartaMessage(string type, string sender, string recipient, string payload) : EventArgs
{
    public string Type { get; } = type;
    public string Sender { get; } = sender;
    public string Recipient { get; } = recipient;
    public string Payload { get; } = payload;
}

public class PipeClient
{
    private NamedPipeClientStream? _pipe;
    private StreamReader? _reader;
    private StreamWriter? _writer;


    public async Task ConnectAsync(string moduleName)
    {
        _pipe = new NamedPipeClientStream(
            ".",
            "MessagePipe",
            PipeDirection.InOut,
            PipeOptions.Asynchronous);

        await _pipe.ConnectAsync();

        _reader = new StreamReader(_pipe);
        _writer = new StreamWriter(_pipe)
        {
            AutoFlush = true
        };

        await _writer.WriteLineAsync(moduleName);

    }

    public async Task SendMessage(string type, string recipient, string payload)
    {
        if (_writer == null)
            throw new InvalidOperationException("Not connected.");

        var message = new TartaMessage("message", ClientName, recipientName, messageBody);
        var json_formatted_message = System.Text.Json.JsonSerializer.Serialize(message);
        await _writer.WriteLineAsync(json_formatted_message);
    }

    public async Task<string?> ReceiveAsync()
    {
        if (_reader == null)
            throw new InvalidOperationException("Not connected.");

        return await _reader.ReadToEndAsync();
    }
}