using System.Text.RegularExpressions;

namespace FormsParse
{
    public class AttributeParser
    {
        public static Dictionary<string, string> Parse(string template)
        {
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentException($"'{nameof(template)}' cannot be null or empty.", nameof(template));
            }

            var usingNamedAttributes = false;
            var attributes = new Dictionary<string, string>();
            var arr = template.Split(',', StringSplitOptions.TrimEntries);
            var idx = 0;
            var prefix_regex = new Regex(@"^((?'name'[a-z]+)\:[ ]?)?(?'val'.+)$", RegexOptions.Singleline, TimeSpan.FromMilliseconds(200));

            foreach (var attribute in arr)
            {
                var result = prefix_regex.Match(attribute);
                if (result.Success)
                {
                    var val = result.Groups["val"].Value;

                    if (result.Groups["name"].Success)
                    {
                        var name = result.Groups["name"].Value;
                        usingNamedAttributes = true;

                        attributes.Add(name, val);
                    }
                    else
                    {
                        if (usingNamedAttributes)
                        {
                            throw new ApplicationException("If using named attributes, all attributes must be named.");
                        }

                        if (idx == 0) attributes.Add("name", val);
                        if (idx == 1) attributes.Add("color", val);
                        if (idx == 2) attributes.Add("type", val);
                        if (idx == 3) attributes.Add("tag", val);
                    }
                }

                idx++;
            }

            return attributes;
        }
    }
}
