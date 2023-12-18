namespace API.Gateway.Domain.Interfaces.Helpers
{
	public interface ICacheHelper
    {
        T? Get<T>(string key);
		void Set<T>(string key, T data, int AbsoluteExpInM, int SlidingExpInM);
		void Remove(string key);
	}
}