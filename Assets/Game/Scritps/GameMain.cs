using SpriteColorFX;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

public class GameMain : MonoBehaviour
{
    [SerializeField] private Sprite mainCharacterSprite, WhitebirdSprite, sparrowSprite;
    [SerializeField] private ObjectSpwner[] _objectSpwners;
    [Inject] private SpawnManager _spawnManager;

    private Transform mainCharacter, targetCharacter;
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
        targetCharacter = _spawnManager.GetRandomTargetGo().transform;
        targetCharacter.GetComponent<SpriteRenderer>().sprite = WhitebirdSprite;
    }

    void SetRandomColorFx()
    {
        GameObject mainCharacter = _spawnManager.GetRandomNotRepeatingObj();
        DemoDissolve demoDissolve = mainCharacter.GetComponent<DemoDissolve>();
        demoDissolve.enabled = true;
    }

    public bool CheckIsMainNeighborhood(GameObject targetGo)
    {
        Transform targetTrans = targetGo.transform;
        float distance = Vector3.Distance(mainCharacter.position, targetTrans.position);
        float offset = 0.1f;
        int targetRow = int.Parse(targetTrans.name.Split('[')[1].Substring(0, 1));
        int targetColumn = int.Parse(targetTrans.name.Split('-')[1].Substring(0, 1));
        int mainRow = int.Parse(mainCharacter.name.Split('[')[1].Substring(0, 1));
        int mainColumn = int.Parse(mainCharacter.name.Split('-')[1].Substring(0, 1));

        bool isUpDown = distance <= GloData.ColumnOffset + offset;
        bool isLeftRight = distance <= GloData.RowOffset + offset;
        bool isNeighbor = Mathf.Abs(targetRow - mainRow).Equals(1)
                          && targetColumn.Equals(mainColumn);

        return isUpDown || isLeftRight || isNeighbor;
    }

    public void SendLetter(GameObject target)
    {
        //mainCharacter.GetComponent<SpriteRenderer>().sprite = sparrowSprite;
        //mainCharacter.GetComponent<SpriteRenderer>().sprite = mainCharacterSprite;
        Transform targetTransform = target.transform;
        bool isTargetOnRight = targetTransform.position.x > mainCharacter.position.x;
        mainCharacter = target.transform;
    }

}
