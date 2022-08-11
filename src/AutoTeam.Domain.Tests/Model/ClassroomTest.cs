using AutoTeam.Domain.Model;
using System.Linq;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTeam.Tests.Model
{
    [TestClass]
    public class ClassroomTest : TestBase<Classroom>
    {
        [TestMethod]
        public void NewClassroomIsEmpty()
        {
            Assert.AreEqual(0, sut.Students.Count());
        }
        
        [TestMethod]
        public void AddStudentTest()
        {
            var student = fixture.Create<Student>();
            sut.AddStudent(student);
            Assert.AreEqual(1, sut.Students.Count(f=>f == student));
        }


        [TestMethod]
        public void AddManyStudentsTest()
        {
            var student = fixture.Create<Student>();
            sut.AddStudent(student);
            var students = fixture.CreateMany<Student>();
            sut.AddStudents(students);
            Assert.AreEqual(students.Count() + 1, sut.Students.Count());
        }

        [TestMethod]
        public void AddSameStudentTwiceTest()
        {
            var student = fixture.Create<Student>();
            sut.AddStudent(student);
            sut.AddStudent(student);
            Assert.AreEqual(1, sut.Students.Count(f => f == student));
        }

        [TestMethod]
        public void RemoveStudentTest()
        {
            var student = fixture.Create<Student>();
            sut.AddStudent(student);
            sut.RemoveStudent(student);
            Assert.IsFalse(sut.Students.Contains(student));
        }
    }
}
