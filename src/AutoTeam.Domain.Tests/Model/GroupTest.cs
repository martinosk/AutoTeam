using System.Linq;
using AutoFixture;
using AutoTeam.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutoTeam.Tests.Model
{
    [TestClass]
    public class GroupTest : TestBase<Group>
    {
        Classification frozenClassification;
        [TestMethod]
        public void AddCapacityTest()
        {
            var capacity = fixture.Create<Capacity>();
            sut.AddCapacity(capacity);
            Assert.AreEqual(1, sut.GroupCapacity.Count(f => f == capacity));
        }

        [TestMethod]
        public void RemoveCapacityTest()
        {
            var capacity = fixture.Create<Capacity>();
            sut.AddCapacity(capacity);
            sut.RemoveCapacity(capacity);
            Assert.IsFalse(sut.GroupCapacity.Contains(capacity));
        }

        [TestMethod]
        public void AddStudentTest()
        {
            AddCapacity(10);
            var student = fixture.Create<Student>();
            sut.AddMember(student);
            Assert.AreEqual(1, sut.CurrentMembers.Count(f => f == student));
            Assert.AreEqual(sut, student.CurrentGroup);
        }

        private void AddCapacity(ushort max)
        {
            frozenClassification = fixture.Freeze<Classification>();
            var capacity = new Capacity(frozenClassification, max);
            sut.AddCapacity(capacity);
        }

        [TestMethod]
        public void AddSameStudentTest()
        {
            AddCapacity(10);
            var student = fixture.Create<Student>();
            sut.AddMember(student);
            sut.AddMember(student);
            Assert.AreEqual(1, sut.CurrentMembers.Count(f => f == student));
        }

        [TestMethod]
        public void RemoveStudentTest()
        {
            AddCapacity(10);
            var student = fixture.Create<Student>();
            sut.AddMember(student);
            sut.RemoveMember(student);
            Assert.IsFalse(sut.CurrentMembers.Contains(student));
            Assert.IsNull(student.CurrentGroup);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void AddStudentNoCapacityTest()
        {
            var classification1 = new Male();
            fixture.Inject(classification1);
            var capacity1 = new Capacity(classification1, 0);
            sut.AddCapacity(capacity1);

            var classification2 = new Male();
            var capacity2 = new Capacity(classification2, 1);
            sut.AddCapacity(capacity2);

            var student = fixture.Create<Student>();
            sut.AddMember(student);
        }

        [TestMethod]
        public void HasAvailableCapacityTest()
        {
            AddCapacity(2);
            Assert.IsTrue(sut.HasAvailableCapacity(frozenClassification));
            sut.AddMember(fixture.Create<Student>());
            Assert.IsTrue(sut.HasAvailableCapacity(frozenClassification));
        }

        [TestMethod]
        public void HasAvailableCapacityNoneTest()
        {
            AddCapacity(0);
            Assert.IsFalse(sut.HasAvailableCapacity(frozenClassification));
        }

        [TestMethod]
        public void CurrentMembersAreAddedToHistoryWhenGroupIsAccepted()
        {
            AddCapacity(10);
            var student = fixture.Create<Student>();
            sut.AddMember(student);
            var groupMembers = sut.CurrentMembers;
            sut.AcceptGroup();
            Assert.AreEqual(groupMembers, sut.GroupMemberHistory.Last());
            Assert.AreEqual(1, groupMembers.Count());
            Assert.AreEqual(0, sut.CurrentMembers.Count());
        }

        [TestMethod]
        public void MultipleGroupsInHistory()
        {
            AddCapacity(10);
            var student = fixture.Create<Student>();
            sut.AddMember(student);
            sut.AcceptGroup();
            sut.AddMember(student);
            sut.AddMember(fixture.Create<Student>());
            sut.AcceptGroup();
            Assert.AreEqual(2, sut.GroupMemberHistory.Count());
            Assert.AreEqual(2, sut.GroupMemberHistory.Last().Count());
            Assert.AreEqual(1, sut.GroupMemberHistory.First().Count());
        }
    }
}
