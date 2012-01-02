// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 1661 $</version>
// </file>

using System;

namespace ICSharpCode.SharpDevelop.Dom
{
	/// <summary>
	/// We don't reference ICSharpCode.Core but still need the logging interface.
	/// </summary>
	internal static class LoggingService
	{
		
		public static void Debug(object message)
		{
		}
		
		public static void Info(object message)
		{
		}
		
		public static void Warn(object message)
		{
		}
		
		public static void Warn(object message, Exception exception)
		{
		}
		
		public static void Error(object message)
		{
		}
		
		public static void Error(object message, Exception exception)
		{
		}
		
		public static bool IsDebugEnabled {
			get { return false; }
		}
	}
}
