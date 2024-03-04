using Client.Domain.Count;
using Client.Domain.Count.Action;
using DevExpress.Mvvm.CodeGenerators;
using Fluxor;

namespace Client.Business
{
    [GenerateViewModel]
    public partial class View2ViewModel
    {
        public readonly IState<CountState> CountState;
        public readonly IDispatcher Dispatcher;

        [GenerateProperty]
        int _Count;

        public View2ViewModel(IDispatcher dispatcher, IState<CountState> counterState)
        {
            Dispatcher = dispatcher;
            CountState = counterState;
            Count = CountState.Value.Number;
            CountState.StateChanged += (s, e) => Count = CountState.Value.Number;
        }

        [GenerateCommand]
        void Increse() => Dispatcher.Dispatch(new IncreseAction());

        [GenerateCommand]
        void Decrease() => Dispatcher.Dispatch(new DecreaseAction());
    }
}
