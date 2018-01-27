using UnityEngine;
using Zenject;

public class GGJInstaller : MonoInstaller<GGJInstaller>
{
    [SerializeField] private GameMain gameMain;
    public override void InstallBindings()
    {
        Container.Bind<GameMain>().FromInstance(gameMain).AsSingle().NonLazy();
        Container.Bind<ClickManager>().FromNew().AsSingle().NonLazy();
        Container.Bind<SpawnManager>().FromNew().AsSingle().NonLazy();
    }
}