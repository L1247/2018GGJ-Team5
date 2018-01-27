using SpriteColorFX;
using UnityEngine;
using  UniRx;
using  UniRx.Triggers;

public class ClickManager
{
    public void SubscribeMouseDown(GameObject gameObject)
    {
        //Debug.Log(gameObject);
        gameObject
            .OnMouseDownAsObservable()
            .Subscribe(_=>
            {
                Debug.Log(gameObject);
            });
    }
}
