using Fluxor;

namespace Client.Domain.Count;

[FeatureState]
public class CountState
{
    public int Number { get; }

    private CountState() { }

    public CountState(int number)
    {
        Number = number;
    }
}