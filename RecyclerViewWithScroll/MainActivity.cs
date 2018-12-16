using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Text.Method;
using Android.Animation;
using System;

namespace RecyclerViewWithScroll
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
					   
			var recyclerView = FindViewById<RecyclerView>(Resource.Id.list);
			recyclerView.SetAdapter(new Adapter());
			recyclerView.SetLayoutManager(new LinearLayoutManager(this));

			var btn = FindViewById<Button>(Resource.Id.button);
			btn.Click += Btn_Click;
		}

		private void Btn_Click(object sender, System.EventArgs e)
		{
			View tv = FindViewById(Resource.Id.text_resize);
						
			ValueAnimator anim = ValueAnimator.OfInt(tv.MeasuredHeight, 1000);
			anim.AddUpdateListener(new AnimatorUpdateListener(tv));
			anim.SetDuration(1000);
			anim.Start();
		}

		private class AnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
		{
			public View AnimatedView { get; set; }
			public AnimatorUpdateListener(View v)
			{
				AnimatedView = v;
			}
			public void OnAnimationUpdate(ValueAnimator animation)
			{
				int val = (int)animation.AnimatedValue;
				ViewGroup.LayoutParams layoutParams = AnimatedView.LayoutParameters;
				layoutParams.Height = val;
				AnimatedView.LayoutParameters = layoutParams;
			}
		}

		private class Adapter : RecyclerView.Adapter
		{
			private enum ViewType
			{
				Dummy = 1,
				Input = 2,
				Resizable = 3
			}

			private int totalCount = 25;
			private int inputPosition = 8;
			private int resizablePos = 11;
			
			public override int ItemCount => totalCount;

			public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
			{
				if (holder is ResizableViewHolder)
				{
					var h = holder as ResizableViewHolder;
					h.Frame.Click += h.Frame_Click;
				}
				return;
			}

			public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
			{
				int layout_id = -1;
				switch (viewType)
				{
					case 1:
						layout_id = Resource.Layout.item_dummy;
						break;
					case 2:
						layout_id = Resource.Layout.long_text_item;
						break;
					case 3:
						layout_id = Resource.Layout.resizable_item;
						break;
					default:
						throw new System.Exception();
				}
				var view = LayoutInflater.From(parent.Context).Inflate(layout_id, parent, false);

				RecyclerView.ViewHolder vh = new ViewHolder(view);
				
				switch (viewType)
				{
					case 1:
					case 2:
						vh = new ViewHolder(view);
						break;
					case 3:
						vh = new ResizableViewHolder(view);
						break;
					default:
						throw new System.Exception("Wrong viewType!");						
				}				

				//Android.Support.V4.Widget.NestedScrollView t;
				return new ViewHolder(view);
			}

			public override int GetItemViewType(int position)
			{
				if (position == inputPosition || position == inputPosition + 1) return (int)ViewType.Input;
				else if (position == resizablePos) return (int)ViewType.Resizable;
				else return (int)ViewType.Dummy;
			}
		}

		private class ViewHolder : RecyclerView.ViewHolder
		{
			public ViewHolder(View view) : base(view) { }
		}

		private class ResizableViewHolder : RecyclerView.ViewHolder
		{
			public FrameLayout Frame { get; private set; }

			public ResizableViewHolder(View v) : base(v)
			{
				Frame = v.FindViewById<FrameLayout>(Resource.Id.resizable);
				//Frame.Click += Frame_Click;
			}

			public void Frame_Click(object sender, System.EventArgs e)
			{
				Frame.Click -= Frame_Click;
				FrameLayout frame = sender as FrameLayout;
				LinearLayout.LayoutParams par = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 400);
				frame.LayoutParameters = par;
				frame.RequestLayout();
			}
		}

		private class LongTextViewHolder : RecyclerView.ViewHolder
		{
			public TextView LongTextView { get; private set; }

			public LongTextViewHolder(View view) : base(view)
			{
				LongTextView = view.FindViewById<TextView>(Resource.Id.long_text);
				if (LongTextView != null)
				{
					LongTextView.MovementMethod = new ScrollingMovementMethod();
				}
			}
		}

	}
}