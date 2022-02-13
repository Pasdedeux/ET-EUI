/*======================================
* 项目名称 ：ET.Demo.Account
* 项目描述 ：
* 类 名 称 ：AccountCheckOutTimeComponentSystem
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/8 15:30:59
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
    [Timer(TimerType.AccountSessionCheckOutTime)]
    public class AccountSessionCheckOutTimer : ATimer<AccountCheckOutTimeComponent>
    {
        public override void Run(AccountCheckOutTimeComponent t)
        {
            try
            {
                t.DeleteSession();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }


    public class AccountCheckOutTimeComponentAwakeSystem : AwakeSystem<AccountCheckOutTimeComponent, long>
    {
        public override void Awake(AccountCheckOutTimeComponent self, long a)
        {
            self.AccountId = a;
            TimerComponent.Instance.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 600000, TimerType.AccountSessionCheckOutTime, self);
        }
    }


    public class AccountCheckOutTimeComponentDestroySystem : DestroySystem<AccountCheckOutTimeComponent>
    {
        public override void Destroy(AccountCheckOutTimeComponent self)
        {
            self.AccountId = 0;
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }


    public static class AccountCheckOutTimeComponentSystem
    {
        public static void DeleteSession(this AccountCheckOutTimeComponent self)
        {
            Session session = self.GetParent<Session>();
            //若当前会话和存储的账户id代表会话一致，则删除该存储信息，并断开连接
            long sessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(self.AccountId);
            if (sessionInstanceId != session.InstanceId)
            {
                session.DomainScene().GetComponent<AccountSessionsComponent>().Remove(self.AccountId);
            }
            session.Send(new A2C_Diconnect() { Error = 1 });
            session.Disconnect().Coroutine();
        }
    }
}
