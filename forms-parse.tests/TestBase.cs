using TestStack.BDDfy;

namespace forms_parse.tests
{
    [Collection("Sequential")]
    public abstract class ContainerTestBase : IAsyncLifetime
    {
        [Fact]
        public virtual void ExecuteScenario() => this.BDDfy(GetType().Name);

        public Task DisposeAsync() => Task.CompletedTask;

        public Task InitializeAsync() => Task.CompletedTask;
    }
}
