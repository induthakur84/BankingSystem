using System;

namespace ProjectCommonCode.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message) : base(message) { }
        public UserNotFoundException(int id) : base($"User with ID {id} was not found.") { }
    }
}
