using Datwise.Contracts;

namespace Datwise.Services
{
    public interface IExampleService
    {
        ExampleDto? GetExample(int id);
    }
}
