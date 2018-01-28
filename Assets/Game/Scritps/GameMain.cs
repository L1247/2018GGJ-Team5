using System;
using System.Collections;
using SpriteColorFX;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

public class GameMain : MonoBehaviour
{
    [SerializeField] private Transform birdGroup;
    [SerializeField] private ObjectSpwner[] _objectSpwners;
    [SerializeField] private AudioSource soundAudioSource;
    [SerializeField] private AudioClip clickClip , sendClip1, sendClip2;
    [Inject] private SpawnManager _spawnManager;

    private Transform mainCharacter, targetCharacter, currentCharacter, clickedCharacter;

    public bool IsClickable = true;
    // Use this for initialization
    void Start()
    {
        for (var i = 0; i < _objectSpwners.Length; i++)
            _objectSpwners[i].Spawn(birdGroup);

        GameStart();
    }

    private void GameStart()
    {
        SetMainAndTarget();
        IsClickable = true;
    }

    void SetMainAndTarget()
    {
        mainCharacter = _spawnManager.GetRandomMainGo().transform;
        _spawnManager.GetBirdMisc(mainCharacter.gameObject).SetBirdType(BirdType.Ostrich);
        targetCharacter = _spawnManager.GetRandomTargetGo().transform;
        _spawnManager.GetBirdMisc(targetCharacter.gameObject).SetBirdType(BirdType.WenBird);
        currentCharacter = mainCharacter;

        Transform Parrot =  _spawnManager.GetRandomMiddleGo().transform;
        _spawnManager.GetBirdMisc(Parrot.gameObject).SetBirdType(BirdType.Parrot);

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
        bool isTargetOnUp = clickedCharacter.position.y > currentCharacter.position.y;
        BirdMisc nextBirdMisc = _spawnManager.GetBirdMisc(target);
        BirdMisc currentbirdMisc = _spawnManager.GetBirdMisc(currentCharacter.gameObject);

        //Debug.Log(isTargetOnRight);
        //Debug.Log(isTargetOnUp);
        ActionTypeCollction[] typeCollctions = new ActionTypeCollction[3];
        if (isTargetOnUp == false)
        {
            if (isTargetOnRight)
            {
                typeCollctions[0] = new ActionTypeCollction(ActionType.SendRight, ActionType.RaiseLeftHand);
                typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseRightHand, ActionType.SendLeft);
            }
            else if (isTargetOnRight == false)
            {
                typeCollctions[0] = new ActionTypeCollction(ActionType.SendLeft, ActionType.RaiseRightHand);
                typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseLeftHand, ActionType.SendRight);
            }
        }
        else if (isTargetOnUp)
        {
            typeCollctions[0] = new ActionTypeCollction(ActionType.SendForward, ActionType.RaiseBackHand);
            typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseForwardHand, ActionType.SendBack);
        }
        //else if (isTargetOnUp == false)
        //{
        //    typeCollctions[0] = new ActionTypeCollction(ActionType.SendBack, ActionType.RaiseForwardHand);
        //    typeCollctions[1] = new ActionTypeCollction(ActionType.RaiseBackHand, ActionType.SendForward);
        //}



        typeCollctions[2] = new ActionTypeCollction(ActionType.Idle, ActionType.Idle);
        SetAndCallActions(
            currentbirdMisc, nextBirdMisc, typeCollctions
        );
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
        StartCoroutine(Fade(actions));
    }

    IEnumerator Fade(Action[] actions)
    {
        for (var i = 0; i < actions.Length; i++)
        {
            if(i== 0) soundAudioSource.PlayOneShot(sendClip1);
            if(i== 1) soundAudioSource.PlayOneShot(sendClip2);
            actions[i]();
            yield return new WaitForSeconds(1f);
        }
       

        currentCharacter = clickedCharacter;
        IsClickable = true;
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
