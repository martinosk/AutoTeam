using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoTeam.Domain.Model;
using AutoTeam.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTeam.Tests.Service
{
    [TestClass]
    public class AssignToBestGroupTest : TestBase<AssignToBestGroup>
    {
        Groups groups;
        Student sutStudent;
        Student f1;
        Student f2;
        Student m1;
        Student m2;
        Group groupA;
        Group groupB;

        [TestInitialize]
        public void InitClassroomAndGroups()
        {
            groups = new Groups(new AssignToBestGroup());

            var fClassification = Classification.Create(ClassificationEnum.Female);
            var mClassification = Classification.Create(ClassificationEnum.Male);
            sutStudent = new Student("Sut", mClassification, Guid.NewGuid());
            f1 = new Student("Anne", fClassification, Guid.NewGuid());
            f2 = new Student("Grete", fClassification, Guid.NewGuid());
            m1 = new Student("Hans", mClassification, Guid.NewGuid());
            m2 = new Student("Ole", mClassification, Guid.NewGuid());

            var fCapacity = new Capacity(fClassification, 2);
            var mCapacity = new Capacity(mClassification, 2);

            groupA = fixture.Create<Group>();
            groups.AddGroup(groupA);
            groupA.AddCapacity(fCapacity);
            groupA.AddCapacity(mCapacity);

            groupB = fixture.Create<Group>();
            groups.AddGroup(groupB);
            groupB.AddCapacity(fCapacity);
            groupB.AddCapacity(mCapacity);
        }

        [TestMethod]
        public void StudentIsAssignedToTheGroupIfHeHasNeverBeenWithCurrentMembers()
        {
            groupA.AddMember(f1);
            groupA.AddMember(m2);
            groupA.AddMember(sutStudent);
            groupA.AcceptGroup();

            groupA.AddMember(f1);
            groupA.AddMember(f2);
            groupA.AddMember(m1);
            groupA.AcceptGroup();

            groupA.AddMember(f1);
            groupA.AddMember(f2);
            groupA.AddMember(sutStudent);
            groupA.AcceptGroup();

            groupB.AddMember(f1);
            groupB.AddMember(m2);
            groupB.AddMember(sutStudent);
            groupB.AcceptGroup();

            groupB.AddMember(f2);
            groupB.AddMember(m2);
            groupB.AddMember(sutStudent);
            groupB.AcceptGroup();

            groupA.AddMember(m1);

            groupB.AddMember(m1); 
            groupB.AddMember(f2);

            Assert.IsFalse(groupA.CurrentMembers.Contains(sutStudent));
            Assert.IsFalse(groupB.CurrentMembers.Contains(sutStudent));

            sut.AssignStudent(groups, sutStudent);

            Assert.IsFalse(groupB.CurrentMembers.Contains(sutStudent));
            Assert.IsTrue(groupA.CurrentMembers.Contains(sutStudent));
        }
        [TestMethod]
        public void StudentIsAssignedToTheGroupWhereHeHasNotBeenWithAnyOfTheCurrentMembers()
        {
            groupA.AddMember(f1); 
            groupA.AddMember(m2); 
            groupA.AddMember(sutStudent);
            groupA.AcceptGroup();

            groupA.AddMember(f1);
            groupA.AddMember(f2); 
            groupA.AddMember(sutStudent);
            groupA.AcceptGroup();

            groupB.AddMember(f1); 
            groupB.AddMember(m1); 
            groupB.AddMember(sutStudent);
            groupB.AcceptGroup();

            groupB.AddMember(f1);
            groupB.AddMember(m2);
            groupB.AddMember(sutStudent);
            groupB.AcceptGroup();

            groupA.AddMember(m2); // student has been with m2 2 times before
            groupA.AddMember(f1); // student has been with f1 4 times before

            groupB.AddMember(m1); // student has been with m1 1 time before
            groupB.AddMember(f2); // student has been with f2 1 time before

            Assert.IsFalse(groupA.CurrentMembers.Contains(sutStudent));
            Assert.IsFalse(groupB.CurrentMembers.Contains(sutStudent));

            sut.AssignStudent(groups, sutStudent);

            Assert.IsFalse(groupA.CurrentMembers.Contains(sutStudent));
            Assert.IsTrue(groupB.CurrentMembers.Contains(sutStudent));
        }

        [TestMethod]
        public void IfSeveralGroupsScoresTheSameTheStudentIsAssignetToTheOneWhereHeHasBeenTheLeast1()
        {
            groupA.AddMember(f1);
            groupA.AddMember(m2);
            groupA.AcceptGroup();
            
            groupB.AddMember(f1);
            groupB.AddMember(m2);
            groupB.AddMember(sutStudent);
            groupB.AcceptGroup();
            
            groupA.AddMember(f2);
            groupA.AddMember(m1);

            groupB.AddMember(f2);
            groupB.AddMember(m1);

            Assert.IsFalse(groupA.CurrentMembers.Contains(sutStudent));
            Assert.IsFalse(groupB.CurrentMembers.Contains(sutStudent));

            sut.AssignStudent(groups, sutStudent);

            Assert.IsTrue(groupA.CurrentMembers.Contains(sutStudent));
            Assert.IsFalse(groupB.CurrentMembers.Contains(sutStudent));
        }

        [TestMethod]
        public void IfSeveralGroupsScoresTheSameTheStudentIsAssignetToTheOneWhereHeHasBeenTheLeast2()
        {
            groupA.AddMember(f1);
            groupA.AddMember(m2);
            groupA.AddMember(sutStudent);
            groupA.AcceptGroup();

            groupB.AddMember(f1);
            groupB.AddMember(m2);
            groupB.AcceptGroup();

            groupA.AddMember(f2);
            groupA.AddMember(m1);

            groupB.AddMember(f2);
            groupB.AddMember(m1);

            Assert.IsFalse(groupA.CurrentMembers.Contains(sutStudent));
            Assert.IsFalse(groupB.CurrentMembers.Contains(sutStudent));

            sut.AssignStudent(groups, sutStudent);

            Assert.IsFalse(groupA.CurrentMembers.Contains(sutStudent));
            Assert.IsTrue(groupB.CurrentMembers.Contains(sutStudent));
        }
    }
}
