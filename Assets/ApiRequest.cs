using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

public class ApiRequest : TruongSingleton<ApiRequest>
{
    [SerializeField] private string wallet; // Đặt giá trị cho wallet ở đây  
    [SerializeField] private string url;

    [Button]
    public void GetUserData(Action<string> onComplete = null, Action<string> onError = null)
    {
        StartCoroutine(IE());

        IEnumerator IE()
        {
            UnityWebRequest request = UnityWebRequest.Get(this.url);
            request.SetRequestHeader("wallet", wallet.ToLower());

            yield return request.SendWebRequest();

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                onComplete?.Invoke(request.downloadHandler.text);
            }
        }
    }

    public void HandleMessage(string eventID, object msgData, Action onSuccess)
    {
        var iDic = (IDictionary)msgData;
        // LogEventOnReceive(eventID, iDic);

        bool success = (bool)iDic["success"];
        if (!success)
        {
            if (!ErrorPopup.IsAvailable) return;
            ErrorPopup.Instance.ShowError((string)(iDic["message"]));
            return;
        }

        onSuccess?.Invoke();
    }
}