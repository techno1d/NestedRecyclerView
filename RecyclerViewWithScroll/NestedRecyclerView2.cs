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
using Android.Support.V7.Widget;
using Android.Support.V4.View;

namespace RecyclerViewWithScroll
{
	[Register("com.techno1d.NewNestedRecyclerView")]
	public class NestedRecyclerView2 : RecyclerView, INestedScrollingParent2
	{
		string TAG = "NewNestedRecyclerView";

		protected Context _context;
		protected NestedScrollingParentHelper _helper;

		public override ScrollAxis NestedScrollAxes => (ScrollAxis)_helper.NestedScrollAxes;

		private View _nestedScrollTarget = null;
		private bool _nestedScrollTargetIsBeingDragged = false;
		private bool _nestedScrollTargetWasUnableToScroll = false;
		private bool _skipsTouchInterception = false;

		#region Ctors
		public NestedRecyclerView2(Context context) : base(context)
		{
			_context = context;
			_helper = new NestedScrollingParentHelper(this);
			NestedScrollingEnabled = true;
		}

		public NestedRecyclerView2(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			_context = context;
			_helper = new NestedScrollingParentHelper(this);
			NestedScrollingEnabled = true;
		}

		public NestedRecyclerView2(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			_context = context;
			_helper = new NestedScrollingParentHelper(this);
			NestedScrollingEnabled = true;
		}

		protected NestedRecyclerView2(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
		#endregion

		public override bool DispatchTouchEvent(MotionEvent e)
		{
			//Log.Debug(TAG, "Dispatch Touch Event");

			bool temporarilySkipsInterception = _nestedScrollTarget != null;

			if (temporarilySkipsInterception)
			{
				//Log.Debug(TAG, "temporarilySkipsInterception is TRUE");
				// If a descendent view is scrolling we set a flag to temporarily skip our onInterceptTouchEvent implementation
				_skipsTouchInterception = true;
				//Log.Debug(TAG, "_skipsTouchInterseption is TRUE");
			}

			// First dispatch, potentially skipping our onInterceptTouchEvent
			bool handled = base.DispatchTouchEvent(e);

			if (temporarilySkipsInterception)
			{
				//Log.Debug(TAG, "temporarilySkipsInterception is TRUE");

				_skipsTouchInterception = false;

				//Log.Debug(TAG, "_skipsTouchInterseption is FALSE");
				// If the first dispatch yielded no result or we noticed that the descendent view is unable to scroll in the
				// direction the user is scrolling, we dispatch once more but without skipping our onInterceptTouchEvent.
				// Note that RecyclerView automatically cancels active touches of all its descendents once it starts scrolling
				// so we don't have to do that.
				if (!handled)// || _nestedScrollTargetWasUnableToScroll)
				{
					handled = base.DispatchTouchEvent(e);
				}
			}

			return handled;

		}

		public override bool OnInterceptTouchEvent(MotionEvent e)		{
			
			return base.OnInterceptTouchEvent(e) && !_skipsTouchInterception;
		}

		public bool OnStartNestedScroll(View child, View target, int axes, int type)
		{
			ScrollAxis axis = (ScrollAxis)axes;

			return (axis == ScrollAxis.Vertical);
		}

		public void OnNestedScrollAccepted(View child, View target, int axes, int type)
		{
			_helper.OnNestedScrollAccepted(child, target, axes, type);

			//base.OnNestedScrollAccepted(child, target, (ScrollAxis)axes);
			_nestedScrollTarget = target;
			StartNestedScroll((ScrollAxis)axes, type);
		}
		
		public void OnNestedPreScroll(View target, int dx, int dy, int[] consumed, int type)
		{
			
			//DispatchNestedPreScroll(dx, dy, consumed, null, type);
		}

		public void OnNestedScroll(View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed, int type)
		{
			base.OnNestedScroll(target, 0, dyConsumed, 0, dyUnconsumed);
			/*
			int oldScrollY = ScrollY;
			ScrollBy(0, dyUnconsumed);
			int myConsumed = ScrollY - oldScrollY;
			int myUnconsumed = dyUnconsumed - myConsumed;
			DispatchNestedScroll(0, myConsumed, 0, myUnconsumed, null, type);
			*/
		}	
		

		public void OnStopNestedScroll(View target, int type)
		{
			//_helper.OnStopNestedScroll(target, type);
			StopNestedScroll(type);
			//_nestedScrollTarget = null;
		}



		public override void OnNestedPreScroll(View target, int dx, int dy, int[] consumed)
		{
			OnNestedPreScroll(target, dx, dy, consumed, ViewCompat.TypeTouch);
		}

		public override void OnNestedScroll(View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
		{
			OnNestedScroll(target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed, ViewCompat.TypeTouch);
		}

		public override void OnNestedScrollAccepted(View child, View target, [GeneratedEnum] ScrollAxis axes)
		{
			OnNestedScrollAccepted(child, target, (int)axes, ViewCompat.TypeTouch);
		}

		public override bool OnStartNestedScroll(View child, View target, [GeneratedEnum] ScrollAxis axes)
		{
			return OnStartNestedScroll(child, target, (int)axes, ViewCompat.TypeTouch);
		}

		public override void OnStopNestedScroll(View target)
		{
			OnStopNestedScroll(target, ViewCompat.TypeTouch);
			//_nestedScrollTarget = null;
		}

	}
}