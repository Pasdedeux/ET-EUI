/*======================================
* 项目名称 ：ET.Demo.Account
* 项目描述 ：
* 类 名 称 ：TokenComponentSystem
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/7 13:58:14
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
    public static class TokenComponentSystem 
    {
        public static void Add(this TokenComponent self, long key, string token)
        {
            self.TokenDictionary.Add(key, token);
            self.TimeOutRemoveKey(key, token).Coroutine();
        }

        public static string Get(this TokenComponent self, long key)
        {
            string value = null;
            self.TokenDictionary.TryGetValue(key, out value);
            return value;
        }

        public static void Remove(this TokenComponent self, long key)
        {
            if (self.TokenDictionary.ContainsKey(key))
            {
                self.TokenDictionary.Remove(key);
            }
        }


        private static async ETTask TimeOutRemoveKey( this TokenComponent self, long key , string tokenKey )
        {
            await TimerComponent.Instance.WaitAsync(600000);

            string onlineToken = self.Get(key);
            if(!string.IsNullOrEmpty(onlineToken) && onlineToken == tokenKey)
            {
                self.Remove(key);
            }
        }

    }
}
