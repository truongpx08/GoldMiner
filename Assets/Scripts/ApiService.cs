using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Networking;

public enum EApiType
{
    GetUserData,
    PostStart,
    PostMove,
    PostFinish,
}

public class ApiService : TruongSingleton<ApiService>
{
    [SerializeField] private string wallet;
    [SerializeField] private string url = "https://refactor.faraland.moonknightlabs.com";

    public void SetWallet(string value)
    {
        wallet = value;
        Debug.LogWarning($"SetWallet {wallet}");
        if (value.IsNullOrWhitespace())
            ErrorPopup.Instance.ShowError($"Wallet {wallet} error");
        else
            GameStateMachine.Instance.ChangeState(EGameState.SelectLevel);
    }

    public void Request(EApiType type, Action<string> onComplete = null, Action<string> onError = null)
    {
        Debug.Log("Requesting " + type);
        switch (type)
        {
            case EApiType.GetUserData:
                StartCoroutine(IEGetRequest("crystal", onComplete, onError));
                break;
            case EApiType.PostStart:
                StartPostRequest("start", CreateStartEncryptObject, onComplete, onError);
                break;
            case EApiType.PostMove:
                StartPostRequest("move", CreateMoveEncryptObject, onComplete, onError);
                break;
            case EApiType.PostFinish:
                StartPostRequest("finish", CreateFinishEncryptObject, onComplete, onError);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private IEnumerator IEGetRequest(string endpoint, Action<string> onComplete = null, Action<string> onError = null)
    {
        using UnityWebRequest
            request = UnityWebRequest.Get(
                $"{url}/{endpoint}"); //using được sử dụng để đảm bảo rằng các tài nguyên được cấp phát được giải phóng hợp lý sau khi không còn cần thiết
        request.SetRequestHeader("wallet", wallet.ToLower());
        yield return request.SendWebRequest();
        HandleResponse(request, onComplete, onError);
    }

    private void StartPostRequest(string action, Func<long, object> createEncryptObject, Action<string> onComplete,
        Action<string> onError)
    {
        StartCoroutine(IEGetRequest("time", json =>
        {
            long timestamp = ParseTimestamp(json);
            var encryptObject = createEncryptObject(timestamp);
            StartCoroutine(IEPostRequest($"crystal/{action}", encryptObject, onComplete, onError));
        }, onError));
    }

    private long ParseTimestamp(string json)
    {
        var obj = JsonUtility.FromJson<TimeData>(json);
        return obj.timestamp;
    }

    private object CreateStartEncryptObject(long timestamp)
    {
        return new StartEncryptObject
        {
            timestamp = timestamp.ToString(),
            level = Option.Instance.CurrentOption.Data.level.ToString()
        };
    }

    private object CreateMoveEncryptObject(long timestamp)
    {
        var hookedItem = MiningMachine.Instance.HookCollider.HookedItem;
        return new MoveEncryptObject
        {
            timestamp = timestamp.ToString(),
            gameId = GameStateMachine.Instance.PlayingState.Data.gameId,
            type = hookedItem.tag.ToLower()
        };
    }

    private object CreateFinishEncryptObject(long timestamp)
    {
        return new FinishEncryptObject
        {
            timestamp = timestamp.ToString(),
            gameId = GameStateMachine.Instance.PlayingState.Data.gameId,
            type = GamePlayUI.Instance.GameOverPanel.FinishType.ToString()
        };
    }

    private IEnumerator IEPostRequest(string endpoint, object data, Action<string> onComplete = null,
        Action<string> onError = null)
    {
        string jsonData = JsonUtility.ToJson(data);
        string encryptedData = EncryptData(jsonData);

        using var request = UnityWebRequest.Post($"{url}/{endpoint}", encryptedData, "application/json");
        request.SetRequestHeader("wallet", wallet.ToLower());
        yield return request.SendWebRequest();
        HandleResponse(request, onComplete, onError);
    }

    private string EncryptData(string jsonData)
    {
        AesEncryption aes = new AesEncryption("yyr33qEVpWY1a0Kp4o1TyJLBvCRrvaUy");
        var encryptedObject = new EncryptedObject { encryptedData = aes.Encrypt(jsonData) };
        return JsonUtility.ToJson(encryptedObject);
    }

    private void HandleResponse(UnityWebRequest request, Action<string> onComplete, Action<string> onError)
    {
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error: {request.error}");
            ErrorPopup.Instance.ShowError(request.error);
            onError?.Invoke(request.error);
        }
        else
        {
            var responseJson = request.downloadHandler.text;
            Debug.Log($"Api Response: {responseJson}");
            ApiResponse responseObj = JsonUtility.FromJson<ApiResponse>(responseJson);
            if (responseObj.success)
                onComplete?.Invoke(responseJson);
            else
                ErrorPopup.Instance.ShowError(responseObj.message);
        }
    }
}


public class AesEncryption
{
    private readonly byte[] key;
    private readonly byte[] iv;

    public AesEncryption(string keyString)
    {
        key = Encoding.UTF8.GetBytes(keyString);
        iv = new byte[16]; // Khởi tạo IV với giá trị 0  
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string encryptedText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cs))
            {
                return reader.ReadToEnd();
            }
        }
    }
}