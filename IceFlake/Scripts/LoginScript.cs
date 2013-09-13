using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.API;
using IceFlake.Client.Scripts;

namespace IceFlake.Scripts
{
    public class LoginScript : Script
    {
        public LoginScript()
            : base("Login", "Uncatalogued")
        { }

        const string
            EMAIL = "E-Mail",
            PASSWORD = "Password",
            ACCOUNT = "WoW1",
            REALM = "Realm",
            CHARACTER = "Character";

        public override void OnTick()
        {
            Frame.Hide("ScriptErrorsFrame");

            if (API.Login.AccountSelectVisible)
            {
                API.Login.SelectGameAccount(ACCOUNT);
            }
            else if (API.Login.LoginScreenVisible)
            {
                API.Login.DoLogin(EMAIL, PASSWORD);
            }
            else if (API.Login.RealmFrameVisible)
            {
                API.Login.SelectRealm(REALM);
            }
            else if (API.Login.CharSelectVisible)
            {
                API.Login.SelectCharacter(CHARACTER);
                API.Login.EnterWorld();
                Stop();
            }

            Sleep(2000);
        }
    }
}
