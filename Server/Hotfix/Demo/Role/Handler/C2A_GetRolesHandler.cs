/*======================================
* 项目名称 ：ET.Demo.Role.Handler
* 项目描述 ：
* 类 名 称 ：C2A_GetRolesHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Role.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/14 11:27:03
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
    public class C2A_GetRolesHandler : AMRpcHandler<C2A_GetRoles, A2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2A_GetRoles request, A2C_GetRoles response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求的Scene错误");
                session.Dispose();
                return;
            }


            if (session.GetComponent<SessionLockComponent>() != null )
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);
            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }


            using (session.AddComponent<SessionLockComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRole, request.AccountId))
                {
                    DBManagerComponent db = DBManagerComponent.Instance;
                    var roleInfo = await db.GetZoneDB(session.DomainZone()).Query<RoleInfo>(e => e.AccountId == request.AccountId && e.ServerId == request.ServerId && e.State == (int)RoleInfoState.Normal);

                    if (roleInfo == null || roleInfo.Count == 0)
                    {
                        reply();
                        return;
                    }


                    foreach (var item in roleInfo)
                    {
                        response.RoleInfo.Add(item.ToMessage());
                        item?.Dispose();
                    }
                    roleInfo.Clear();

                    reply();
                }
            }
        }
    }
}
