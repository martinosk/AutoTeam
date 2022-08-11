using AutoTeam.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTeam.Domain.Service
{
    public class AssignToBestGroup : IGroupAssigmentStrategy
    {
        private readonly Random r;

        public AssignToBestGroup()
        {
            r = new Random();
        }

        public void AssignStudent(Groups groups, Student student)
        {
            if (groups == null)
                throw new ArgumentNullException(nameof(groups));
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            var availableGroups = groups.Where(f => f.HasAvailableCapacity(student.Classification));
            var ScoredGroups = new List<ScoredGroup>();

            // Loop all available groups
            foreach (var group in availableGroups.OrderBy(x => r.Next()))
            {
            
                var timesInGroupWithMembers = GetTimesInGroupWithMembers(groups, student, group);

                var timesInGroup = group.GroupMemberHistory.Count(f => f.Contains(student));

                // If student has never been in group with any of the current members, assign him straight away
                if (timesInGroupWithMembers == 0 && timesInGroup == 0)
                {
                    group.AddMember(student);
                    return;
                }

                ScoredGroups.Add(new ScoredGroup(timesInGroupWithMembers, timesInGroup, group));
            }

            var bestScore = ScoredGroups.OrderBy(f => f.Score).ThenBy(f => f.NumberOfTimesInGroup);
            bestScore.First().Group.AddMember(student);
        }

        private static int GetTimesInGroupWithMembers(Groups groups, Student student, Group group)
        {
            var timesInGroupWithMembers = 0;
            // Calculate how many times the student has been together with the other members
            foreach (var currentMember in group.CurrentMembers)
            {
                foreach (var groupAll in groups)
                {
                    foreach (var previousGroupMembers in groupAll.GroupMemberHistory.Where(f => f.Contains(student) && f.Contains(currentMember)))
                    {
                        timesInGroupWithMembers++;
                    }
                }
            }

            return timesInGroupWithMembers;
        }

        private class ScoredGroup
        {
            public ScoredGroup(int score, int timesInGroup, Group group)
            {
                this.Score = score;
                this.Group = group;
                this.NumberOfTimesInGroup = timesInGroup;
            }
            public int Score { get; }
            public Group Group { get; }
            public int NumberOfTimesInGroup { get; }
        }
    }
}
