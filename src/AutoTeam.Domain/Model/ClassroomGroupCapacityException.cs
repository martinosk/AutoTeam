using System;

namespace AutoTeam.Domain.Model
{
    public class ClassroomGroupCapacityException : Exception
    {
        public ClassroomGroupCapacityException(string message) : base(message)
        {
        }
    }
}