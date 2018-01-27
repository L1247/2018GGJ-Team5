using System;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert = ModestTree.Assert;
using Zenject.Tests.Bindings.FromPrefab;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestBinding : ZenjectUnitTestFixture
    {
        [Test]
        public void TestBind ( )
        {
            Container.Bind<IFoo>().WithId( "foo3" ).To<Foo1>().AsTransient();
            Container.Bind<IFoo>().WithId( "foo" ).To<Foo1>().AsTransient();
            Container.Bind<IFoo>().To<Foo2>().AsSingle();
            Container.Bind<Bar1>().AsSingle();
            Container.Bind<Bar2>().AsSingle();
            Container.Bind<Bar3>().AsSingle();

            Assert.IsNotNull( Container.Resolve<Bar1>() );
            Assert.IsNotNull( Container.Resolve<Bar2>() );
            Debug.Log( Container.Resolve<Bar1>()._foo );
            Debug.Log( Container.Resolve<Bar2>()._foo );
            Debug.Log( Container.Resolve<Bar3>()._foo );

            Assert.IsNotEqual( Container.Resolve<Bar1>()._foo ,
                Container.Resolve<Bar3>()._foo );

        }

        [Test]
        public void TestBindCached ( )
        {
            Container.Bind<Foo1>().AsCached();
            Container.Bind<IFoo>().To<Foo1>().AsCached();
            Container.Bind<Bar2>().AsSingle().NonLazy();
            Container.Bind<Bar4>().AsSingle().NonLazy();
            Debug.Log( Container.Resolve<Bar2>() );
            Debug.Log( Container.Resolve<Bar4>() );

            Assert.IsNotEqual( Container.Resolve<Bar2>()._foo ,
                Container.Resolve<Bar4>()._foo );
        }

        [Test]
        public void TestConditionalBindings ( )
        {
            Container.Bind<Foo1>().AsCached().WhenInjectedInto<Bar4>();
            Container.Bind<IFoo>().To<Foo1>().AsCached().WhenInjectedInto<Bar2>();
            Container.Bind<Bar2>().AsSingle().NonLazy();
            Container.Bind<Bar4>().AsSingle().NonLazy();
            Debug.Log( Container.Resolve<Bar2>() );
            Debug.Log( Container.Resolve<Bar4>() );

            Assert.IsNotEqual( Container.Resolve<Bar2>()._foo ,
                Container.Resolve<Bar4>()._foo );
        }
    }
    public class Foo1 : IFoo
    {
        public int num;
        public Foo1 ( )
        {
            Debug.Log( "new Foo1" );
            num++;
        }
    }
    public class Foo2 : IFoo { }
    public class Bar1
    {
        [Inject(Id = "foo")]
        public IFoo _foo;
    }

    public class Bar3
    {
        [Inject(Id = "foo3")]
        public IFoo _foo;
    }

    public class Bar2
    {
        [Inject]
        public IFoo _foo;
    }

    public class Bar4
    {
        [Inject]
        public Foo1 _foo;
    }
}
