using SpriteColorFX;
using UnityEngine;
using  UniRx;
using  UniRx.Triggers;
using Zenject;

public class ClickManager
{
    [Inject] private GameMain gameMain;
    public void SubscribeMouseDown(GameObject gameObject)
    {
        //Debug.Log(gameObject);
        gameObject
            .OnMouseDownAsObservable()
            .Subscribe(_=>
            {
                gameMain.CheckIsWASD(gameObject);
            });
    }
}
