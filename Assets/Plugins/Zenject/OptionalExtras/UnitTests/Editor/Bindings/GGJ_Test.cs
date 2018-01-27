using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using System.Linq;
using Assert = ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class GGJ_Test : ZenjectUnitTestFixture
    {
        [Test]
        public void TestTransient()
        {

            Container.Bind<Goo>().FromNew().AsSingle().NonLazy();

            Assert.IsNotNull(Container.Resolve<Goo>());
        }

    }
    interface IGoo
    {
    }

    class Goo : IGoo
    {
    }
}

