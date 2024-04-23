﻿using Asjc.Extensions;

namespace Asjc.Natex.Matchers
{
    public class ComparisonMatcher : NatexMatcher<IComparable>
    {
        public override NatexMatchResult Match(IComparable value, Natex natex)
        {
            string pattern = natex.Pattern;
            if (pattern.StartsWith(">="))
            {
                var obj = pattern[2..].ChangeType(value.GetType());
                return obj != null && value.CompareTo(obj) >= 0 ? NatexMatchResult.Success : NatexMatchResult.Failure;
            }
            if (pattern.StartsWith('≥'))
            {
                var obj = pattern[1..].ChangeType(value.GetType());
                return obj != null && value.CompareTo(obj) >= 0 ? NatexMatchResult.Success : NatexMatchResult.Failure;
            }
            if (pattern.StartsWith('>'))
            {
                var obj = pattern[1..].ChangeType(value.GetType());
                return obj != null && value.CompareTo(obj) > 0 ? NatexMatchResult.Success : NatexMatchResult.Failure;
            }
            if (pattern.StartsWith("<="))
            {
                var obj = pattern[2..].ChangeType(value.GetType());
                return obj != null && value.CompareTo(obj) <= 0 ? NatexMatchResult.Success : NatexMatchResult.Failure;
            }
            if (pattern.StartsWith('≤'))
            {
                var obj = pattern[1..].ChangeType(value.GetType());
                return obj != null && value.CompareTo(obj) <= 0 ? NatexMatchResult.Success : NatexMatchResult.Failure;
            }
            if (pattern.StartsWith('<'))
            {
                var obj = pattern[1..].ChangeType(value.GetType());
                return obj != null && value.CompareTo(obj) < 0 ? NatexMatchResult.Success : NatexMatchResult.Failure;
            }
            return NatexMatchResult.Default;
        }
    }
}
