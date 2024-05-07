public interface IWeapon
{
    void Attack();
}

public class BastardSword : IWeapon
{
    public void Attack() => System.Console.WriteLine("대검으로 공격");
}

public class Dagger : IWeapon
{
    public void Attack() => System.Console.WriteLine("단검으로 공격");
}

public abstract class WeaponDecorator : IWeapon
{
    public abstract void Attack();
}

public class Poision : WeaponDecorator
{
    private readonly IWeapon _weapon;

    public Poision(IWeapon weapon) => _weapon = weapon;

    public override void Attack()
    {
        System.Console.Write("독묻은 ");
        _weapon.Attack();
    }
}

public class Spike : WeaponDecorator
{
    private readonly IWeapon _weapon;

    public Spike(IWeapon weapon) => _weapon = weapon;

    public override void Attack()
    {
        System.Console.Write("대못박힌 ");
        _weapon.Attack();
    }
}

public static class DecoratorTest
{
    public static void Test()
    {
        IWeapon weapon1 = new Poision(new BastardSword());
        weapon1.Attack();

        IWeapon weapon2 = new Spike(new Dagger());
        weapon2.Attack();

        IWeapon weapon3 = new Poision(new Spike(new BastardSword()));
        weapon3.Attack();

        IWeapon weapon4 = new Spike(new Poision(new Dagger()));
        weapon4.Attack();
    }
}