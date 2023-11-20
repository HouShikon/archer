using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager 
{
    protected GameFacade facede;

    public BaseManager(GameFacade facede)
    {
        this.facede = facede;
    }
    public virtual void Update()
    {

    }
    public virtual void OnInit() { }
    public virtual void OnDestory() { }
}
