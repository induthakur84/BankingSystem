using System;

namespace ProjectCommonCode.Exceptions
{
    public class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(string message) : base(message) { }
        public ProductNotFoundException(int id) : base($"Product with ID {id} was not found.") { }
    }
}
