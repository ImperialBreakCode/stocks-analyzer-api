using API.Settlement.Domain.DTOs.Response.FinalizeDTOs;
using API.Settlement.Domain.Enums;

namespace API.Settlement.Domain.Interfaces.HelpersInterfaces
{
	public interface IConstantsHelperWrapper
	{
		ICommissionConstants CommissionConstants { get; }
		IJobInitializationFlags JobInitializationFlags { get; }
		IMessageConstants MessageConstants { get; }
		IRouteConstants RouteConstants { get; }
	}
}