using FormsParse;
using FormsParse.Models;
using Shouldly;

namespace forms_parse.tests.SimpleTags
{
    public class ConnectionsMustHaveAType : ContainerTestBase
    {
        string _config = "";
        FormDefinition? _form;
        ApplicationException _ex;

        void GivenAConfigurationWithAnInvalidConnection()
        {
            _config = string.Format("#(name: a, tag: 1){0}#(name: b, tag: 2){0}- connections{0}#(source: 1, target: 2)", Environment.NewLine);
        }

        void WhenTheConfigurationIsParsed()
        {
            try
            {
                var parser = new FormParser();
                _form = parser.Parse(_config);
            }
            catch(ApplicationException ex) 
            { 
                _ex = ex;
            }

        }

        void ThenAnApplicationExceptionShouldBeThrown()
        {
            _ex.ShouldNotBeNull();
        }
    }
}