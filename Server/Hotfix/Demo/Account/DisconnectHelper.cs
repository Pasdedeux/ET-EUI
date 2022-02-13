/*======================================
* 项目名称 ：ET.Demo.Account
* 项目描述 ：
* 类 名 称 ：DisconnectHelper
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/7 16:08:31
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
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect( this Session self )
        {
            if (self==null || self.IsDisposed)
            {
                return;
            }

            long instanceID = self.InstanceId;

            await TimerComponent.Instance.WaitAsync(1000);

            if (self.InstanceId != instanceID)
            {
                return;

            }

            self.Dispose();
        }
    }
}
