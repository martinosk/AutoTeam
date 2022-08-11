using System;
using System.Collections;
using System.Collections.Generic;
using AutoTeam.Domain.Service;

namespace AutoTeam.Domain.Model
{
    public class Groups : IEnumerable<Group>
    {
        private readonly HashSet<Group> groups;
        private readonly IGroupAssigmentStrategy groupAssignmentStrategy;

        public Groups(IGroupAssigmentStrategy groupAssignmentStrategy)
        {
            this.groupAssignmentStrategy = groupAssignmentStrategy ?? throw new ArgumentNullException(nameof(groupAssignmentStrategy));
            groups = new HashSet<Group>();
        }

        public void AddGroup(Group group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            groups.Add(group);
        }

        public void RemoveGroup(Group group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));
            groups.Remove(group);
        }

        public void AssignStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            groupAssignmentStrategy.AssignStudent(this, student);
        }
        
        public IEnumerator<Group> GetEnumerator()
        {
            return ((IEnumerable<Group>)groups).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Group>)groups).GetEnumerator();
        }
    }
}
