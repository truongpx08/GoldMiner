using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReopenUtils : TruongSingleton<ReopenUtils>
{
    [DllImport("__Internal")]
    private static extern void Reopen();

    public void OnReopenComplete(string reactJson)
    {
        Debug.Log("OnReopenComplete \n " + reactJson);

        var reactObj = JsonUtility.FromJson<ReactResponse>(reactJson);
        if (reactObj.success)
            ApiService.Instance.Request(EApiType.Reopen, json =>
            {
                var jsonObject = JsonUtility.FromJson<FinishData>(json);
                GamePlayUI.Instance.GameOverPanel.SetPoint(jsonObject.data.tamanXReward);
                GamePlayUI.Instance.GameOverPanel.SetTamanXBalance(jsonObject.data.totalTamanX);

                GamePlayUI.Instance.Loading.SetActive(false);
                Debug.Log("Earn data");
            });
        else
        {
            GamePlayUI.Instance.Loading.SetActive(false);
            ErrorPopup.Instance.ShowError(reactObj.message);
        }
    }

    public void CallReact()
    {
        Debug.Log("CallReact Reopen");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
         Reopen();
#endif
    }
}

[Serializable]
public class ReactResponse
{
    public bool success;
    public string message;
}