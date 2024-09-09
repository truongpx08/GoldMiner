using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum EApiType
{
    GetUserData,
    PostStart,
    PostMove,
}

public class ApiService : TruongSingleton<ApiService>
{
    [SerializeField] private string wallet;
    [SerializeField] private string url = "https://refactor.faraland.moonknightlabs.com";

    public void Request(EApiType type, Action<string> onComplete = null, Action<string> onError = null)
    {
        switch (type)
        {
            case EApiType.GetUserData:
                StartCoroutine(IEGetUserData(onComplete, onError));
                break;
            case EApiType.PostStart:
                StartCoroutine(IEPostStart(onComplete, onError));
                break;
            case EApiType.PostMove:
                StartCoroutine(IEPostMove(onComplete, onError));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }


    private IEnumerator IEGetUserData(Action<string> onComplete = null, Action<string> onError = null)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{this.url}/crystal");
        request.SetRequestHeader("wallet", wallet.ToLower());

        yield return request.SendWebRequest();

        HandleResponse(request, onComplete, onError);
    }

    private IEnumerator IEPostStart(Action<string> onComplete = null, Action<string> onError = null)
    {
        long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        var levelObject = new StartObject
        {
            timestamp = timestamp.ToString(),
            level = Option.Instance.CurrentOption.Level.ToString()
        };

        yield return IEPostRequest("crystal/start", levelObject, onComplete, onError);
    }

    private IEnumerator IEPostMove(Action<string> onComplete, Action<string> onError)
    {
        var hookedItem = MiningMachine.Instance.HookCollider.HookedItem;
        long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        var moveObject = new MoveObject
        {
            timestamp = timestamp.ToString(),
            gameId = GameStateMachine.Instance.PlayingState.Data.gameId,
            type = hookedItem.tag.ToLower()
        };

        yield return IEPostRequest("crystal/move", moveObject, onComplete, onError);
    }

    private IEnumerator IEPostRequest(string endpoint, object data, Action<string> onComplete = null,
        Action<string> onError = null)
    {
        // Chuyển đổi đối tượng thành JSON  
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log($"Json Data: {jsonData}");

        // Mã hóa dữ liệu  
        AesEncryption aes = new AesEncryption("yyr33qEVpWY1a0Kp4o1TyJLBvCRrvaUy");
        string encryptedData = aes.Encrypt(jsonData);
        Debug.Log($"Encrypted Data: {encryptedData}");

        var encryptedObject = new EncryptedObject { encryptedData = encryptedData };
        var encryptedJson = JsonUtility.ToJson(encryptedObject);

        // Gửi yêu cầu POST  
        using var request = UnityWebRequest.Post($"{this.url}/{endpoint}", encryptedJson, "application/json");
        request.SetRequestHeader("wallet", wallet.ToLower());
        yield return request.SendWebRequest();

        HandleResponse(request, onComplete, onError);
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
                onComplete?.Invoke(request.downloadHandler.text);
            else
                ErrorPopup.Instance.ShowError(responseObj.message);
        }
    }
}

[Serializable]
public class StartObject : TimestampObject
{
    public string level;
}

[Serializable]
public class MoveObject : TimestampObject
{
    public string gameId;
    public string type;
}

public class TimestampObject
{
    public string timestamp;
}

[Serializable]
public class EncryptedObject
{
    public string encryptedData;
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

[Serializable]
public class ApiResponse
{
    public bool success;
    public string message;
}