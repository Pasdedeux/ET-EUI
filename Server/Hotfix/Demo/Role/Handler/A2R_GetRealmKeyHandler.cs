/*======================================
* 项目名称 ：ET.Demo.Role.Handler
* 项目描述 ：
* 类 名 称 ：A2R_GetRealmKeyHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Role.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/14 18:32:48
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
    public class A2R_GetRealmKeyHandler : AMActorRpcHandler<Scene, A2R_GetRealmKey, R2A_GetRealmKey>
    {
        protected override async ETTask Run(Scene unit, A2R_GetRealmKey request, R2A_GetRealmKey response, Action reply)
        {
            if (unit.DomainScene().SceneType != SceneType.Realm) 
            {
                Log.Error("请求的Scene场景错误");
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                return;
            }

            string key = TimeHelper.ServerNow().ToString() + RandomHelper.RandInt64().ToString();
            unit.GetComponent<TokenComponent>().Remove(request.AccountId);
            unit.GetComponent<TokenComponent>().Add(request.AccountId, key);
            response.RealmKey = key;
            reply();

            await ETTask.CompletedTask;
        }
    }
}
