
using System;
using System.IO;
using UnityEngine;

public class BirdMisc
{

    protected Sprite _sprite;
    protected SpriteRenderer _spriteRenderer;
    public GameObject gameObject;
    private Bird _bird;

    public BirdMisc(GameObject go, BirdType birdType)
    {
        gameObject = go;
        _spriteRenderer = go.GetComponent<SpriteRenderer>();
        _bird = new Bird(birdType);
    }

    public void SetBirdType(BirdType type)
    {
        _bird = new Bird(type);
        SetSprite(ActionType.Idle);
    }

    public void SetSprite(ActionType type)
    {
        _spriteRenderer.sprite = _bird.GetSprite(type);
    }
}

public class Bird
{
    private readonly string defaultPath = "Character";
    private readonly string birdSpritePath;
    private BirdType _birdType;
    private string _birdName;

    public Bird(BirdType type)
    {
        _birdType = type;
        _birdName = type.ToString();
        birdSpritePath = CombinePath(defaultPath, _birdName);
    }

    public Sprite GetSprite(ActionType type)
    {
        string actionName = CombineActionPath(_birdType.GetEnumDescription(), type.GetEnumDescription());
        string resourcesPath = CombinePath(birdSpritePath, actionName);
        //Debug.Log(resourcesPath);
        return Resources.Load<Sprite>(resourcesPath);
    }

    private string CombineActionPath(string path1, string path2)
    {
        return string.Format("{0} {1}", path1, path2);
    }

    private string CombinePath(string path1, string path2)
    {
        return string.Format("{0}/{1}", path1, path2);
    }

}