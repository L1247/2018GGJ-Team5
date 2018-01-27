using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ObjectSpwner>().FromNew().NonLazy();
        Container.Bind<GameMain>().FromNewComponentOnNewGameObject().NonLazy();
    }
}