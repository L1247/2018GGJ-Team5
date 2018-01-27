using Zenject;
using System.Collections;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class TestLogger : ZenjectIntegrationTestFixture
{
    [SetUp]
    public void CommonInstall ( )
    {
        Debug.Log("SetUp");
    }

    [UnityTest]
    public IEnumerator TestInitialValues ( )
    {
        // Setup initial state by creating game objects from scratch, loading prefabs/scenes, etc

        PreInstall();
        // Call Container.Bind methods
        Container.Bind<Logger>().AsSingle();

        PostInstall();

        // Add test assertions for expected state
        // Using Container.Resolve or [Inject] fields
        var logger = Container.Resolve<Logger>();
        Assert.That( logger.Log == "" );
        
        yield break;
    }
}

public class Logger
{
    public Logger ( )
    {
        Log = "";
    }

    public string Log
    {
        get;
        private set;
    }

    public void Write ( string value )
    {
        if ( value == null )
        {
            throw new System.ArgumentException();
        }

        Log += value;
    }
}