
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class DlgLoginViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button E_LoginButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoginButton == null )
     			{
		    		this.m_E_LoginButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Sprite_BackGround/E_Login");
     			}
     			return this.m_E_LoginButton;
     		}
     	}

		public UnityEngine.UI.Image E_LoginImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_LoginImage == null )
     			{
		    		this.m_E_LoginImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Sprite_BackGround/E_Login");
     			}
     			return this.m_E_LoginImage;
     		}
     	}

		public UnityEngine.UI.InputField E_AccountInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_AccountInputField == null )
     			{
		    		this.m_E_AccountInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"Sprite_BackGround/E_Account");
     			}
     			return this.m_E_AccountInputField;
     		}
     	}

		public UnityEngine.UI.Image E_AccountImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_AccountImage == null )
     			{
		    		this.m_E_AccountImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Sprite_BackGround/E_Account");
     			}
     			return this.m_E_AccountImage;
     		}
     	}

		public UnityEngine.UI.InputField E_PasswordInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_PasswordInputField == null )
     			{
		    		this.m_E_PasswordInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"Sprite_BackGround/E_Password");
     			}
     			return this.m_E_PasswordInputField;
     		}
     	}

		public UnityEngine.UI.Image E_PasswordImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_PasswordImage == null )
     			{
		    		this.m_E_PasswordImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Sprite_BackGround/E_Password");
     			}
     			return this.m_E_PasswordImage;
     		}
     	}








        public UnityEngine.UI.Button E_ChooseServer1Button
        {
            get
            {
                if (this.uiTransform == null)
                {
                    Log.Error("uiTransform is null.");
                    return null;
                }
                if (this.m_E_ChooseServer1Button == null)
                {
                    this.m_E_ChooseServer1Button = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject, "Sprite_BackGround/E_ChooseServer1");
                }
                return this.m_E_ChooseServer1Button;
            }
        }



        public UnityEngine.UI.Button E_ChooseServer2Button
        {
            get
            {
                if (this.uiTransform == null)
                {
                    Log.Error("uiTransform is null.");
                    return null;
                }
                if (this.m_E_ChooseServer2Button == null)
                {
                    this.m_E_ChooseServer2Button = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject, "Sprite_BackGround/E_ChooseServer2");
                }
                return this.m_E_ChooseServer2Button;
            }
        }


        public UnityEngine.UI.Button E_EnterServerButton
        {
            get
            {
                if (this.uiTransform == null)
                {
                    Log.Error("uiTransform is null.");
                    return null;
                }
                if (this.m_E_EnterServerButton == null)
                {
                    this.m_E_EnterServerButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject, "Sprite_BackGround/E_ChooseServer3");
                }
                return this.m_E_EnterServerButton;
            }
        }


        public UnityEngine.UI.Button E_CreateRoleButton
        {
            get
            {
                if (this.uiTransform == null)
                {
                    Log.Error("uiTransform is null.");
                    return null;
                }
                if (this.m_E_CreateRoleButton == null)
                {
                    this.m_E_CreateRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject, "Sprite_BackGround/E_CreateRole");
                }
                return this.m_E_CreateRoleButton;
            }
        }


        public UnityEngine.UI.Button E_DeleteRoleButton
        {
            get
            {
                if (this.uiTransform == null)
                {
                    Log.Error("uiTransform is null.");
                    return null;
                }
                if (this.m_E_DeleteRoleButton == null)
                {
                    this.m_E_DeleteRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject, "Sprite_BackGround/E_DeleteRole");
                }
                return this.m_E_DeleteRoleButton;
            }
        }

        public UnityEngine.UI.Button E_GetRealmKeyButton
        {
            get
            {
                if (this.uiTransform == null)
                {
                    Log.Error("uiTransform is null.");
                    return null;
                }
                if (this.m_E_GetRealmKeyButton == null)
                {
                    this.m_E_GetRealmKeyButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject, "Sprite_BackGround/E_GetRealmKeyPlay");
                }
                return this.m_E_GetRealmKeyButton;
            }
        }

        public UnityEngine.UI.Button E_EnterPlayButton
        {
            get
            {
                if (this.uiTransform == null)
                {
                    Log.Error("uiTransform is null.");
                    return null;
                }
                if (this.m_E_EnterPlayButton == null)
                {
                    this.m_E_EnterPlayButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject, "Sprite_BackGround/E_EnterPlay");
                }
                return this.m_E_EnterPlayButton;
            }
        }




        public void DestroyWidget()
		{
			this.m_E_LoginButton = null;
			this.m_E_LoginImage = null;
			this.m_E_AccountInputField = null;
			this.m_E_AccountImage = null;
			this.m_E_PasswordInputField = null;
			this.m_E_PasswordImage = null;
			this.uiTransform = null;

            this.m_E_ChooseServer1Button = null;
            this.m_E_ChooseServer2Button = null;
            this.m_E_EnterServerButton = null;

            this.m_E_CreateRoleButton = null;
            this.m_E_DeleteRoleButton = null;
            this.m_E_EnterPlayButton = null;
            this.m_E_GetRealmKeyButton = null;
        }

		private UnityEngine.UI.Button m_E_LoginButton = null;
		private UnityEngine.UI.Image m_E_LoginImage = null;
		private UnityEngine.UI.InputField m_E_AccountInputField = null;
		private UnityEngine.UI.Image m_E_AccountImage = null;
		private UnityEngine.UI.InputField m_E_PasswordInputField = null;
		private UnityEngine.UI.Image m_E_PasswordImage = null;
		public Transform uiTransform = null;

        private UnityEngine.UI.Button m_E_ChooseServer1Button = null;
        private UnityEngine.UI.Button m_E_ChooseServer2Button = null;
        private UnityEngine.UI.Button m_E_EnterServerButton = null;
        private UnityEngine.UI.Button m_E_CreateRoleButton = null;
        private UnityEngine.UI.Button m_E_DeleteRoleButton = null;
        private UnityEngine.UI.Button m_E_EnterPlayButton = null;
        private UnityEngine.UI.Button m_E_GetRealmKeyButton = null;

    }
}
