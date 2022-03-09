using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ET
{
	public static  class DlgLoginSystem
	{

		public static void RegisterUIEvent(this DlgLogin self)
		{
			self.View.E_LoginButton.AddListener(() => { self.OnLoginClickHandler();});


			self.View.E_ChooseServer1Button.AddListener(() => { self.OnClickServer1(); });
			self.View.E_ChooseServer2Button.AddListener(() => { self.OnClickServer2(); });
			self.View.E_EnterServerButton.AddListener(() => { self.OnClickEnterServer(); });
			self.View.E_CreateRoleButton.AddListener(() => { self.OnClickCreateRole(); });
			self.View.E_DeleteRoleButton.AddListener(() => { self.OnClickDeleteRole(); });
			self.View.E_EnterPlayButton.AddListener(() => { self.OnClickEnterDobby(); });
			self.View.E_GetRealmKeyButton.AddListener(() => { self.OnClickGetRealmKey(); });
		}

		public static void ShowWindow(this DlgLogin self, Entity contextData = null)
		{
			
		}
		
		public static async void OnLoginClickHandler(this DlgLogin self)
		{
			GUITest.zonScene = self.DomainScene();

			int errorCode  = await LoginHelper.Login(
				self.DomainScene(), 
				ConstValue.LoginAddress, 
				self.View.E_AccountInputField.GetComponent<InputField>().text, 
				self.View.E_PasswordInputField.GetComponent<InputField>().text);

			if (errorCode != ErrorCode.ERR_Success)
            {
				Log.Error(errorCode.ToString());
				return;
            }


			errorCode = await LoginHelper.GetServerInfo(self.ZoneScene());
			if (errorCode != ErrorCode.ERR_Success)
			{
				Log.Error(errorCode.ToString());
				return;
			}


			//self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
			//self.DomainScene().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Lobby);
		}

		public static async void OnClickServer1(this DlgLogin self) 
		{
			self.DomainScene().GetComponent<ServerInfoComponent>().CurrentServerId = 1;
			await ETTask.CompletedTask;
		}

		public static async void OnClickServer2(this DlgLogin self)
		{
			self.DomainScene().GetComponent<ServerInfoComponent>().CurrentServerId = 2;
			await ETTask.CompletedTask;
		}

		public static async void OnClickEnterServer(this DlgLogin self)
		{
			//获取指定区服信息角色信息
			int errorCode = await LoginHelper.GetRoles(self.ZoneScene());

			if (errorCode != ErrorCode.ERR_Success)
			{
				Log.Error(errorCode.ToString());
				return;
			}

			Log.Debug("LoginHelper.GetRoles");
		}

		public static async void OnClickCreateRole(this DlgLogin self)
		{
			int errorCode = await LoginHelper.CreateRole(self.ZoneScene(), "创建角色");

			if (errorCode != ErrorCode.ERR_Success)
			{
				Log.Error(errorCode.ToString());
				return;
			}

			Log.Debug("LoginHelper.CreateRole");
		}

		public static async void OnClickDeleteRole(this DlgLogin self)
		{
			int errorCode = await LoginHelper.DeleteRole(self.ZoneScene());

			if (errorCode != ErrorCode.ERR_Success)
			{
				Log.Error(errorCode.ToString());
				return;
			}

			Log.Debug("LoginHelper.DeleteRole");
		}

		public static async void OnClickEnterDobby(this DlgLogin self)
		{
            int errorCode = await LoginHelper.EnterGame(self.ZoneScene());

            if (errorCode != ErrorCode.ERR_Success)
            {
                Log.Error(errorCode.ToString());
                return;
            }

            Log.Debug("LoginHelper.EnterGame");

            await ETTask.CompletedTask;
		}


		public static async void OnClickGetRealmKey(this DlgLogin self)
		{
			int errorCode = await LoginHelper.GetRealmKey(self.ZoneScene());

			if (errorCode != ErrorCode.ERR_Success)
			{
				Log.Error(errorCode.ToString());
				return;
			}

			Log.Debug("LoginHelper.GetRealmKey");

			await ETTask.CompletedTask;
		}


		public static void HideWindow(this DlgLogin self)
		{

		}
	}
}
