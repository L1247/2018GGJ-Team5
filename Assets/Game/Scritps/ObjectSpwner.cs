
using System;
using UnityEngine;
using Zenject;

public class ObjectSpwner : MonoBehaviour
{
    [SerializeField] private Transform firstTransform;

    [Header("行")] [SerializeField] private int row;
    [Header("列")] [SerializeField] private int column;
    [Space(10)] [SerializeField] private float rowOffset, columnOffset;
    [Inject] private ClickManager clickManager;
    [Inject] private SpawnManager spawnManager;
    [Inject] private GameMain gameMain;
    private static int spawnIndex = 1;


    public void Spawn()
    {
        firstTransform = transform.GetChild(0);
        Transform parentTransform = firstTransform.parent;
        Vector3 firstPostion = firstTransform.position;
        string initName = firstTransform.name;

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                string newName = String.Format("{0} [{1}]", initName, spawnIndex);

                Vector3 xOffset = Vector3.right * rowOffset * i;
                Vector3 yOffset = Vector3.down * columnOffset * j;
                Vector3 spawnPos = firstPostion + xOffset + yOffset;
                GameObject newObj = Instantiate(firstTransform, spawnPos
                    , Quaternion.identity, parentTransform).gameObject;
                newObj.name = newName;
                spawnIndex++;
                clickManager.SubscribeMouseDown(newObj);
                spawnManager.PutIntoPool(newObj);
            }
        }
        firstTransform.gameObject.SetActive(false);
    }
}
