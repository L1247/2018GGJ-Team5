using UnityEngine;
using Zenject;

public class GGJInstaller : MonoInstaller<GGJInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GameMain>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        Container.Bind<ClickManager>().FromNew().AsSingle().NonLazy();
        Container.Bind<SpawnManager>().FromNew().AsSingle().NonLazy();
    }
}