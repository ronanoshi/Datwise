using Datwise.Models;

namespace Datwise.Data
{
    public class ExampleRepository
    {
        public ExampleModel? Get(int id)
        {
            // In-memory stub
            if (id == 1)
            {
                return new ExampleModel { Id = 1, Name = "FromRepository" };
            }
            return null;
        }
    }
}
