﻿using System.Reflection;

namespace Asjc.Natex.Matchers
{
    /// <summary>
    /// A NatexMatcher for matching property of an object.
    /// </summary>
    public class PropertyMatcher : NatexMatcher
    {
        public List<string[]> DefaultPaths { get; set; } = [];

        /// <inheritdoc/>
        public override Func<object?, bool?>? Create(Natex natex)
        {
            // Prepare BindingFlags.
            var flags = BindingFlags.Instance | BindingFlags.Public;
            if (natex.CaseInsensitive) flags |= BindingFlags.IgnoreCase;
            // Parse the path and pattern.
            string[]? path = null;
            string pattern;
            var arr = natex.Pattern.Split(':', 2);
            if (arr.Length == 2 && !arr[0].Contains(' ')) // Perhaps properties that are clearly illegal should be ignored.
            {
                path = arr[0].Split('.');
                pattern = arr[1];
            }
            else
            {
                pattern = natex.Pattern;
            }
            // Create a Natex.
            natex = new(pattern, natex);
            return value =>
            {
                if (path == null)
                    return DefaultPaths.Any(Handle) ? true : null;
                else
                    return Handle(path);

                bool Handle(string[] path)
                {
                    object? obj = value;
                    foreach (var name in path)
                    {
                        if (obj == null) return false;
                        var info = obj.GetType().GetProperty(name, flags);
                        if (info == null) return false;
                        obj = info.GetValue(obj);
                    }
                    return natex.Match(obj);
                }
            };
        }
    }
}
