using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.Attributes
{
    public class ShouldAllowArbitraryAttributes : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfiguration()
        {
            _config = string.Format("#(name: a, type: event, foo: foo){0}#(name: b, bar: bar)", Environment.NewLine);
        }

        void WhenTheConfigurationIsParsed()
        {
            var parser = new FormParser();
            _form = parser.Parse(_config);
        }

        void ThenArbitraryAttributesShouldBeAvailable()
        {
            var group = _form!.Rows[0];
            var buttons = group.Columns[0].Items;

            buttons[0].Attributes.TryGetValue("foo", out var _).ShouldBeTrue();
            buttons[0].Attributes.TryGetValue("bar", out var _).ShouldBeFalse();

            buttons[1].Attributes.TryGetValue(key: "bar", out var _).ShouldBeTrue();
            buttons[1].Attributes.TryGetValue("foo", out var _).ShouldBeFalse();
        }
    }
}