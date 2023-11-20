using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class LoginPannel : BasePanel
{
    public Button closeButton;
    public Button StartButton;
    private InputField username;
    private InputField password;
    private LoginRequest loginRequest;
    //public Button StartButton;

    private void Start()
    {

        loginRequest = GetComponent<LoginRequest>();
        username = transform.Find("NameLabel/InputField").GetComponent<InputField>();
        password = transform.Find("PassWordLabel/InputField").GetComponent<InputField>();

        closeButton.onClick.AddListener(OnCloseClick);
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        EnterAnimation();


    }

    public override void OnPause()
    {
        HideAnimation();
    }

    public override void OnResume()
    {
        EnterAnimation();
    }

    public override void OnExit()
    {
        HideAnimation();

    }
    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();

    }

    private void OnLoginClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(username.text))
        {
            msg += "请输入用户名";
        }
        else if(string.IsNullOrEmpty(password.text))
        {
            msg += "请输入密码";
        }
        if(msg!="")
        {
            uiMng.ShowMessage(msg);
            
            return;
        }

        loginRequest.SendRequest(username.text, password.text);
    }

    public void OnloginResponse(ReturnCode returnCode)
    {
        if(returnCode==ReturnCode.Success)
        {
            uiMng.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            uiMng.ShowMessageSync("用户名或密码错误");
        }
    }
    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);
    }
    

    private void EnterAnimation()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }
    private void HideAnimation()
    {
        transform.DOScale(0, 0.3f);
        transform.DOLocalMoveX(1000, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }
}
