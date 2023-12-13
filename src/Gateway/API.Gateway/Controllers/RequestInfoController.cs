using API.Gateway.Domain.Entities;
using API.Gateway.Domain.Interfaces;
using API.Gateway.Infrastructure.Services.MongoDB;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    [Controller]
	[Route("api/[controller]")]
	public class RequestInfoController : Controller
	{
        private readonly IRequestService _requestService;
        public RequestInfoController(IRequestService requestService)
        {
			_requestService = requestService;
        }


		[HttpGet]
		[Route("ThisRouteInfoForLast24H")]
		public async Task<string> GetRouteInfo(string route)
		{

			return await _requestService.GetRouteStatisticsLast24Hours(route);
		}

		[HttpGet]
		[Route("RouteInfoForLast24H")]
		public async Task<string> GetThisRouteInfo()
		{

			return await _requestService.GetRouteStatisticsLast24Hours();
		}


	}
}
