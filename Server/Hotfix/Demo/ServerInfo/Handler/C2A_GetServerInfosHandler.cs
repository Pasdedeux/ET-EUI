/*======================================
* 项目名称 ：ET.Demo.ServerInfo.Handler
* 项目描述 ：
* 类 名 称 ：C2A_GetServerInfosHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.ServerInfo.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/9 15:14:59
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
    public class C2A_GetServerInfosHandler : AMRpcHandler<C2A_GetServerInfos, A2C_GetServerInfos>
    {
        protected override async ETTask Run(Session session, C2A_GetServerInfos request, A2C_GetServerInfos response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account )
            {
                Log.Error("请求场景错误");
                session?.Dispose();
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


            foreach (var item in session.GetComponent<ServerInfoManagerComponent>().ServerInfos)
            {
                response.ServerInfoProtoList.Add(item.ToMessage());
            }
            reply();

            await ETTask.CompletedTask;
        }
    }
}
