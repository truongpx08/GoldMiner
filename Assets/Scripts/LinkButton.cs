using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LinkButton : TruongUIButton
{
    protected override void Start()
    {
        base.Start();
        AddActionToButton(CallReact);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.button.interactable = true;
    }

    [DllImport("__Internal")]
    private static extern void OnClickLinkButton();

    private void CallReact()
    {
        this.button.interactable = false;
        Debug.LogWarning("Call React");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        OnClickLinkButton();
#endif
    }
}