using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.Forms
{
    public class ShouldParseSections : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfigurationWithAMultipleColumns()
        {
            _config = string.Format("a{0}--{0}#(b, Red)", Environment.NewLine);
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
            _form!.Rows.Count.ShouldBe(2);

            var group = _form.Rows[1];
            _form.CurrentRow.ShouldBe(group);
            group.Columns.Count.ShouldBe(1);
        }
    }
}