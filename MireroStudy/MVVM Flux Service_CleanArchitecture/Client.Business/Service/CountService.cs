using Client.Business.Domain.ServiceInterface;

namespace Client.Service;

public class CountService : ICountService
{
    public int Decrease(int count)
    {
        return count - 1;
    }

    public int Increase(int count)
    {
        return count + 1;
    }
}
