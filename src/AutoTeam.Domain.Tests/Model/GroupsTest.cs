using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using System.Text;
using System.Threading.Tasks;
using AutoTeam.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTeam.Tests.Model
{
    [TestClass]
    public class GroupsTest : TestBase<Groups>
    {
        [TestMethod]
        public void AddGroupTest()
        {
            var group = fixture.Create<Group>();
            sut.AddGroup(group);
            Assert.AreEqual(1, sut.Count(f => f == group));
        }

        [TestMethod]
        public void AddSameGroupTwiceTest()
        {
            var group = fixture.Create<Group>();
            sut.AddGroup(group);
            sut.AddGroup(group);
            Assert.AreEqual(1, sut.Count(f => f == group));
        }

        [TestMethod]
        public void RemoveGroupTest()
        {
            var group = fixture.Create<Group>();
            sut.AddGroup(group);
            sut.RemoveGroup(group);
            Assert.IsFalse(sut.Contains(group));
        }
    }
}
