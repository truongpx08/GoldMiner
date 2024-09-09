using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Items : TruongSingleton<Items>
{
    [SerializeField] private List<Transform> lines;
    public List<Transform> Lines => this.lines;
    [SerializeField] private GameObject chestPrefab;
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
            case EItemType.Chest:
                lineList = GetAvailableLines(new List<int> { 3, 4, 6, 7, 8 });
                Debug.Log(lineList.Count);
                return lineList[Random.Range(0, lineList.Count)];
            default:
                throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
        }
    }

    private List<Transform> GetAvailableLines(List<int> linesPosition)
    {
        var lineList = new List<Transform>();
        foreach (var linePosition in linesPosition)
        {
            int lineIndex = linePosition - 1;
            var line = this.lines[lineIndex];
            if (line.childCount < 2)
                lineList.Add(line);
        }

        return lineList;
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
            EItemType.Chest => this.chestPrefab,
            _ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
        };
    }

    [Button]
    float UpdatePositionLines()
    {
        int count = this.lines.Count;

        // Kiểm tra nếu số dòng ít hơn 2  
        if (count < 2) return 0f;

        // Tính toán khoảng cách và yPerLine  
        var first = this.lines[0];
        var last = this.lines[count - 1];
        var firstLP = first.transform.localPosition;

        float space = Mathf.Abs(last.transform.localPosition.y - firstLP.y);
        float yPerLine = space / (count - 1);

        // Cập nhật các vị trí cho các dòng  
        for (int i = 1; i < count - 1; i++)
        {
            var line = this.lines[i];
            line.transform.localPosition = new Vector3(firstLP.x, firstLP.y - yPerLine * i, firstLP.z);
        }

        return yPerLine;
    }
}