using Client.Business.Domain.ViewInterface;
using Client.Domain.Count;
using Client.Domain.Count.Action;
using DevExpress.Mvvm.CodeGenerators;
using Fluxor;

namespace Client.Business;

[GenerateViewModel]
public partial class MainViewModel
{
    private readonly IServiceProvider _service;
    private readonly IStore Store;
    public readonly IDispatcher Dispatcher;
    public readonly IState<CountState> CountState;

    [GenerateProperty]
    int _Count;

    public MainViewModel(IServiceProvider service, IStore store, IDispatcher dispatcher, IState<CountState> counterState)
    {
        _service = service;
        Store = store;
        Dispatcher = dispatcher;
        CountState = counterState;

        Initialize();
    }

    [GenerateCommand]
    void Increse() => Dispatcher.Dispatch(new IncreseAction());

    [GenerateCommand]
    void Decrease() => Dispatcher.Dispatch(new DecreaseAction());

    [GenerateCommand]
    void ShowView1()
    {
        var view = _service.GetService(typeof(IView1Dialog)) as IView1Dialog;
        view.Show();
    }

    [GenerateCommand]
    void ShowView2()
    {
        var view = _service.GetService(typeof(IView2Dialog)) as IView2Dialog;
        view.Show();
    }

    #region Initialize
    private void Initialize()
    {
        SetChangeEvent();
        ValueInitialize();
        Store.InitializeAsync().Wait();
    }

    private void ValueInitialize()
    {
        Count = CountState.Value.Number;
    }

    private void SetChangeEvent()
    {
        CountState.StateChanged += CounterState_StateChanged;
    }
    #endregion

    private void CounterState_StateChanged(object? sender, EventArgs e)
    {
        Count = CountState.Value.Number;
    }
}
