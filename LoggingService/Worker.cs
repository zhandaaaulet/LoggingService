using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LoggingService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IFolderLogger _folderLogger;

        public Worker(ILogger<Worker> logger, IFolderLogger folderLogger)
        {
            _logger = logger;
            _folderLogger = folderLogger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://localhost:61351/api/queue/retrieve/log");
                    string result = response.Content.ReadAsStringAsync().Result;
                    _logger.LogInformation(result);
                    try
                    {
                        var log = JsonConvert.DeserializeObject<MessageDto>(result);
                        if (log != null)
                        {
                           
                            _folderLogger.LogWrite($"Worker running at: {DateTimeOffset.Now}");
                            await client.GetAsync("http://localhost:61351/api/queue/handled/" + log.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex.Message);
                    }

                }
                await Task.Delay(20000, stoppingToken);
            }
        }
    }
}
