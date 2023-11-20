using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPannel : BasePanel
{
    private Button loginButton;
    public override void OnEnter()
    {
        base.OnEnter();
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        loginButton.onClick.AddListener(OnLoginClick);
    }

    private void OnLoginClick()
    {
        uiMng.PushPanel(UIPanelType.Login);
        //loginButton.transform.DOScale(0, 0.3f).OnComplete(() => loginButton.gameObject.SetActive(false));
        //loginButton.gameObject.SetActive(false);
        loginButton.transform.DOScale(0, 0.3f);
        
    }

    //public override void OnPause()
    //{
    //    base.OnPause();
    //    Debug.Log("pause");

    //}
    public override void OnResume()
    {
        base.OnResume();
        //Debug.Log("resume");
        loginButton.gameObject.SetActive(true);
        //loginButton.gameObject.SetActive(true);
        loginButton.transform.DOScale(1, 0.3f);

    }
}
