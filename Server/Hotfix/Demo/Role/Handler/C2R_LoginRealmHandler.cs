/*======================================
* 项目名称 ：ET.Demo.Role.Handler
* 项目描述 ：
* 类 名 称 ：C2R_LoginRealmHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Role.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/14 19:24:16
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Derek Liu 2022. All rights reserved.
*******************************************************************
======================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class C2R_LoginRealmHandler : AMRpcHandler<C2R_LoginRealm, R2C_LoginRealm>
    {
        protected override async ETTask Run(Session session, C2R_LoginRealm request, R2C_LoginRealm response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Realm)
            {
                Log.Error($"请求到错误Scene： {session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            if (session.GetComponent<SessionLockComponent>() != null)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            if (token == null || token != request.RealmTokenKey)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply?.Invoke();
                session?.Disconnect().Coroutine();
                return;
            }


            session.DomainScene().GetComponent<TokenComponent>().Remove(request.AccountId);

            using (session.AddComponent<SessionLockComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait((int)CoroutineLockType.LoginRealm, request.AccountId))
                {
                    //固定分配一个Gate
                    StartSceneConfig config = RealmGateAddressHelper.GetGate(session.DomainZone(), request.AccountId);

                    //向Gate请求一个key
                    G2R_GetLoginGateKey g2R_GetLoginKey = (G2R_GetLoginGateKey)await MessageHelper.CallActor(config.InstanceId, new R2G_GetLoginGateKey()
                    {
                        AccountId = request.AccountId,
                    });

                    if (g2R_GetLoginKey.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = g2R_GetLoginKey.Error;
                        reply();
                        return;
                    }

                    response.GateSessionKey = g2R_GetLoginKey.GetSessionKey;
                    response.GateAddress = config.OuterIPPort.ToString();
                    reply();

                    session?.Disconnect().Coroutine();
                }
            }


            await ETTask.CompletedTask;
        }
    }
}
