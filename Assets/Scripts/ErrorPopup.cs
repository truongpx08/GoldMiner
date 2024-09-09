using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ErrorPopup : TruongSingleton<ErrorPopup>
{
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI notificationText;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPopup();
        LoadNotificationText();
    }

    private void LoadNotificationText()
    {
        this.notificationText = content.Find(TruongConstant.NotificationText).GetComponent<TextMeshProUGUI>();
    }

    private void LoadPopup()
    {
        this.content = transform.Find(TruongConstant.CONTENT);
    }

    [Button]
    private void EnableContent()
    {
        EnableGo(content);
    }

    [Button]
    private void DisableContent()
    {
        DisableGo(content);
    }

    [Button]
    public void ShowError(string message)
    {
        this.notificationText.text = message;
        EnableContent();
    }
}