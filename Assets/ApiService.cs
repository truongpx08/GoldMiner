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
    PostStart
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
        var levelObject = new LevelObject
            { timestamp = timestamp.ToString(), level = Option.Instance.CurrentOption.Level.ToString() };
        var levelJson = JsonUtility.ToJson(levelObject);
        Debug.Log("levelJson: " + levelJson);

        AesEncryption aes = new AesEncryption("yyr33qEVpWY1a0Kp4o1TyJLBvCRrvaUy");

        // Mã hóa dữ liệu  
        string encryptedData = aes.Encrypt(levelJson);
        Debug.Log($"Encrypted Data: {encryptedData}");

        var encryptedObject = new EncryptedObject { encryptedData = encryptedData };
        var encryptedJson = JsonUtility.ToJson(encryptedObject);
        Debug.Log("Encrypted Json: " + encryptedJson);

        // // Giải mã dữ liệu  
        // string decryptedData = aes.Decrypt(encryptedData);
        // Debug.Log($"Decrypted Data: {decryptedData}");

        // PostRequest

        using var request = UnityWebRequest.Post($"{this.url}/crystal/start",
            encryptedJson, "application/json");
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
            Debug.Log($"Response: {responseJson}");
            ApiResponse responseObj = JsonUtility.FromJson<ApiResponse>(responseJson);
            if (responseObj.success)
                onComplete?.Invoke(request.downloadHandler.text);
            else
                ErrorPopup.Instance.ShowError(responseObj.message);
        }
    }
}

[Serializable]
public class LevelObject
{
    public string timestamp;
    public string level;
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