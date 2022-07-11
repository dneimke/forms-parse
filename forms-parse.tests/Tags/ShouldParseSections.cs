using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.SimpleTags
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
            _form!.Groups.Count.ShouldBe(2);
            
            var group = _form.Groups[1];
            _form.CurrentGroup.ShouldBe(group);
            group.Columns.Count.ShouldBe(1);
        }
    }
}