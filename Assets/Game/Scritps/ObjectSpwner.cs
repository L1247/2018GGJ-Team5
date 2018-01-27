
using System;
using UnityEngine;
using Zenject;

public class ObjectSpwner : MonoBehaviour
{
    private Transform firstTransform;

    [Header("行")] [SerializeField] private int row;
    [Inject] private ClickManager clickManager;
    [Inject] private SpawnManager spawnManager;
    private static int spawnIndex = 1;


    public void Spawn()
    {
        firstTransform = transform.GetChild(0);
        Transform parentTransform = firstTransform.parent;
        Vector3 firstPostion = firstTransform.position;
        string initName = "鳥";

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < GloData.ColumnCount; j++)
            {
                string newName = String.Format("{0} [{1}-{2}]", initName, spawnIndex, (j + 1));

                Vector3 xOffset = Vector3.right * GloData.RowOffset * i;
                Vector3 yOffset = Vector3.down * GloData.ColumnOffset * j;
                Vector3 spawnPos = firstPostion + xOffset + yOffset;
                GameObject newObj = Instantiate(firstTransform, spawnPos
                    , Quaternion.identity, parentTransform).gameObject;
                newObj.name = newName;
                clickManager.SubscribeMouseDown(newObj);
                spawnManager.PutIntoPool(newObj);
            }
            spawnIndex++;
        }
        firstTransform.gameObject.SetActive(false);
    }
}
