/*======================================
* 项目名称 ：ET.Demo.Role.Handler
* 项目描述 ：
* 类 名 称 ：G2R_GetLoginGateKeyHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Role.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/14 19:37:50
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
    public class G2R_GetLoginGateKeyHandler : AMActorRpcHandler<Scene, R2G_GetLoginGateKey, G2R_GetLoginGateKey>
    {
        protected override async ETTask Run(Scene unit, R2G_GetLoginGateKey request, G2R_GetLoginGateKey response, Action reply)
        {
            if (unit.SceneType != SceneType.Gate)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                return;
            }

            string key = RandomHelper.RandInt64().ToString() + TimeHelper.ServerNow().ToString();
            unit.GetComponent<GateSessionKeyComponent>().Remove(request.AccountId);
            unit.GetComponent<GateSessionKeyComponent>().Add(request.AccountId, key);
            response.GetSessionKey = key;
            reply();

            await ETTask.CompletedTask;
        }
    }
}
