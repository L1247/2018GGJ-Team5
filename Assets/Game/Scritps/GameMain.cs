using UnityEngine;
using Zenject;
using UniRx;

public class GameMain : MonoBehaviour
{
    [SerializeField] private ObjectSpwner[] objectSpwners;
    [Inject] private SpawnManager spawnManager;
    
    // Use this for initialization
    void Start()
    {
        objectSpwners = FindObjectsOfType<ObjectSpwner>();
        for (var i = 0; i < objectSpwners.Length; i++)
        {
            objectSpwners[i].Spawn();
        }

        //InitEnd();
        Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ => InitEnd());
    }

    public void InitEnd()
    {
        spawnManager.GetRandomNotRepeatingObj(true);
    }

}
