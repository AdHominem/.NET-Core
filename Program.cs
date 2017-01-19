using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TeleprompterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTeleprompterAsync().Wait();
        }

        private static async Task RunTeleprompterAsync()
        {
            var config = new TeleprompterConfig();
            var displayTask = ShowTeleprompterAsync(config);

            var speedTask = GetInputAsync(config);
            await Task.WhenAny(displayTask, speedTask);
        }

        private static async Task ShowTeleprompterAsync(TeleprompterConfig config)
        {
            foreach (var line in ReadFrom("sampleQuotes.txt"))
            {
                Console.Write(line);
                if (!string.IsNullOrWhiteSpace(line))
                {
                    await Task.Delay(config.DelayInMilliseconds);
                }
            }
            config.SetDone();
        }

        private static async Task GetInputAsync(TeleprompterConfig config)
        {
            Action work = () =>
            {
                do
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '>')
                        config.UpdateDelay(-10);
                    else if (key.KeyChar == '<')
                        config.UpdateDelay(10);
                } while (!config.Done);
            };
            await Task.Run(work);
        }

        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using (var reader = File.OpenText(file))
            {
                var lineLength = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    foreach (var word in line.Split(' '))
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
                    lineLength = 0;
                }
            }
        }
    }
}
