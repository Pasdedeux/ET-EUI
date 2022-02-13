/*======================================
* 项目名称 ：ET.Demo.ServerInfo
* 项目描述 ：
* 类 名 称 ：ServerInfoManagerComponentSystem
* 类 描 述 ：
* 命名空间 ：ET.Demo.ServerInfo
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/9 15:01:26
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

    public class ServerInfoManagerComponentAwakeSystem : AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }



    public class ServerInfoManagerComponentDestroySystem : DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach (var item in self.ServerInfos)
            {
                item?.Dispose();
            }

            self.ServerInfos.Clear();
        }
    }



    public class ServerInfoManagerComponentLoadSystem : LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }


    public static class ServerInfoManagerComponentSystem
    {
        /// <summary>
        /// 服务器初始化时获取对应数据库信息
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask Awake(this ServerInfoManagerComponent self)
        {
            var serverInfoList = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(e => true);

            if (serverInfoList == null || serverInfoList.Count <= 0)
            {
                Log.Error("serverInfo count is Zero");
                self.ServerInfos.Clear();
                var serverInfoConfigs = ServerInfoConfigCategory.Instance.GetAll();
                foreach (var item in serverInfoConfigs.Values)
                {
                    ServerInfo newServerInfo = self.AddChildWithId<ServerInfo>(item.Id);
                    newServerInfo.ServerName = item.ServerName;
                    newServerInfo.Status = (int)ServerStatus.Normal;
                    newServerInfo.ServerDesc = item.ServerDesc;
                    self.ServerInfos.Add(newServerInfo);

                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save<ServerInfo>(newServerInfo);


                }

                return;
            }

            foreach (var item in self.ServerInfos)
            {
                item?.Dispose();
            }
            self.ServerInfos.Clear();

            foreach (var item in serverInfoList)
            {
                self.AddChild(item);
                self.ServerInfos.Add(item);
            }

            await ETTask.CompletedTask;
        }
    }
}
