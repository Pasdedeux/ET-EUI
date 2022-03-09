/*======================================
* 项目名称 ：ET.Demo.Account.Handler
* 项目描述 ：
* 类 名 称 ：L2G_AddLoginRecordHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/15 16:14:03
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
    public class G2L_AddLoginRecordHandler : AMActorRpcHandler<Scene, G2L_AddLoginRecord, L2G_AddLoginRecord>
    {
        protected override async ETTask Run(Scene unit, G2L_AddLoginRecord request, L2G_AddLoginRecord response, Action reply)
        {
            long accountId = request.AccountId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginCenterLock, accountId.GetHashCode()))
            {
                unit.GetComponent<LoginInfoRecordComponent>().Remove(accountId);
                unit.GetComponent<LoginInfoRecordComponent>().Add(accountId, request.ServerId);
            }
            reply();
        }
    }
}
