using Asjc.Utils.Extensions;

namespace Asjc.Natex.Matchers
{
    /// <summary>
    /// A NatexMatcher that returns <see langword="true"/> eternally when the pattern is "*" or "any".
    /// </summary>
    public class AnythingMatcher : NatexMatcher
    {
        /// <inheritdoc/>
        public override Func<object, bool?>? Create(Natex natex)
        {
            if (natex.Pattern.Equals("*", natex.CaseInsensitive) ||
                natex.Pattern.Equals("any", natex.CaseInsensitive))
                return _ => true;
            return null;
        }
    }
}
