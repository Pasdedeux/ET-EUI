/*======================================
* 项目名称 ：ET.Demo.Role.Handler
* 项目描述 ：
* 类 名 称 ：C2A_DeleteRoleHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Role.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/14 12:57:20
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
    public class C2A_DeleteRoleHandler : AMRpcHandler<C2A_DeleteRole, A2C_DeleteRole>
    {
        protected override async ETTask Run(Session session, C2A_DeleteRole request, A2C_DeleteRole response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
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
            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply?.Invoke();
                session?.Disconnect().Coroutine();
                return;
            }


            using (session.AddComponent<SessionLockComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRole, request.AccountId))
                {
                    DBManagerComponent db = DBManagerComponent.Instance;
                    var roleInfo = await db.GetZoneDB(request.ServerId).Query<RoleInfo>(e => /*e.Id == request.RoleInfoId && */e.ServerId == request.ServerId && e.State == (int)RoleInfoState.Normal);

                    if (roleInfo == null || roleInfo.Count<=0)
                    {
                        response.Error = ErrorCode.ERR_NetWorkError;
                        reply();
                        return;
                    }

                    var roleInfoItem = roleInfo[0];
                    session.AddChild(roleInfoItem);

                    roleInfoItem.State = (int)RoleInfoState.Freeze;

                    await DBManagerComponent.Instance.GetZoneDB(request.ServerId).Save(roleInfoItem);
                    response.DeleteRoleInfoId = roleInfoItem.Id;
                    roleInfoItem?.Dispose();

                    reply();
                }
            }



            await ETTask.CompletedTask;
        }
    }
}
