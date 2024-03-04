namespace Client.Business.Domain.ServiceInterface;
public interface ICountService
{
    int Increase(int count);
    int Decrease(int count);
}
