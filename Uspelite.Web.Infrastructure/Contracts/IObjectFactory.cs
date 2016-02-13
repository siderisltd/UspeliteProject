namespace Uspelite.Web.Infrastructure.Contracts
{
    using System;

    public interface IObjectFactory
    {
        T GetInstance<T>();

        object GetInstance(Type type);

        T TryGetInstance<T>();

        object TryGetInstance(Type type);
    }
}
