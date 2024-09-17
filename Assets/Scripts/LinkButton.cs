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

    [DllImport("__Internal")]
    private static extern void OnClickLinkButton();

    private void CallReact()
    {
        Debug.LogWarning("Call React");
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        OnClickLinkButton();
#endif
    }
}