using System;
using System.Collections.Generic;

public class GameDataParser
{
}

[Serializable]
public class UserData
{
    public bool success;
    public UserDataDetails data;
}

[Serializable]
public class UserDataDetails
{
    public User user;
    public float highScore;
    public List<CrystalData> crystalData;
}

[Serializable]
public class User
{
    public string wallet;
    public float tamanX;
    public float taman;
}

[Serializable]
public class CrystalData
{
    public int level;
    public float BALANCE_UNLOCK;
    public long FEE_INPUT;
    public float CHEST_VALUE;
    public float NORMAL_CHEST;
    public float RARE_CHEST;
    public float EPIC_CHEST;
    public float LEGENDARY_CHEST;
    public bool AVAILABLE;
}