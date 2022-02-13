﻿/*======================================
* 项目名称 ：ET.Demo.Account.Handler
* 项目描述 ：
* 类 名 称 ：L2G_DisconnectGateUnitHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/8 18:00:37
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
    public class L2G_DisconnectGateUnitHandler : AMActorRpcHandler<Scene, L2G_DisconnectGateUnit, G2L_DisconnectGateUnit>
    {
        protected override async ETTask Run(Scene unit, L2G_DisconnectGateUnit request, G2L_DisconnectGateUnit response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GateLoginLock, accountId.GetHashCode()))
            {
                PlayerComponent playerComponent = unit.GetComponent<PlayerComponent>();
                Player gateUnit = playerComponent.Get(accountId);

                if (gateUnit == null)
                {
                    reply();
                    return;
                }

                playerComponent.Remove(accountId);
                gateUnit.Dispose();
            }

            reply();

            await ETTask.CompletedTask;
        }
    }
}
