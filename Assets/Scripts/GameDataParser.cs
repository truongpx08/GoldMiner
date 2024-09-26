using System;
using System.Collections.Generic;

public class GameDataParser
{
}

[Serializable]
public class ApiResponse
{
    public bool success;
    public string message;
}

[Serializable]
public class EncryptedObject
{
    public string encryptedData;
}

//Time
[Serializable]
public class TimeData : ApiResponse
{
    public long timestamp;
}

//EncryptObject
public class BaseEncryptObject
{
    public string timestamp;
}

[Serializable]
public class StartEncryptObject : BaseEncryptObject
{
    public string level;
}

[Serializable]
public class MoveEncryptObject : BaseEncryptObject
{
    public string gameId;
    public string type;
}

[Serializable]
public class FinishEncryptObject : MoveEncryptObject
{
}

[Serializable]
public class RerollEncryptObject : BaseEncryptObject
{
    public string gameId;
}

// UserData
[Serializable]
public class UserData : ApiResponse
{
    public UserDataDetails data;
}

[Serializable]
public class UserDataDetails
{
    public User user;
    public ChanceData chance;
    public HighScoreData highScore;
    public List<CrystalData> crystalData;
}

[Serializable]
public class ChanceData
{
    public int NORMAL_CHEST;
    public int RARE_CHEST;
    public int EPIC_CHEST;
    public int LEGENDARY_CHEST;
}

[Serializable]
public class HighScoreData
{
    public float tamanXClaimed;
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
    public long BALANCE_UNLOCK;
    public long FEE_INPUT;
    public float CHEST_VALUE;
    public float NORMAL_CHEST;
    public float RARE_CHEST;
    public float EPIC_CHEST;
    public float LEGENDARY_CHEST;
    public bool AVAILABLE;
}

// StartData
[Serializable]
public class StartData : ApiResponse
{
    public StartDataDetails data;
}

[Serializable]
public class StartDataDetails
{
    public string gameId;
}

// MoveData

[Serializable]
public class MoveData : ApiResponse
{
    public MoveDataDetails data;
}

[Serializable]
public class MoveDataDetails
{
    public float numChest;
}

// FinishData
[Serializable]
public class FinishData : ApiResponse
{
    public FinishDataDetails data;
}

[Serializable]
public class FinishDataDetails
{
    public float tamanXReward;
    public float totalTamanX;
}