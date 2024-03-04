using Client.Business.Domain.ServiceInterface;
using Client.Domain.Count;
using Client.Domain.Count.Action;
using Fluxor;

namespace Client.Reduce;

public class CountReducer
{
    private readonly ICountService CountService;

    public CountReducer(ICountService countService)
    {
        CountService = countService;
    }

    [ReducerMethod]
    public CountState ReduceIncreaseCounterAction(CountState state, IncreseAction action) =>
        new(number: state.Number + 1);

    [ReducerMethod(typeof(DecreaseAction))]
    public CountState ReduceDecreaseCounterAction(CountState state) =>
        new(number: CountService.Decrease(state.Number));
}