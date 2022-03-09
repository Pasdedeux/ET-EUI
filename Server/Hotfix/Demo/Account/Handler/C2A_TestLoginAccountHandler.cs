/*======================================
* 项目名称 ：ET.Demo.Account.Handler
* 项目描述 ：
* 类 名 称 ：C2A_TestLoginAccountHandler
* 类 描 述 ：
* 命名空间 ：ET.Demo.Account.Handler
* 机器名称 ：DEREK-SURFACEPR 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：LHW
* 创建时间 ：2022/2/5 18:32:28
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
    public class C2A_TestLoginAccountHandler : AMRpcHandler<C2A_TestLoginAccount, A2C_TestLoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_TestLoginAccount request, A2C_TestLoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求到错误Scene： {session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockComponent>() != null)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            //if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.AccountPassword))
            //{
            //    response.Error = ErrorCode.ERR_NetWorkError;
            //    reply();
            //    session.Disconnect().Coroutine();
            //    return;
            //}


            if (AccountNameIsWrong())
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }


            if (AccountPasswordIsWrong())
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }


            using (session.AddComponent<SessionLockComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.AccountLoginConflict, request.AccountName.GetHashCode()))
                {
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(e => e.AccountName == request.AccountName);
                    Account account = null;
                    if (accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);

                        if (account.AccountType == (int)AccoutType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_NetWorkError;
                            reply();
                            session.Disconnect().Coroutine();
                            return;
                        }


                        if (!account.AccountPassword.Equals(account.AccountPassword))
                        {
                            response.Error = ErrorCode.ERR_NetWorkError;
                            reply();
                            session.Disconnect().Coroutine();
                            return;
                        }
                    }
                    else
                    {
                        //查不到就直接新创建一个，走创建账号流程
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName;
                        account.AccountPassword = request.AccountPassword;
                        account.CreateTime = TimeHelper.ServerNow();
                        account.AccountType = (int)AccoutType.General;

                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
                    }


                    //给登录中心服发送消息
                    StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                    long loginInstaceId = startSceneConfig.InstanceId;
                    var l2AResult = (L2A_LoginAccountResponse)await ActorMessageSenderComponent.Instance.Call(loginInstaceId, new A2L_LoginAccountRequest() { AccountId = account.Id });

                    if (l2AResult.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = l2AResult.Error;

                        reply();
                        session?.Disconnect().Coroutine();
                        account?.Dispose();

                        return;
                    }


                    //如不存在，則爲0
                    //获取此用户ID对应的Session信息，断开其Session连接
                    long accountSessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(account.Id);
                    var otherSession = Game.EventSystem.Get(accountSessionInstanceId) as Session;
                    otherSession?.Send(new A2C_Diconnect() { Error = 0 }); 
                    otherSession?.Disconnect().Coroutine();
                    session.DomainScene().GetComponent<AccountSessionsComponent>().Add(account.Id, session.InstanceId);
                    session.AddComponent<AccountCheckOutTimeComponent, long>(account.Id);


                    string token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, token);

                    response.AccountId = account.Id;
                    response.Token = token;

                    reply();
                    account?.Dispose();


                }
            }
        }

        private bool AccountPasswordIsWrong()
        {
            return false;
        }

        private bool AccountNameIsWrong()
        {
            return false;
        }
    }
}
