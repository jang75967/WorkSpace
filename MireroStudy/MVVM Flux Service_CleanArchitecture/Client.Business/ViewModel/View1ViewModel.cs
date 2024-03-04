using Client.Domain.Count;
using DevExpress.Mvvm.CodeGenerators;
using Fluxor;

namespace Client.Business
{
    [GenerateViewModel]
    public partial class View1ViewModel
    {
        public readonly IState<CountState> CountState;

        [GenerateProperty]
        int _Count;

        public View1ViewModel(IState<CountState> counterState)
        {
            CountState = counterState;
            Count = CountState.Value.Number;
            CountState.StateChanged += (s, e) => Count = CountState.Value.Number;
        }
    }
}
