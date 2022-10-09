using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.SimpleTags
{
    public class ShouldParseConnections : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfigurationWithTwoConnections()
        {
            _config = string.Format("#(name: a, tag: 1){0}#(name: b, tag: 2){0}- connections{0}#(type: deactivate, source: 1, target: 2){0}#(type: activate, source: 2, target: 1)", Environment.NewLine);
        }

        void WhenTheConfigurationIsParsed()
        {
            var parser = new FormParser();
            _form = parser.Parse(_config);
        }

        void ThenTwoConnectionsShouldBeAvailable()
        {
            _form!.Connections.Count.ShouldBe(2);
            _form.Connections[0].Type.ShouldBe(nameof(DeactivateConnection));
            _form.Connections[1].Type.ShouldBe(nameof(ActivateConnection));
        }
    }
}