using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.Buttons
{
    public class ShouldParseAllTypes : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfiguration()
        {
            _config = string.Format("#(name: a, type: label){0}#(name: a, type: switch){0}#(name: a, type: button){0}#(name: a, type: unknown){0}",
                Environment.NewLine);
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

        void AndAllTypesAreCorrect()
        {
            _form!.Rows.Count.ShouldBe(1);

            var group = _form.Rows[0];

            var buttons = group.Columns[0].Items;
            buttons.Count.ShouldBe(4);

            buttons[0].Type.ShouldBe(nameof(Label));
            buttons[1].Type.ShouldBe(nameof(Switch));
            buttons[2].Type.ShouldBe(nameof(Button));
            buttons[3].Type.ShouldBe(nameof(Button));
        }
    }
}