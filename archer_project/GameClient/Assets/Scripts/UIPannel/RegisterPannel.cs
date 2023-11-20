using Assets.Scripts.Request;
using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPannel : BasePanel
{
    private InputField username;
    private InputField password;
    private InputField rePassword;
    private RegisterRequest registerRequest;

    //private GameObject closeButton;
    public Button closeButton;

    private void Start()
    {

        registerRequest = GetComponent<RegisterRequest>();
        username = transform.Find("NameLabel/InputField").GetComponent<InputField>();
        password = transform.Find("PassWordLabel/InputField").GetComponent<InputField>();
        rePassword = transform.Find("RPassWordLabel/InputField").GetComponent<InputField>();
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        //closeButton = GameObject.Find("CloseButton");    
        //closeButton.GetComponent<Button>().onClick.AddListener(OnCloseClick);
        closeButton.onClick.AddListener(OnCloseClick);

    }
    public override void OnEnter()
    {
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.3f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.3f);

    }


    private void OnRegisterClick()
    {
        PlayClickSound();
        string msg = "";
        
        if (string.IsNullOrEmpty(username.text))
        {
            msg += "用户名不能为空";
        }
        else if (string.IsNullOrEmpty(password.text))
        {
            msg += "密码不能为空";
            //Debug.Log(msg);
        }
        else if (password.text != rePassword.text)
        {
            msg += "密码不一致";
            //Debug.Log(msg);
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
            return;
        }
        //进行注册 发送到服务器端
        Debug.Log(msg);
        registerRequest.SendRequest(username.text, password.text);
    }

    
    private void OnCloseClick()
    {
        PlayClickSound();
        //Debug.Log("close");
        transform.DOScale(0, 0.3f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.3f);
        tweener.OnComplete(() => uiMng.PopPanel());
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageSync("注册成功");
        }
        else
        {
            uiMng.ShowMessageSync("用户名重复");
        }
    }
}
