// This entire file is written by AI
using System.IO.Pipes;

public class PipeClient
{
    private NamedPipeClientStream? _pipe;
    private StreamReader? _reader;
    private StreamWriter? _writer;

    public async Task ConnectAsync()
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
    }

    public async Task SendAsync(string message)
    {
        if (_writer == null)
            throw new InvalidOperationException("Not connected.");

        await _writer.WriteLineAsync(message);
    }

    public async Task<string?> ReceiveAsync()
    {
        if (_reader == null)
            throw new InvalidOperationException("Not connected.");

        return await _reader.ReadLineAsync();
    }
}