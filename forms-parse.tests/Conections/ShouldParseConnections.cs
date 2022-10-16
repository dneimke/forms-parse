using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.Conections
{
    public class ShouldParseConnections : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;

        void GivenAConfigurationWithTwoConnections()
        {
            _config = @"#(name: Button A, type: switch, color: Red, tag: 1)
#(name: Button B, type: switch, color: Blue, tag: 2)
- connections 
#(type: deactivate, source: 1, target: 2)
#(type: activate, source: 2, target: 1)";
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