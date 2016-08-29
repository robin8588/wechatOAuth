using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat
{
	/// <summary>
	/// 值类型API参数
	/// </summary>
	public class WeChatStringParameter : WeChatParameter
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public WeChatStringParameter()
			: base()
		{

		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="name">key</param>
		/// <param name="value">value</param>
		public WeChatStringParameter(string name, string value)
			: base(name, value)
		{ 
		
		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="name">key</param>
		/// <param name="value">value</param>
		public WeChatStringParameter(string name, bool value)
			: base(name, value? "1" : "0")
		{

		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="name">key</param>
		/// <param name="value">value</param>
		public WeChatStringParameter(string name, int value)
			: base(name, string.Format("{0}",value))
		{

		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="name">key</param>
		/// <param name="value">value</param>
		public WeChatStringParameter(string name, long value)
			: base(name, string.Format("{0}", value))
		{

		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="name">key</param>
		/// <param name="value">value</param>
		public WeChatStringParameter(string name, object value)
			: base(name, string.Format("{0}", value))
		{

		}
		/// <summary>
		/// 值
		/// </summary>
		public new string Value
		{
			get
			{
				return (string)base.Value;
			}
			set
			{
				base.Value=value;
			}
		}
	}
	
}
