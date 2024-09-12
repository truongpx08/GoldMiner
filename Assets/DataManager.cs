using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : TruongSingleton<DataManager>
{
    [SerializeField] private UserData userData;
    public UserData UserData => this.userData;
    [SerializeField] private int level;
    public int Level => this.level;

    public void SetUserData(UserData newUserData)
    {
        this.userData = newUserData;
    }

    public void SetLevel(int value)
    {
        this.level = value;
    }
}