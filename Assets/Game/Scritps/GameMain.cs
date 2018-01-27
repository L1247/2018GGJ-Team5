using UnityEngine;
using Zenject;
using UniRx;

public class GameMain : MonoBehaviour
{
    [SerializeField] private Sprite mainCharacterSprite;
    [SerializeField] private ObjectSpwner[] _objectSpwners;
    [Inject] private SpawnManager _spawnManager;
    
    // Use this for initialization
    void Start()
    {
        for (var i = 0; i < _objectSpwners.Length; i++)
            _objectSpwners[i].Spawn();

        //SetMainAndTarget();
        Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ => SetMainAndTarget());
    }

    public void SetMainAndTarget()
    {
        GameObject mainCharacter = _spawnManager.GetRandomMainGo();
        mainCharacter.GetComponent<SpriteRenderer>().sprite = mainCharacterSprite;
    }

}
