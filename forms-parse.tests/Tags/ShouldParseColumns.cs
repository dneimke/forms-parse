using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.SimpleTags
{
    public class ShouldParseColumns : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfigurationWithAMultipleColumns()
        {
            _config = string.Format("a{0}|#(b, Red)", Environment.NewLine);
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
            group.Columns.Count.ShouldBe(2);

            var buttons1 = group.Columns[0].Items;
            buttons1.Count.ShouldBe(1);

            var buttons2 = group.Columns[1].Items;
            buttons2.Count.ShouldBe(1);

            buttons1[0].Name.ShouldBe("a");
            buttons1[0].Color.ShouldBe(KnownColors.Default);

            buttons2[0].Name.ShouldBe("b");
            buttons2[0].Color.ShouldBe(KnownColors.Red);
        }
    }
}