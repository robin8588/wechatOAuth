using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat
{
	/// <summary>
	/// 微博API参数
	/// </summary>
	public abstract class WeChatParameter
	{
		/// <summary>
		/// 参数名称
		/// </summary>
		public string Name
		{
			get;
			set;
		}
		/// <summary>
		/// 参数值
		/// </summary>
		public object Value
		{
			get;
			set;
		}
		/// <summary>
		/// 构造函数
		/// </summary>
		public WeChatParameter()
		{ 
		
		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="name">key</param>
		/// <param name="value">value</param>
		public WeChatParameter(string name, object value)
		{
			this.Name = name;
			this.Value = value;
		}
	}
}
