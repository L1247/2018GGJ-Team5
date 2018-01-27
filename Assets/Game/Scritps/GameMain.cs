using SpriteColorFX;
using UnityEngine;
using Zenject;
using UniRx;

public class GameMain : MonoBehaviour
{
    [SerializeField] private Sprite mainCharacterSprite, sparrowSprite;
    [SerializeField] private ObjectSpwner[] _objectSpwners;
    [Inject] private SpawnManager _spawnManager;

    private Transform mainCharacter;
    // Use this for initialization
    void Start()
    {
        for (var i = 0; i < _objectSpwners.Length; i++)
            _objectSpwners[i].Spawn();

        SetMainAndTarget();
        Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ => SetRandomColorFx());
    }

    void SetMainAndTarget()
    {
        mainCharacter = _spawnManager.GetRandomMainGo().transform;
        mainCharacter.GetComponent<SpriteRenderer>().sprite = mainCharacterSprite;
    }

    void SetRandomColorFx()
    {
        GameObject mainCharacter = _spawnManager.GetRandomNotRepeatingObj();
        DemoDissolve demoDissolve = mainCharacter.GetComponent<DemoDissolve>();
        demoDissolve.enabled = true;
    }

    public void CheckIsWASD(GameObject go)
    {
        Transform targetTrans = go.transform;
        float distance = Vector3.Distance(mainCharacter.position, targetTrans.position);
        float offset = 0.1f;
        Debug.Log(distance);
        if (distance <= GloData.ColumnOffset + offset || distance <= GloData.RowOffset + offset)
        {
            mainCharacter.GetComponent<SpriteRenderer>().sprite = sparrowSprite;
            mainCharacter = targetTrans;
            mainCharacter.GetComponent<SpriteRenderer>().sprite = mainCharacterSprite;
        }
    }

}
