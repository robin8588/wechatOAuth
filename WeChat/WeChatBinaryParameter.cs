using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat
{
	/// <summary>
	/// RAW数据型API参数
	/// </summary>
	public class WeChatBinaryParameter : WeChatParameter
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public WeChatBinaryParameter()
			: base()
		{

		}
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="name">key</param>
		/// <param name="value">value</param>
		public WeChatBinaryParameter(string name, byte[] value)
			: base(name, value)
		{ 
		
		}
		/// <summary>
		/// 值
		/// </summary>
		public new byte[] Value
		{
			get
			{
				return (byte[])base.Value;
			}
			set
			{
				base.Value = value;
			}
		}
	}
}
