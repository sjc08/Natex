﻿namespace Asjc.Natex.Matchers
{
    public class MultiPatternMatcher : NatexMatcher<IEnumerable<Natex>, object>
    {
        public override IEnumerable<Natex>? Parse(Natex natex)
        {
            var arr = natex.Pattern.Split(" ");
            return arr.Length > 1 ? arr.Select(p => new Natex(p, natex)) : null;
        }

        public override NatexMatchResult Match(object value, IEnumerable<Natex> data, Natex natex)
        {
            return data.All(n => n.Match(value)) ? NatexMatchResult.Success : NatexMatchResult.Default;
        }
    }
}
