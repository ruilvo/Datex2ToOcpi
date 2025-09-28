namespace Datex2ToOcpi.Core;

public interface IHasId
{
    int Id { get; }
}

public interface IHasCombineWith<T>
{
    void CombineWith(T other);
}
