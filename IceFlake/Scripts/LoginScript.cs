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

        public override void OnTick()
        {
            if (Manager.ObjectManager.IsInGame)
                return;

            ////var s = Program.CharacterSettings;
            ////if (s == null || Extensions.IsAnyNull(s.WoWAccount, s.WoWEmail, s.WoWPassword))
            ////    return;

            //Frame.Hide("ScriptErrorsFrame");

            //if (API.Login.AccountSelectVisible)
            //{
            //    API.Login.SelectGameAccount(s.WoWAccount);
            //}
            //else if (API.Login.LoginScreenVisible)
            //{
            //    API.Login.DoLogin(s.WoWEmail, s.WoWPassword);
            //}
            //else if (API.Login.RealmFrameVisible)
            //{
            //    API.Login.SelectRealm(s.Realm);
            //}
            //else if (API.Login.CharSelectVisible)
            //{
            //    API.Login.SelectCharacter(s.Character);
            //    API.Login.EnterWorld();
            //    Stop();
            //}

            //Sleep(10000);
        }
    }
}
