﻿using Microsoft.Extensions.Options;

namespace API.Gateway.Domain.Interfaces.Helpers
{
	public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
