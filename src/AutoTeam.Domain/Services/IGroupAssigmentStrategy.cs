using AutoTeam.Domain.Model;
using System;
using System.Linq;

namespace AutoTeam.Domain.Service
{
    public interface IGroupAssigmentStrategy
    {
        void AssignStudent(Groups groups, Student student);
    }

    public class AssignToFirstAvailableGroup : IGroupAssigmentStrategy
    {
        public void AssignStudent(Groups groups, Student student)
        { 
            var availableGroup =
                groups.FirstOrDefault(f => f.HasAvailableCapacity(student.Classification));
            if (availableGroup == null)
                throw new InvalidOperationException("No groups with available capacity found");
            availableGroup.AddMember(student);
        }
    }

}
