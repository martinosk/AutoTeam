using System.Linq;
using AutoTeam.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoFixture;

namespace AutoTeam.Tests.Service
{
    [TestClass]
    public class ClassroomAssignGroupsTest : TestBase<Classroom>
    {
        Classroom frozenClassroom;

        protected override void LocalInit()
        {
            // Freeze classification so that all students and group capacities has the same classification
            var frozenClassification = fixture.Freeze<Classification>();

            // Create the groups that the classroom should contain
            var groups = fixture.CreateMany<Group>(2);

            var totalGroupCapacity = 0;
            foreach (var group in groups)
            {
                group.AddCapacity(fixture.Create<Capacity>());
                totalGroupCapacity += group.GroupCapacity.Where(f => f.Classification == frozenClassification).Sum(f => f.Max);
            }

            var students = fixture.CreateMany<Student>(totalGroupCapacity);

            frozenClassroom = fixture.Freeze<Classroom>();

            foreach (var group in groups)
                frozenClassroom.Groups.AddGroup(group);

            frozenClassroom.AddStudents(students);

        }

        [TestMethod]
        public void AssignStudentsToGroupsAssigsAllStudentsToAGroup()
        {
            Assert.AreEqual(0, frozenClassroom.Groups.Sum(f=>f.CurrentMembers.Count()), "Some students already in a group");
            sut.AssignStudentsToGroups();
            Assert.AreEqual(frozenClassroom.Students.Count(), frozenClassroom.Groups.Sum(f => f.CurrentMembers.Count()), "Not all students were assigned to a group");
        }

        [TestMethod]
        public void AssignStudentsToGroupsOnlyAssigsUnassignedStudentsToAGroup()
        {
            var sutStudent = sut.Students.First();
            var originalGroup = sut.Groups.Last();
            originalGroup.AddMember(sutStudent);
            Assert.IsTrue(originalGroup.CurrentMembers.Contains(sutStudent));
            sut.AssignStudentsToGroups();
            Assert.IsTrue(originalGroup.CurrentMembers.Contains(sutStudent));
            Assert.AreEqual(frozenClassroom.Students.Count(), frozenClassroom.Groups.Sum(f => f.CurrentMembers.Count()), "Not all students were assigned to a group");
        }
        
        [TestMethod, ExpectedException(typeof(ClassroomGroupCapacityException))]
        public void AssignStudentsToGroupsThrowsExceptionIfThereAreTooManyStudents()
        {
            frozenClassroom.AddStudent(fixture.Create<Student>());
            sut.AssignStudentsToGroups();
        }

    }
}
