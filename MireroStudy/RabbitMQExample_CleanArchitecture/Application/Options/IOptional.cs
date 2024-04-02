using Microsoft.Extensions.Options;

namespace Application.Options;

public interface IOptional<out T> : IOptions<T> where T : class, new()
{
    void Update(Action<T> applyChanges);
}
