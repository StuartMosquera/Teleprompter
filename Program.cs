namespace Teleprompter;

internal class Program
{
    private static async Task Main(string[] args) => await RunTeleprompter();

    private static async Task RunTeleprompter()
    {
        var config = new TeleprompterConfig();
        Task display = ShowTeleprompter(config);
        Task speed = GetInput(config);

        await Task.WhenAny(display, speed);
    }

    private static async Task ShowTeleprompter(TeleprompterConfig config)
    {
        IEnumerable<string> words = ReadFrom("sampleQuotes.txt");

        foreach (string word in words)
        {
            Console.Write(word);

            if (!string.IsNullOrWhiteSpace(word))
                await Task.Delay(config.DelayInMilliseconds);
        }

        config.SetDone();
    }

    private static async Task GetInput(TeleprompterConfig config)
    {
        void Work()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.KeyChar == '>')
                    config.UpdateDelay(-10);

                if (key.KeyChar == '<')
                    config.UpdateDelay(10);

                if ((key.KeyChar == 'X') || (key.KeyChar == 'x'))
                    config.SetDone();
            } while (!config.Done);
        }

        await Task.Run(Work);
    }

    private static IEnumerable<string> ReadFrom(string file)
    {
        string? line;

        using StreamReader reader = File.OpenText(file);

        while ((line = reader.ReadLine()) != null)
        {
            string[] words = line.Split(' ');
            var lineLength = 0;

            foreach (string word in words)
            {
                yield return word + " ";
                lineLength += word.Length + 1;

                if (lineLength > 70)
                {
                    yield return Environment.NewLine;
                    lineLength = 0;
                }
            }

            yield return Environment.NewLine;
        }
    }
}
