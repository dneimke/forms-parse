using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.Buttons
{
    public class ShouldParseUnknownColors : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfigurationWithTwoButtons()
        {
            _config = string.Format("a{0}#(b, NotAKnownColor)", Environment.NewLine);
        }

        void WhenTheConfigurationIsParsed()
        {
            var parser = new FormParser();
            _form = parser.Parse(_config);
        }

        void ThenAFormIsCreated()
        {
            _form.ShouldNotBeNull();
        }

        void AndIsDefinedCorrectly()
        {
            _form!.Rows.Count.ShouldBe(1);

            var group = _form.Rows[0];
            _form.CurrentRow.ShouldBe(group);
            group.Columns.Count.ShouldBe(1);

            var buttons = group.Columns[0].Items;
            buttons.Count.ShouldBe(2);

            buttons[0].Name.ShouldBe("a");
            buttons[0].Color.ShouldBe(KnownColors.Default);

            buttons[1].Name.ShouldBe("b");
            buttons[1].Color.ShouldBe("NotAKnownColor");
        }
    }
}