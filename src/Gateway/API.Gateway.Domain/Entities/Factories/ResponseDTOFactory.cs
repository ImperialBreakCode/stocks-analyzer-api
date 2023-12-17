using API.Gateway.Domain.DTOs;

namespace API.Gateway.Domain.Entities.Factories
{
	public class ResponseDTOFactory
	{
		public ResponseDTO Create(string message)
		{
			return new ResponseDTO
			{
				Message = message
			};
		}
	}
}
