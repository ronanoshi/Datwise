using Datwise.Contracts;
using Datwise.Models;

namespace Datwise.Services
{
    public class ExampleService : IExampleService
    {
        public ExampleDto? GetExample(int id)
        {
            // TODO: replace with data access and real implementation
            if (id == 1)
            {
                return new ExampleDto { Id = 1, Name = "Sample" };
            }
            return null;
        }
    }
}
