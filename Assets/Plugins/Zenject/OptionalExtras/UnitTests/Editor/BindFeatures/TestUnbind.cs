using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.BindFeatures
{
    [TestFixture]
    public class TestUnbind : ZenjectUnitTestFixture
    {
        interface ITest
        {
        }

        interface ITest2
        {
        }

        class Test2 : ITest, ITest2
        {
        }

        [Test]
        public void Run()
        {
            Container.Bind<ITest>().To<Test2>().AsCached();
            Container.Bind<ITest>().To<Test2>().AsCached();

            Debug.Log(Container.ResolveAll<ITest>().Count);
            List<ITest> itest = Container.ResolveAll<ITest>();
            Debug.Log(itest[0] == itest[1]);
            Assert.IsNotNull(Container.Resolve<ITest>());

            Container.Unbind<ITest>();

            Assert.IsNull(Container.TryResolve<ITest>());
        }

        [Test]
        public void TestUnbindInterfaces()
        {
            Container.BindInterfacesTo<Test2>().AsSingle();

            Assert.IsNotNull(Container.Resolve<ITest>());
            Assert.IsNotNull(Container.Resolve<ITest2>());

            Container.UnbindInterfacesTo<Test2>();

            Assert.IsNull(Container.TryResolve<ITest>());
            Assert.IsNull(Container.TryResolve<ITest2>());
        }
    }
}
