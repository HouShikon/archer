using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : BaseManager
{
    public RequestManager(GameFacade gameFacede) : base(gameFacede) { }
    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();
    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        requestDict.Add(actionCode, request);
    }
    public void RemoveRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);

    }
    public void HandleResponse(ActionCode actionCode, string data)
    {
        BaseRequest request = requestDict.TryGet<ActionCode, BaseRequest>(actionCode);
        if(request==null)
        {
            Debug.LogWarning("无法得到ActionCode");
            return;

        }
        request.OnResponse(data);
    }
}
