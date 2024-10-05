using System;

public interface IHealth
{
    public void TakeDamage(int damage);

    public void Dead();

    public void SuscribeLifeChange(Action<float> action);

    public void UnsuscribeLifeChange(Action<float> action);

    public void SuscribeActionDeath(Action action);

    public void UnsuscribeDeath(Action action);
}

public interface IHealth<T> : IHealth
{
    public void SuscribeAction(Action<T> action);

    public void Unsuscribe(Action<T> action);
}