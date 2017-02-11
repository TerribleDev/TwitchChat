using System;
using System.Threading.Tasks;

namespace TwitchChat
{
    public class Program
    {
        public ChatHandler handler;
        public static void Main(string[] args)
        {
            Console.Clear();
            new Program().MainAsync(args).GetAwaiter().GetResult();
        }

        public async Task MainAsync(string[] args)
        {
            handler = new ChatHandler("terrible_dev", string.Empty);
            await Console.Out.WriteLineAsync("Connecting!");
            await handler.ConnectAsync();
            await Console.Out.WriteLineAsync("Connected!");
            while(true)
            {
                var chatLine = await handler.ReadLine();
                Console.WriteLine(chatLine);
                await Task.Delay(250);
            }
        }
    }
}
