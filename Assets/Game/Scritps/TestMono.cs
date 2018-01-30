using DG.Tweening;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer.DOFade(0.75f , 2f).SetEase(Ease.InOutQuad).SetLoops(-1,LoopType.Yoyo);
    }
}
