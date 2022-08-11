using System;
using AutoTeam.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTeam.Tests.Model
{
    [TestClass]
    public class StudentTest : TestBase<Student>
    {
        [TestMethod]
        public void ToStringReturnsName()
        {
            var name = Guid.NewGuid().ToString();
            var sut = Student.WithName(name).WithClassification(Classification.Create(ClassificationEnum.Female));
            Assert.AreEqual(name, sut.ToString());
        }

        [TestMethod]
        public void EqualsChecksTheStudentId()
        {
            var sut2 = new Student(Guid.NewGuid().ToString(), Classification.Create(ClassificationEnum.Female), sut.Id);
            Assert.AreEqual(sut, sut2);
        }
    }
}
