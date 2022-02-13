/*======================================
* 项目名称 ：ET.Demo.Role.Handler
* 项目描述 ：
* 类 名 称 ：C2A_CreateRoleHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Role.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/9 17:02:40
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
    public class C2A_CreateRoleHandler : AMRpcHandler<C2A_CreateRole, A2C_CreateRole>
    {
        protected override async ETTask Run(Session session, C2A_CreateRole request, A2C_CreateRole response, Action reply)
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

            if (string.IsNullOrEmpty( request.Name ))
            {
                response.Error = ErrorCode.ERR_CreateRoleNameError;
                reply();
                //toto 创角不断开连接，可以继续重新发送创角申请
                return;
            }

            using (session.AddComponent<SessionLockComponent>())
            {

                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRole, request.AccountId))
                {
                    var roleInfo = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<RoleInfo>(e => e.Name == request.Name && e.ServerId == request.ServerId && e.State == (int)RoleInfoState.Normal);
                    if (roleInfo != null && roleInfo.Count > 0)
                    {
                        //toto 当前存在该角色
                        response.Error = ErrorCode.ERR_CreateRoleNameSameError;
                        reply();

                        return;
                    }

                    RoleInfo newRoleInfo = session.AddChildWithId<RoleInfo>(IdGenerater.Instance.GenerateUnitId(request.ServerId));
                    newRoleInfo.Name = request.Name;
                    newRoleInfo.State = (int)RoleInfoState.Normal;
                    newRoleInfo.ServerId = request.ServerId;
                    newRoleInfo.AccountId = request.AccountId;
                    newRoleInfo.CreateTime = TimeHelper.ServerNow();
                    newRoleInfo.LastLoginTime = 0;

                    await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save(newRoleInfo);

                    response.RoleInfoProto = newRoleInfo.ToMessage();
                    reply();
                }

            }
            await ETTask.CompletedTask;
        }
    }
}
