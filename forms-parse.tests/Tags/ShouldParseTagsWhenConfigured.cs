using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.SimpleTags
{
    public class ShouldParseTagsWhenConfigured : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfigurationWithTags()
        {
            _config = string.Format("#(name: a, type: event, foo: foo, tag: xxx)", Environment.NewLine);
        }

        void WhenTheConfigurationIsParsed()
        {
            var parser = new FormParser();
            _form = parser.Parse(_config);
        }

        void ThenTheTagShouldBeAvailable()
        {
            var group = _form!.Rows[0];
            var buttons = group.Columns[0].Items;
            
            buttons[0].Attributes.TryGetValue("tag", out var _).ShouldBeTrue();
            buttons[0].Tag.ShouldBe("xxx");
        }
    }
}