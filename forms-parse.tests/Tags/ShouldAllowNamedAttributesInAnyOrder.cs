using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.SimpleTags
{
    public class ShouldAllowNamedAttributesInAnyOrder : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfiguration()
        {
            _config = string.Format("#(D, Blue, switch){0}#(name: D, type: switch, color: Blue)", 
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

            buttons[0].Name.ShouldBe("D");
            buttons[0].Type.ShouldBe(nameof(Switch));
            buttons[0].Color.ShouldBe(KnownColors.Blue);

            buttons[1].Name.ShouldBe("D");
            buttons[1].Type.ShouldBe(nameof(Switch));
            buttons[1].Color.ShouldBe(KnownColors.Blue);

        }
    }
}