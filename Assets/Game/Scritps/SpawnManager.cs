using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{
    private List<GameObject> classMates = new List<GameObject>();
    private List<BirdMisc> birdMiscs = new List<BirdMisc>();
    private int selectIndex;
    private bool shuffled;
    private void Shuffle()
    {
        classMates.Shuffle();
        shuffled = true;
    }

    public void PutIntoPool(GameObject go)
    {
        classMates.Add(go);

        BirdMisc birdMisc = new BirdMisc(go, BirdType.Sparrow);
        birdMisc.gameObject = go;
        birdMiscs.Add(birdMisc);
    }

    public BirdMisc GetBirdMisc(GameObject go)
    {
        return birdMiscs.Find(birdmisc => birdmisc.gameObject.Equals(go));
    }


    public GameObject GetRandomNotRepeatingObj(bool showLog = false)
    {
        if (shuffled == false) Shuffle();
        GameObject selectedObj = classMates[selectIndex];
        selectIndex++;
        if (selectIndex >= classMates.Count) selectIndex = 0;
        if (showLog) Debug.Log(selectedObj);
        return selectedObj;
    }

    public GameObject GetRandomMainGo()
    {
        int randIndex = Random.Range(0, GloData.ColumnCount);
        return birdMiscs[randIndex].gameObject;
    }

    public GameObject GetRandomTargetGo()
    {
        int randIndex = Random.Range(classMates.Count - GloData.ColumnCount, classMates.Count);
        return birdMiscs[randIndex].gameObject;
    }
}