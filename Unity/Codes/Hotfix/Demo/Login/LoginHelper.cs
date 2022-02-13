using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string account, string password)
        {
            A2C_TestLoginAccount a2C_TestLoginAccount = null;
            Session session = null;

            try
            {
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
                a2C_TestLoginAccount = (A2C_TestLoginAccount)await session.Call(new C2A_TestLoginAccount() { AccountName = account, AccountPassword = password });

            }
            catch (Exception e)
            {
                session?.Dispose();
                Log.Error(e.ToString());

                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_TestLoginAccount.Error != ErrorCode.ERR_Success)
            {
                return a2C_TestLoginAccount.Error;
            }
            else
            {
                Log.Debug("登录成功!");
            }

            zoneScene.AddComponent<SessionComponent>().Session = session;
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();

            zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_TestLoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountID = a2C_TestLoginAccount.AccountId;

            return ErrorCode.ERR_Success;

            //try
            //{
            //    // 创建一个ETModel层的Session
            //    R2C_Login r2CLogin;
            //    Session session = null;
            //    try
            //    {
            //        session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
            //        {
            //            r2CLogin = (R2C_Login) await session.Call(new C2R_Login() { Account = account, Password = password });
            //        }
            //    }
            //    finally
            //    {
            //        session?.Dispose();
            //    }

            //    // 创建一个gate Session,并且保存到SessionComponent中
            //    Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2CLogin.Address));
            //    gateSession.AddComponent<PingComponent>();
            //    zoneScene.AddComponent<SessionComponent>().Session = gateSession;
				
            //    G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(
            //        new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId});

            //    Log.Debug("登陆gate成功!");

            //    await Game.EventSystem.PublishAsync(new EventType.LoginFinish() {ZoneScene = zoneScene});
            //}
            //catch (Exception e)
            //{
            //    Log.Error(e);
            //}
        } 


        public static async ETTask<int> GetServerInfo( Scene zoneScene )
        {
            A2C_GetServerInfos a2C_GetServerInfos = null;

            try
            {
                a2C_GetServerInfos = (A2C_GetServerInfos)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfos()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountID,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                });
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }

            if (a2C_GetServerInfos.Error != ErrorCode.ERR_Success)
            {
                return a2C_GetServerInfos.Error;
            }

            //todo 用AddChild方式创建了一个对象，并放置于Scene树中
            foreach (var item in a2C_GetServerInfos.ServerInfoProtoList )
            {
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfoComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(item);
                zoneScene.GetComponent<ServerInfoComponent>().serverInfosList.Add(serverInfo);
            }


            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }


        public static async ETTask<int> CreateRole(Scene zoneScene, string name)
        {
            A2C_CreateRole a2C_CreateRole = null;

            //服务器返回
            try
            {
                a2C_CreateRole = (A2C_CreateRole)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_CreateRole()
                {
                    AccountId = zoneScene.GetComponent<AccountInfoComponent>().AccountID,
                    Token = zoneScene.GetComponent<AccountInfoComponent>().Token,
                    Name = name,
                    ServerId = zoneScene.GetComponent<ServerInfoComponent>().CurrentServerId
                }) ;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetWorkError;
            }


            if (a2C_CreateRole.Error!= ErrorCode.ERR_Success)
            {
                Log.Error(a2C_CreateRole.Error.ToString());

                return a2C_CreateRole.Error;
            }

            RoleInfo newRoleInfo = zoneScene.GetComponent<RoleInfoComponent>().AddChild<RoleInfo>();
            newRoleInfo.FromMessage(a2C_CreateRole.RoleInfoProto);
            zoneScene.GetComponent<RoleInfoComponent>().roleInfos.Add(newRoleInfo);

            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }
    }
}