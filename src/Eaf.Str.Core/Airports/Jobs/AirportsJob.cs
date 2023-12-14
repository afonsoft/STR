using Eaf.BackgroundJobs.Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Eaf.Domain.Repositories;
using Eaf.ObjectMapping;
using Eaf.Domain.Uow;
using Eaf.Timing;
using System.ComponentModel;
using Hangfire;

namespace Eaf.Str.Airports.Jobs
{
    public class AirportsJob : AsyncBackgroundJob<bool>, IAirportsJob
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly HttpClient _httpClient;
        private readonly IRepository<Airport> _repository;
        private readonly IObjectMapper _objectMapper;
        private const string url = "https://channelcfg-api.voegol.com.br/ApplicationList/Airports";

        public AirportsJob(IHttpClientFactory httpClientFactory, IRepository<Airport> repository, IObjectMapper objectMapper, IUnitOfWorkManager unitOfWorkManager)
        {
            _repository = repository;
            _objectMapper = objectMapper;
            _httpClient = httpClientFactory.CreateClient("AirportsHttpClient");
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _unitOfWorkManager = unitOfWorkManager;
        }

        [DisableConcurrentExecution(300)]
        [Description("Update Airports")]
        public override async Task ExecuteAsync(bool args, PerformContext context, CancellationToken token)
        {
            context.WriteLine("Start AirportsJob");
            context.WriteLine($"Force Update: {args}");

            try
            {
                var responseMessage = await _httpClient.GetAsync(new Uri(url));

                responseMessage.EnsureSuccessStatusCode();

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();
                    if (responseContent.IsNullOrEmpty())
                        throw new EafException("Error in Response Contenct");

                    var jObject = JsonConvert.DeserializeObject<AirportsJsonDto>(responseContent);

                    if (jObject == null)
                        throw new EafException("Error in Deserialize Object");

                    foreach (var item in jObject.Content)
                    {
                        try
                        {
                            using (var unitOfWork = _unitOfWorkManager.Begin())
                            {
                                if (_repository.GetAll().Any(e => e.IATACode.ToUpper() == item.IATACode.ToUpper()))
                                {
                                    if (args)
                                    {
                                        var airport = _repository.GetAll().FirstOrDefault(e => e.IATACode.ToUpper() == item.IATACode.ToUpper());
                                        if (airport != null)
                                        {
                                            int id = airport.Id;
                                            _objectMapper.Map(item, airport);
                                            airport.Id = id;
                                            airport.IATACode = item.IATACode.ToUpper();
                                            airport.LastModificationTime = Clock.Now;
                                            await _repository.InsertOrUpdateAndGetIdAsync(airport);
                                            await _unitOfWorkManager.Current.SaveChangesAsync();
                                        }
                                    }
                                }
                                else
                                {
                                    var airport = _objectMapper.Map<Airport>(item);
                                    airport.IATACode = item.IATACode.ToUpper();
                                    if (airport.ICAOCode == null)
                                        airport.ICAOCode = airport.IATACode;
                                    airport.CreationTime = Clock.Now;
                                    airport.CreatorUserId = 2;
                                    await _repository.InsertOrUpdateAndGetIdAsync(airport);
                                    await _unitOfWorkManager.Current.SaveChangesAsync();
                                }
                                unitOfWork.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            context.WriteLine($"Error in InsertOrUpdate {item.IATACode} : {ex?.InnerException?.Message ?? ex?.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.WriteLine("Error in AirportsJob");
                context.WriteLine($"{ex?.InnerException?.Message ?? ex?.Message}");
            }

            context.WriteLine("End AirportsJob");
        }
    }
}