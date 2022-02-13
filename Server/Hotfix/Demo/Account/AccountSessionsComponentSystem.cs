/*======================================
* 项目名称 ：ET.Demo.Account
* 项目描述 ：
* 类 名 称 ：AccountSessionsComponentSystem
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/8 15:05:20
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

    public class AccountSessionComponentDestroySystem : DestroySystem<AccountSessionsComponent>
    {
        public override void Destroy(AccountSessionsComponent self)
        {
            self.AccountSessionDictionary.Clear();
        }
    }

    public static class AccountSessionsComponentSystem
    {
        public static long Get(this AccountSessionsComponent self, long accountId)
        {
            if (!self.AccountSessionDictionary.TryGetValue(accountId, out long instanceId))return 0;
            return instanceId;
        }

        public static void Add(this AccountSessionsComponent self, long accountId, long sessionInstanceId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId))
            {
                self.AccountSessionDictionary[accountId] = sessionInstanceId;
                return;
            }
            self.AccountSessionDictionary.Add(accountId, sessionInstanceId);
        }


        public static void Remove(this AccountSessionsComponent self, long accountId)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountId)) self.AccountSessionDictionary.Remove(accountId);
        }
    }
}
