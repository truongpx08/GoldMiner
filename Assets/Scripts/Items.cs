using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Items : TruongSingleton<Items>
{
    [SerializeField] private List<Transform> lines;
    public List<Transform> Lines => this.lines;
    [SerializeField] private GameObject rewardPrefab;
    [SerializeField] private GameObject timePrefab;
    [SerializeField] private GameObject trapPrefab;

    public Transform GetRandomLineWithItem(EItemType itemType)
    {
        List<Transform> lineList;
        switch (itemType)
        {
            case EItemType.Trap:
                lineList = GetLines(new List<int> { 2, 5 });
                return lineList[Random.Range(0, lineList.Count)];
            case EItemType.Time:
            case EItemType.Reward:
                lineList = GetLines(new List<int> { 3, 4, 6, 7 });
                return lineList[Random.Range(0, lineList.Count)];
            default:
                throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
        }
    }

    private List<Transform> GetLines(List<int> linesPosition)
    {
        var lineList = new List<Transform>();
        foreach (var linePosition in linesPosition)
        {
            int lineIndex = linePosition - 1;
            lineList.Add(this.lines[lineIndex]);
        }

        return lineList;
    }

    public GameObject GetItemPrefabWithType(EItemType itemType)
    {
        return itemType switch
        {
            EItemType.Trap => this.trapPrefab,
            EItemType.Time => this.timePrefab,
            EItemType.Reward => this.rewardPrefab,
            _ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
        };
    }
}