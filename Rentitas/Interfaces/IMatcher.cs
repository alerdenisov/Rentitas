using System;

namespace Rentitas
{
    public interface IMatcher
    {
        Type[] Types { get; }
        bool Matches(IEntity entity);
    }

    public interface ICompoundMatcher : IMatcher
    {
        Type[] AllOfTypes { get; }
        Type[] AnyOfTypes { get; }
        Type[] NoneOfTypes { get; }
    }

    public interface IAllOfMatcher : ICompoundMatcher
    {
        IAnyOfMatcher AnyOf(params Type[] types);
        IAnyOfMatcher AnyOf(params IMatcher[] matchers);
        INoneOfMatcher NoneOf(params Type[] types);
        INoneOfMatcher NoneOf(params IMatcher[] matchers);
    }

    public interface IAnyOfMatcher : ICompoundMatcher
    {
        INoneOfMatcher NoneOf(params Type[] types);
        INoneOfMatcher NoneOf(params IMatcher[] matchers);
    }

    public interface INoneOfMatcher : ICompoundMatcher
    {
    }
}