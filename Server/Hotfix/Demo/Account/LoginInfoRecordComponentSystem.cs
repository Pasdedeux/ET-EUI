/*======================================
* 项目名称 ：ET.Demo.Account
* 项目描述 ：
* 类 名 称 ：LoginInfoRecordComponentSystem
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/8 16:47:44
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

    public class LoginInfoRecordComponentDestroySystem : DestroySystem<LoginInfoRecordComponent>
    {
        public override void Destroy(LoginInfoRecordComponent self)
        {
            self.AccountLoginInfoDict.Clear();
        }
    }




    public static class LoginInfoRecordComponentSystem
    {

        public static void Add( this LoginInfoRecordComponent self, long key, int value )
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict[key] = value;
                return;
            }
            self.AccountLoginInfoDict.Add(key, value);
        }


        public static void Remove(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDict.ContainsKey(key))
            {
                self.AccountLoginInfoDict.Remove(key);
            }
        }

        public static int Get(this LoginInfoRecordComponent self, long key)
        {
            if (self.AccountLoginInfoDict.TryGetValue(key,out int zone))
            {
                return zone;
            }
            return -1;
        }


        public static bool IsExist( this LoginInfoRecordComponent self, long key )
        {
            return self.AccountLoginInfoDict.ContainsKey(key);
        }

    }
}
