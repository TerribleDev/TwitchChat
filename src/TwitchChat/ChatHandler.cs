using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitchChat
{
    public class ChatHandler
    {
        private readonly string userName;
        private readonly string password;
        private readonly string host;
        private readonly int port;
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        public ChatHandler(string userName, string password, string host = "irc.chat.twitch.tv", int port = 6667)
        {


            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            this.userName = userName;
            if(string.IsNullOrWhiteSpace(password))
            {
                var envPassword = Environment.GetEnvironmentVariable("TWITCH_OAUTH");
                if(string.IsNullOrEmpty(envPassword))
                {
                    throw new ArgumentNullException(nameof(password));
                }
                this.password = envPassword;
            }
            else
            {
                this.password = password;
            }

            this.port = port;
            this.host = host;
        }
        public async Task ConnectAsync() 
        {
            client?.Dispose();
            client = new TcpClient();
            await client.ConnectAsync(this.host, this.port);
            reader?.Dispose();
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            await writer.WriteLineAsync($"PASS {this.password}");
            await writer.WriteLineAsync($"NICK {this.userName}");
            await writer.WriteLineAsync($"USER {this.userName} 8 * :{userName}");
            await writer.WriteLineAsync($"JOIN #{userName}");
            await writer.FlushAsync();
            Enumerable.Range(0, 10)
                .ToList()
                .ForEach(a=>reader.ReadLine());
            
        }

        public async Task<string> ReadLine()
        {
            if(client == null || !client.Connected)
            {
                await Console.Error.WriteLineAsync("client not connected!");
                await this.ConnectAsync();
            }
             return  await reader.ReadLineAsync();
        }
        

    }
}