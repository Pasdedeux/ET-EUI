/*======================================
* 项目名称 ：ET.Demo.Account.Handler
* 项目描述 ：
* 类 名 称 ：A2L_LoginAccountRequestHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/8 17:11:32
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
    public class A2L_LoginAccountRequestHandler : AMActorRpcHandler<Scene, A2L_LoginAccountRequest, L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response, Action reply)
        {
            long accountId = request.AccountId;

            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, accountId.GetHashCode()))
            {
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(accountId))
                {
                    reply();
                    return;
                }


                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone, accountId);

                var g2l_disconnectGateUnit = (G2L_DisconnectGateUnit)await MessageHelper.CallActor(gateConfig.InstanceId, new L2G_DisconnectGateUnit() { AccountId = accountId });
                response.Error = g2l_disconnectGateUnit.Error;
                reply();
            }
        }
    }
}
