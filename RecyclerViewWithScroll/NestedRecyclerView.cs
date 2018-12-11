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
	[Register("com.techno1d.NestedRecyclerView")]
	public class NestedRecyclerView : RecyclerView, INestedScrollingParent
	{
		protected Context _context;

		private View _nestedScrollTarget = null;
		private bool _nestedScrollTargetIsBeingDragged = false;
		private bool _nestedScrollTargetWasUnableToScroll = false;
		private bool _skipsTouchInterception = false;
		
		#region Ctors
		public NestedRecyclerView(Context context) : base(context)
		{
			_context = context;
		}

		public NestedRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs) { _context = context; }

		public NestedRecyclerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			_context = context;
		}

		protected NestedRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
		#endregion


		public override bool DispatchTouchEvent(MotionEvent e)
		{
			bool temporarilySkipsInterception = _nestedScrollTarget != null;

			if (temporarilySkipsInterception)
			{
				// If a descendent view is scrolling we set a flag to temporarily skip our onInterceptTouchEvent implementation
				_skipsTouchInterception = true;
			}

			// First dispatch, potentially skipping our onInterceptTouchEvent
			bool handled = base.DispatchTouchEvent(e);

			if (temporarilySkipsInterception)
			{
				_skipsTouchInterception = false;

				// If the first dispatch yielded no result or we noticed that the descendent view is unable to scroll in the
				// direction the user is scrolling, we dispatch once more but without skipping our onInterceptTouchEvent.
				// Note that RecyclerView automatically cancels active touches of all its descendents once it starts scrolling
				// so we don't have to do that.
				if (!handled || _nestedScrollTargetWasUnableToScroll)
				{
					handled = base.DispatchTouchEvent(e);
				}
			}

			return handled;			
		}

		// Skips RecyclerView's onInterceptTouchEvent if requested
		
		public override bool OnInterceptTouchEvent(MotionEvent e)
		{
			return base.OnInterceptTouchEvent(e) && !_skipsTouchInterception;
		}

		public override void OnNestedScroll(View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
		{
			base.OnNestedScroll(target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);

			if (target == _nestedScrollTarget && !_nestedScrollTargetIsBeingDragged)
			{
				if (dyConsumed != 0)
				{
					// The descendent was actually scrolled, so we won't bother it any longer.
					// It will receive all future events until it finished scrolling.
					_nestedScrollTargetIsBeingDragged = true;
					_nestedScrollTargetWasUnableToScroll = false;
				}
				else if (dyConsumed == 0 && dyUnconsumed != 0)
				{
					// The descendent tried scrolling in response to touch movements but was not able to do so.
					// We remember that in order to allow RecyclerView to take over scrolling.
					_nestedScrollTargetWasUnableToScroll = true;
					if (target.Parent != null)
						target.Parent.RequestDisallowInterceptTouchEvent(false);
				}
			}
			
		}

		public override void OnNestedScrollAccepted(View child, View target, [GeneratedEnum] ScrollAxis axes)
		{
			if (axes != 0 && Android.Views.ScrollAxis.Vertical != 0)
			{
				// A descendent started scrolling, so we'll observe it.
				_nestedScrollTarget = target;
				_nestedScrollTargetIsBeingDragged = false;
				_nestedScrollTargetWasUnableToScroll = false;
			}
						
			base.OnNestedScrollAccepted(child, target, axes);
		}

		public override bool OnStartNestedScroll(View child, View target, [GeneratedEnum] ScrollAxis nestedScrollAxes)
		{
			bool secondPart = (int)Build.VERSION.SdkInt < 21 || ScrollAxis.Vertical != 0;

			return (nestedScrollAxes != 0 && secondPart);
		}

		public override void OnStopNestedScroll(View child)
		{
			_nestedScrollTarget = null;
			_nestedScrollTargetIsBeingDragged = false;
			_nestedScrollTargetWasUnableToScroll = false;
		}		
	}
}