using System;
using AutoTeam.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTeam.Tests.Model
{
    [TestClass]
    public class ClassificationTest : TestBase<Classification>
    {
        [TestMethod]
        public void TestFactory()
        {
            var result = Classification.Create(ClassificationEnum.Female);
            Assert.IsInstanceOfType(result, typeof(Female));
            result = Classification.Create(ClassificationEnum.Male);
            Assert.IsInstanceOfType(result, typeof(Male));
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestFactoryRange()
        {
            Classification.Create((ClassificationEnum)int.MinValue);
        }
    }
}
