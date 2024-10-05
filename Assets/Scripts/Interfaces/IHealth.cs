using System;

public interface IHealth
{
    public void TakeDamage(int damage);

    public void Dead();

    public void SuscribeAction(Action action);

    public void Unsuscribe(Action action);
}

public interface IHealth<T> : IHealth
{
    public void SuscribeAction(Action<T> action);

    public void Unsuscribe(Action<T> action);
}