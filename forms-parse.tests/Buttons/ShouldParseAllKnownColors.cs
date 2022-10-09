using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.Buttons
{
    public class ShouldParseAllKnownColors : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfiguration()
        {
            _config = string.Format("#(a, {1}){0}#(b, {2}){0}#(c, {3}){0}#(d, {4}){0}#(e, {5})",
                Environment.NewLine,
                KnownColors.Gray, KnownColors.Blue, KnownColors.Black, KnownColors.Green, KnownColors.Red);
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
            buttons.Count.ShouldBe(5);

            buttons[0].Name.ShouldBe("a");
            buttons[0].Color.ShouldBe(KnownColors.Gray);

            buttons[1].Name.ShouldBe("b");
            buttons[1].Color.ShouldBe(KnownColors.Blue);

            buttons[2].Name.ShouldBe("c");
            buttons[2].Color.ShouldBe(KnownColors.Black);

            buttons[3].Name.ShouldBe("d");
            buttons[3].Color.ShouldBe(KnownColors.Green);

            buttons[4].Name.ShouldBe("e");
            buttons[4].Color.ShouldBe(KnownColors.Red);
        }
    }
}