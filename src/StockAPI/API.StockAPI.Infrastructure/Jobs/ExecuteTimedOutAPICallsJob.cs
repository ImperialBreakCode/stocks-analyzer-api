using API.StockAPI.Domain.InterFaces;
using API.StockAPI.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.StockAPI.Infrastructure.Jobs
{
    public class ExecuteTimedOutAPICallsJob : IJob
    {
        private readonly ITimedOutCallServices _callServices;
        public ExecuteTimedOutAPICallsJob(ITimedOutCallServices callServices)
        {
            _callServices = callServices;
        }
        public Task Execute(IJobExecutionContext context)
        {

            var calls = _callServices.GetFailedCallsFromDB().Result;

            if(!calls.Any())
            {
                Console.WriteLine("No backed up calls detected");
                return Task.CompletedTask;
            }

            foreach (var call in calls)
            {
                var result = _callServices.RecallFailedCall(call);
                if(result is null)
                {
                    break;
                }
                _callServices.DeleteFailedCallInDB(call);
            }

            return Task.CompletedTask;
        }
    }
}
