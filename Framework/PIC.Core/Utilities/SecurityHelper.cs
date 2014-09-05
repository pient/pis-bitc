using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

namespace PIC.Common
{
	/// <summary>
	/// SecurityUtility提供一些安全方面的功能，如：检查某个用户登陆是否有效，转换当前线程的安全上下文到不同的用户帐户
	/// </summary>
	public class SecurityHelper
	{
		// 定义 p/invoke
		[DllImport(@"advapi32.dll")]
		public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, 
			int dwLogonType, int dwLogonProvider, out System.IntPtr phToken);

		[DllImport(@"Kernel32.dll")]
		public static extern int GetLastError();

		[DllImport(@"advapi32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto, SetLastError=true)]
		public extern static bool DuplicateToken(IntPtr hToken, 
			int impersonationLevel,  
			ref IntPtr hNewToken);

		private const int LOGON32_LOGON_INTERACTIVE = 2;
		private const int LOGON32_PROVIDER_DEFAULT = 0;
		private const int SecurityImpersonation = 2;
		private WindowsImpersonationContext impersonationContext = null;

		/// <summary>
		/// 用新的帐户运行当前线程
		/// </summary>
		/// <param name="userName">要切换的用户</param>
		public void Switch (string userName, string password, string domain)
		{
			IntPtr token = IntPtr.Zero;
			WindowsImpersonationContext impersonationContext = null;

			// 以给定的用户帐户登陆log on as the give user account
			bool loggedOn = LogonUser(
				// 用户名
				userName,
				// 计算机名或域名
				domain,
				password,
				LOGON32_LOGON_INTERACTIVE,   
				LOGON32_PROVIDER_DEFAULT,    
				// 获取指定用户的用户令牌
				out token); 

			if (loggedOn == false)
			{
				throw new System.Security.SecurityException(userName + " logon failed" );
			}

			IntPtr tokenDuplicate = IntPtr.Zero;
			WindowsIdentity tempWindowsIdentity =null;

			// 复制安全令牌
			if(DuplicateToken(token, SecurityImpersonation, ref tokenDuplicate) != false) 
			{
				tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);

				// 用新的window标识运行当前线程
				impersonationContext = tempWindowsIdentity.Impersonate();
			}
			else
			{
				throw new System.Security.SecurityException("Logon use failed");
			}
		}

		/// <summary>
		/// 返回到原来的用户帐户
		/// </summary>
		public void UndoSwitch()
		{
			impersonationContext.Undo();
		}

		/// <summary>
        /// // 根据用户提供的username/password/domain组合创建WindowIdentity对象
		/// </summary>
		/// <param name="userName">用户名</param>
		/// <param name="password">密码</param>
		/// <param name="domain">如果使用本地安全，则为域名或计算机名</param>
		/// <returns></returns>
		public WindowsIdentity LogonUser(string userName, string password, string domain)
		{
			IntPtr token = IntPtr.Zero;
			bool loggedOn = LogonUser(
                // 用户名
				userName,
                // 计算机名或域名
				domain,
				password,
				LOGON32_LOGON_INTERACTIVE,   
				LOGON32_PROVIDER_DEFAULT,
                // 获取指定用户的用户令牌
				out token); 

			if (loggedOn == false)
			{
				throw new System.Security.SecurityException(userName + " logon failed" );
			}

			// 根据新创建的令牌创建WindowIdentity对象
			WindowsIdentity newID = new WindowsIdentity(token);
			return newID;
		}

		/// <summary>
		/// 检查是否username/password/domain组合是一个有效的用户登陆
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="domain"></param>
		public void ValidateUser(string userName, string password, string domain)
		{
			IntPtr token = IntPtr.Zero;
			bool loggedOn = LogonUser(
				// 用户名
				userName,
                // 计算机名或域名
				domain,
				// 密码
				password,
				// LOGON32_LOGON_INTERACTIVE .
				LOGON32_LOGON_INTERACTIVE,   
				// Logon provider = LOGON32_PROVIDER_DEFAULT.
				LOGON32_PROVIDER_DEFAULT,
                // 获取指定用户的用户令牌
				out token); 

			if (loggedOn  == false)
			{
				throw new System.Security.SecurityException(userName + " logon failed" );
			}
		}



	}
}

