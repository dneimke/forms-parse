using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.Buttons
{
    public class ShouldParseNamesWithSpaces : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfiguration()
        {
            _config = string.Format("#(name: A and B, type: label){0}#(A and B)",
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
            buttons.Count.ShouldBe(2);

            buttons[0].Name.ShouldBe("A and B");
            buttons[1].Name.ShouldBe("A and B");

        }
    }
}