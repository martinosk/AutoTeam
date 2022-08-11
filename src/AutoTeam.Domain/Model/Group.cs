using System;
using System.Linq;
using System.Collections.Generic;

namespace AutoTeam.Domain.Model
{
    public class Group
    {
        private HashSet<Student> currentMembers;
        private readonly List<Capacity> groupCapacity;
        private readonly List<IEnumerable<Student>> groupMemberHistory;
        
        public IEnumerable<Student> CurrentMembers { get { return currentMembers; } }
        
        public IEnumerable<IEnumerable<Student>> GroupMemberHistory { get { return groupMemberHistory; } }

        public IEnumerable<Capacity> GroupCapacity { get { return groupCapacity; } }

        public Group()
        {
            currentMembers = new HashSet<Student>();
            groupCapacity = new List<Capacity>();
            groupMemberHistory = new List<IEnumerable<Student>>();
        }

        public void AddCapacity(Capacity capacity)
        {
            if (capacity == null)
                throw new ArgumentNullException(nameof(capacity));
            groupCapacity.Add(capacity);
        }

        public void RemoveCapacity(Capacity capacity)
        {
            if (capacity == null)
                throw new ArgumentNullException(nameof(capacity));
            groupCapacity.Remove(capacity);
        }
        
        public void AddMember(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            if (!HasAvailableCapacity(student.Classification))
                throw new ArgumentException("There is no available capacity for the classification " + student.Classification + " in the group", nameof(student));
            currentMembers.Add(student);
            student.SetCurrentGroup(this);
        }

        public bool HasAvailableCapacity(Classification classification)
        {
            if (classification == null)
                throw new ArgumentNullException(nameof(classification));
            return RemainingCapacity(classification) > 0;
        }

        public int RemainingCapacity(Classification classification)
        {
            if (classification == null)
                throw new ArgumentNullException(nameof(classification));
            return GroupCapacity.Where(f => f.Classification == classification).Sum(f => f.Max) - currentMembers.Count(f => f.Classification == classification);
        }

        public void RemoveMember(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            currentMembers.Remove(student);
            student.RemoveFromCurrentGroup();
        }

        public void AcceptGroup()
        {
            groupMemberHistory.Add(CurrentMembers);
            foreach (var member in CurrentMembers)
                member.RemoveFromCurrentGroup();
            currentMembers = new HashSet<Student>();
        }
    }
}
