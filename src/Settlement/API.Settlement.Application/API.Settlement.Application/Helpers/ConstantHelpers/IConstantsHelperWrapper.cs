using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;

namespace API.Settlement.Application.Helpers.ConstantHelpers
{
	public class ConstantsHelperWrapper : IConstantsHelperWrapper
	{
		public ICommissionConstants CommissionConstants { get; }
		public IJobInitializationFlags JobInitializationFlags { get; }
		public IMessageConstants MessageConstants { get; }
		public IRouteConstants RouteConstants { get; }
		public ConstantsHelperWrapper(ICommissionConstants commissionConstants, 
									  IJobInitializationFlags jobInitializationFlags, 
									  IMessageConstants messageConstants, 
									  IRouteConstants routeConstants)
		{
			CommissionConstants = commissionConstants;
			JobInitializationFlags = jobInitializationFlags;
			MessageConstants = messageConstants;
			RouteConstants = routeConstants;
		}

		
	}
}