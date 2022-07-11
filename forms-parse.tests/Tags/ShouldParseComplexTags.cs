using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.SimpleTags
{
    public class ShouldParseComplexTags : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfigurationWithAComplexTag()
        {
            _config = string.Format("a{0}#(b, Red)", Environment.NewLine);
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
            _form!.Groups.Count.ShouldBe(1);
            
            var group = _form.Groups[0];
            _form.CurrentGroup.ShouldBe(group);
            group.Columns.Count.ShouldBe(1);

            var buttons = group.Columns[0].Items;
            buttons.Count.ShouldBe(2);

            buttons[0].Name.ShouldBe("a");
            buttons[0].Color.ShouldBe(KnownColors.Default);

            buttons[1].Name.ShouldBe("b");
            buttons[1].Color.ShouldBe(KnownColors.Red);
        }
    }
}