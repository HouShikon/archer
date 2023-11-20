using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Request
{
    public class RegisterRequest : BaseRequest
    {

        private RegisterPannel registerPanel;
        public override void Awake()
        {
            requestCode = RequestCode.User;
            actionCode = ActionCode.Register;
            registerPanel = GetComponent<RegisterPannel>();
            base.Awake();
        }
        public void SendRequest(string username, string password)
        {
            string data = username + "," + password;
            base.SendRequest(data);
        }

        public override void OnResponse(string data)
        {
            ReturnCode returnCode = (ReturnCode)int.Parse(data);
            registerPanel.OnRegisterResponse(returnCode);
        }
    }
}
