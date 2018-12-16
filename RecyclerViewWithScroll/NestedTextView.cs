using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Support.V4.View;

namespace RecyclerViewWithScroll
{
	[Register("com.techno1d.NestedTextView")]
	public class NestedTextView : TextView, INestedScrollingChild
	{
		private NestedScrollingChildHelper _helper;
		private Context _context;

		#region Ctors
		public NestedTextView(Context context) : base(context)
		{
			_context = context;
			_helper = new NestedScrollingChildHelper(this);
			NestedScrollingEnabled = true;
		}
		public NestedTextView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			_context = context;
			_helper = new NestedScrollingChildHelper(this);
			NestedScrollingEnabled = true;
		}
		public NestedTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			_context = context;
			_helper = new NestedScrollingChildHelper(this);
			NestedScrollingEnabled = true;
		}
		public NestedTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			_context = context;
			_helper = new NestedScrollingChildHelper(this);
			NestedScrollingEnabled = true;
		}
		protected NestedTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
		#endregion
		/*
		public bool DispatchNestedPreScroll(int dx, int dy, int[] consumed, int[] offsetInWindow, int type)
		{
			
			return _helper.DispatchNestedPreScroll(dx, dy, consumed, offsetInWindow, type);
		}

		public bool DispatchNestedScroll(int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int[] offsetInWindow, int type)
		{
			return _helper.DispatchNestedScroll(dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, offsetInWindow, type);
		}

		public bool InvokeHasNestedScrollingParent(int type)
		{
			return _helper.InvokeHasNestedScrollingParent(type);
		}

		public bool StartNestedScroll([GeneratedEnum] ScrollAxis axes, int type)
		{
			return _helper.StartNestedScroll((int)axes, type);
		}

		public void StopNestedScroll(int type)
		{
			_helper.StopNestedScroll(type);
		}
		*/

		public override bool StartNestedScroll([GeneratedEnum] ScrollAxis axes)
		{
			return _helper.StartNestedScroll((int)axes);
		}
		public override void StopNestedScroll()
		{
			var hasParent = HasNestedScrollingParent;
			_helper.StopNestedScroll();
		}
		public override bool DispatchNestedFling(float velocityX, float velocityY, bool consumed)
		{
			return _helper.DispatchNestedFling(velocityX, velocityY, consumed);
		}

		public override bool DispatchNestedPreFling(float velocityX, float velocityY)
		{
			return _helper.DispatchNestedPreFling(velocityX, velocityY);
		}

		public override bool DispatchNestedPreScroll(int dx, int dy, int[] consumed, int[] offsetInWindow)
		{
			return _helper.DispatchNestedPreScroll(dx, dy, consumed, offsetInWindow);
		}

		public override bool DispatchNestedScroll(int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int[] offsetInWindow)
		{
			return _helper.DispatchNestedScroll(dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, offsetInWindow);
		}

		

		
		
	}
}