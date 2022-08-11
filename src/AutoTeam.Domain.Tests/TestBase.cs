using System.Globalization;
using System.Threading;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoTeam.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTeam.Tests
{
    [TestClass]
    public abstract class TestBase<T> where T:class
    {
        protected T sut;
        protected IFixture fixture;

        [TestInitialize]
        public void Init()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Inject<IGroupAssigmentStrategy>(new AssignToFirstAvailableGroup());
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            LocalInit();
            sut = fixture.Create<T>();
        }

        protected virtual void LocalInit()
        { }

        [TestMethod]
        public void VerifyConstructorGuards()
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(T).GetConstructors());
        }

        [TestMethod]
        public void VerifyMethodGuards()
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(T).GetMethods( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Static));
        }

        [TestMethod]
        public void EqualsNullCheck()
        {
            var equals = sut.Equals(null);
            Assert.IsFalse(equals);
        }
    }
}
