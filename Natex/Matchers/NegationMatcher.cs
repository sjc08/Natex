
namespace Asjc.Natex.Matchers
{
    /// <summary>
    /// A NatexMatcher for handling logical negation operator ("!").
    /// </summary>
    public class NegationMatcher : INatexMatcher
    {
        /// <inheritdoc/>
        public Func<object?, bool?>? Create(Natex natex)
        {
            if (natex.Pattern.StartsWith('!'))
            {
                Natex subNatex = new(natex.Pattern[1..], natex);
                return value => !subNatex.Match(value);
            }
            return null;
        }
    }
}
