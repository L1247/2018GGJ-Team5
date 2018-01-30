using System;
using System.Collections;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    [SerializeField] private Transform birdGroup;
    [SerializeField] private ObjectSpwner[] _objectSpwners;
    [SerializeField] private AudioSource soundAudioSource;
    [SerializeField] private AudioClip clickClip, sendClip1, sendClip2;
    [SerializeField] private GameObject UI_Rule;
    [SerializeField] private Image UI_Result;

    [SerializeField] private Sprite Win, GameOver;

    [Inject] private SpawnManager _spawnManager;
    private Transform mainCharacter, targetCharacter, currentCharacter, clickedCharacter, parrotCharacter;
    [HideInInspector] public bool IsClickable = true;

    // Use this for initialization
    void Start()
    {
        for (var i = 0; i < _objectSpwners.Length; i++)
            _objectSpwners[i].Spawn(birdGroup);

        GameStart();
    }

    private void GameStart()
    {
        UI_Rule.SetActive(true);
        UI_Result.gameObject.SetActive(false);
        SetMainAndTarget();
        IsClickable = true;
    }

    void SetMainAndTarget()
    {
        mainCharacter = _spawnManager.GetRandomMainGo();
        targetCharacter = _spawnManager.GetRandomTargetGo();
        parrotCharacter = _spawnManager.GetRandomMiddleGo();
        currentCharacter = mainCharacter;
    }

    public bool CheckIsCurrentCharNeighborhood(GameObject targetGo)
    {
        Transform targetTrans = targetGo.transform;
        if (targetTrans.Equals(currentCharacter))
            return false;
        float distance = Vector3.Distance(currentCharacter.position, targetTrans.position);
        float offset = 0.1f;
        int targetRow = int.Parse(targetTrans.name.Split('[')[1].Substring(0, 1));
        int targetColumn = int.Parse(targetTrans.name.Split('-')[1].Substring(0, 1));
        int mainRow = int.Parse(currentCharacter.name.Split('[')[1].Substring(0, 1));
        int mainColumn = int.Parse(currentCharacter.name.Split('-')[1].Substring(0, 1));

        bool isUpDown = distance <= GloData.ColumnOffset + offset;
        bool isLeftRight = distance <= GloData.RowOffset + offset;
        bool isNeighbor = Mathf.Abs(targetRow - mainRow).Equals(1)
                          && targetColumn.Equals(mainColumn);

        return isUpDown || isLeftRight || isNeighbor;
    }

    public void SendLetter(GameObject target)
    {
        IsClickable = false;
        clickedCharacter = target.transform;
        bool isTargetOnRight = clickedCharacter.position.x > currentCharacter.position.x;
        bool isTargetOnLeft = clickedCharacter.position.x < currentCharacter.position.x;
        bool isTargetOnUp = clickedCharacter.position.y > currentCharacter.position.y;
        bool isTargetOnDown = clickedCharacter.position.y < currentCharacter.position.y;
        BirdMisc nextBirdMisc = _spawnManager.GetBirdMisc(target);
        BirdMisc currentbirdMisc = _spawnManager.GetBirdMisc(currentCharacter.gameObject);

        ActionTypeCollction[] typeCollctions = new ActionTypeCollction[3];
        if (isTargetOnRight)
        {
            typeCollctions[0] = new ActionTypeCollction(ActionType.SendRight, ActionType.RaiseLeftHand);
            typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseRightHand, ActionType.SendLeft);
        }
        else if (isTargetOnLeft)
        {

            typeCollctions[0] = new ActionTypeCollction(ActionType.SendLeft, ActionType.RaiseRightHand);
            typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseLeftHand, ActionType.SendRight);
        }
        else if (isTargetOnUp)
        {
            typeCollctions[0] = new ActionTypeCollction(ActionType.SendForward, ActionType.RaiseBackHand);
            typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseForwardHand, ActionType.SendBack);
        }
        else if (isTargetOnDown)
        {
            typeCollctions[0] = new ActionTypeCollction(ActionType.SendBack, ActionType.RaiseForwardHand);
            typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseBackHand, ActionType.SendForward);
        }

        typeCollctions[2] = new ActionTypeCollction(ActionType.Idle, ActionType.Idle);
        SetAndCallActions(currentbirdMisc, nextBirdMisc, typeCollctions);
    }

    private void SetAndCallActions(BirdMisc current, BirdMisc next, params ActionTypeCollction[] types)
    {
        Action[] actions = new Action[types.Length];
        for (var i = 0; i < types.Length; i++)
        {
            ActionTypeCollction typeCollction = types[i];
            Action action = () => {
                current.SetSprite(typeCollction.GetTypes()[0]);
                next.SetSprite(typeCollction.GetTypes()[1]);
            };
            actions[i] = action;
        }
        soundAudioSource.PlayOneShot(clickClip);
        StartCoroutine(DoActions(actions));
    }

    IEnumerator DoActions(Action[] actions)
    {
        for (var i = 0; i < actions.Length; i++)
        {
            actions[i]();
            yield return new WaitForSeconds(1f);
            if (i == 0) soundAudioSource.PlayOneShot(sendClip1);
            if (i == 1)
            {
                soundAudioSource.PlayOneShot(sendClip2);
            }
        }
        IsClickable = CheckGameResult(clickedCharacter);
        currentCharacter = clickedCharacter;
    }

    private bool CheckGameResult(Transform clickedTarget)
    {
        bool isRightBird = clickedTarget.Equals(targetCharacter);
        bool isWrongBird = clickedTarget.Equals(parrotCharacter);
        bool isNormalBird = (isRightBird || isWrongBird) == false;
        if (isRightBird) SetResultSprite(Win);
        if (isWrongBird) SetResultSprite(GameOver);

        return isNormalBird;
    }

    void SetResultSprite(Sprite sprite)
    {
        UI_Result.sprite = sprite;
        UI_Result.gameObject.SetActive(true);
    }
}

public class ActionTypeCollction
{
    private ActionType[] types = new ActionType[2];
    public ActionTypeCollction(ActionType mainCharType, ActionType TargetType)
    {
        types[0] = mainCharType;
        types[1] = TargetType;
    }

    public ActionType[] GetTypes()
    {
        return types;
    }
}
