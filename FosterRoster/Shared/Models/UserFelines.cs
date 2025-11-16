namespace FosterRoster.Shared.Models;

public abstract record UserFelines
{
    public abstract bool Has(int felineId);
    public static readonly UserFelines None = new UserFelinesNone();
    public static readonly UserFelines All = new UserFelinesAll();
    public static UserFelines Some(HashSet<int> felineIds) => new UserFelinesSome(felineIds);
}

/// <summary>
///     Filter when user has access to all felines.
/// </summary>
public sealed record UserFelinesAll : UserFelines
{
    public override bool Has(int felineId) => true;
}

/// <summary>
///     Filter when user has access to no felines.
/// </summary>
public sealed record UserFelinesNone : UserFelines
{
    public override bool Has(int felineId) => false;
}

/// <summary>
///     Filter when user has access to some felines.
/// </summary>
/// <param name="FelineIds"></param>
public sealed record UserFelinesSome(HashSet<int> FelineIds) : UserFelines
{
    public override bool Has(int felineId) => FelineIds.Contains(felineId);
}
