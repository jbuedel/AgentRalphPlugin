using System;
using System.Diagnostics;

namespace SharpRefactoring
{
	public class Window : IEquatable<Window>
	{
		private readonly int top;
		private readonly int bottom;

        [DebuggerStepThrough]
		public Window(int top, int bottom)
		{
			if(top < 0 || bottom < 0 || top > bottom)
				throw new ArgumentException();

			this.top = top;
			this.bottom = bottom;
		}
        
		public int Top
		{
            [DebuggerStepThrough]
			get { return top; }
		}

		public int Bottom
		{
            [DebuggerStepThrough]
			get { return bottom; }
		}

		public int Size
		{
            [DebuggerStepThrough]
			get { return Bottom - Top + 1; }
		}

	    public static Window Null
	    {
            [DebuggerStepThrough]
	        get { return new Window(0,0); }
	    }

	    public bool Equals(Window obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.Top == Top && obj.Bottom == Bottom;
		}

		public override string ToString()
		{
			return string.Format("Top: {0}, Bottom: {1}", top, bottom);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Window)) return false;
			return Equals((Window)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Top * 397) ^ Bottom;
			}
		}
	}

}