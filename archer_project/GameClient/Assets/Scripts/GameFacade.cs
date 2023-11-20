
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    private static GameFacade _instance;
    public static GameFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameFacade").GetComponent<GameFacade>();
            }
            return _instance;
        }
    }
    private RequestManager requestManager;
    private PlayerManager playerManager;
    private AudioManager audioManager;
    private CameraManager cameraManager;
    private UIManager uiManager;
    private ClientManager clientManager;

    private bool isEnterPlaying = false;
    

    // Start is called before the first frame update
    void Start()
    {
        InitManager();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateManager();
        if(isEnterPlaying)
        {
            EnterPlaying();
            isEnterPlaying = false;
        }
    }
    private void InitManager()
    {
        requestManager=new RequestManager(this);
        playerManager=new PlayerManager(this);
        audioManager=new AudioManager(this);
        cameraManager=new CameraManager(this);
        uiManager=new UIManager(this);
        clientManager=new ClientManager(this);

        requestManager.OnInit();
        playerManager.OnInit();
        audioManager.OnInit();
        cameraManager.OnInit();
        uiManager.OnInit();
        clientManager.OnInit();
        
    }
    private void OnDestroyManager()
    {
        requestManager.OnDestory();
        playerManager.OnDestory();
        audioManager.OnDestory();
        cameraManager.OnDestory();
        uiManager.OnDestory();
        clientManager.OnDestory();
    }

    private void UpdateManager()
    {
        requestManager.Update();
        playerManager.Update();
        audioManager.Update();
        cameraManager.Update();
        uiManager.Update();
        clientManager.Update();
    }
    private void OnDestroy()
    {
        OnDestroyManager();
    }
    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        requestManager.AddRequest(actionCode, request);
    }
    public void RemoveRequest(ActionCode actionCode)
    {
        requestManager.RemoveRequest(actionCode);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestManager.HandleResponse(actionCode, data);
    }
    public void ShowMessage(string msg)
    {
        uiManager.ShowMessage(msg);
    }
    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
        clientManager.SendRequest(requestCode, actionCode, data);
    }

    public void PlayBgSound(string soundName)
    {
        audioManager.PlayBgSound(soundName);
    }
    public void PlayNormalSound(string soundName)
    {
        audioManager.PlayNormalSound(soundName);
    }

    public void SetUserData(UserData ud)
    {
        playerManager.UserData = ud;
    }
    public UserData GetUserData()
    {
        return playerManager.UserData;
    }
    public void SetCurrentRoleType(RoleType rt)
    {
        playerManager.SetCurrentRoleType(rt);
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return playerManager.GetCurrentRoleGameObject();
    }

    private void EnterPlaying()
    {
        playerManager.SpawnRoles();
        cameraManager.FollowRole();
    }
    public void EnterPlayingSync()
    {
        isEnterPlaying = true;
    }
    public void StartPlaying()
    {
        playerManager.AddControlScript();
        playerManager.CreateSyncRequest();
    }

    public void SendAttack(int damage)
    {
        playerManager.SendAttack(damage);
    }
    public void GameOver()
    {
        cameraManager.WalkthroughScene();
        playerManager.GameOver();
    }
    public void UpdateResult(int totalCount, int winCount)
    {
        playerManager.UpdateResult(totalCount, winCount);
    }
}
