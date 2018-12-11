using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Views;

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
		}

		private class Adapter : RecyclerView.Adapter
		{
			private enum ViewType
			{
				Dummy = 1,
				Input = 2
			}

			private int totalCount = 25;
			private int inputPosition = 8;
			
			public override int ItemCount => totalCount;

			public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
			{
				return;
			}

			public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
			{
				var view = LayoutInflater.From(parent.Context).Inflate(viewType == 1 ? Resource.Layout.item_dummy : Resource.Layout.input_item, parent, false);
				ViewHolder vh = new ViewHolder(view);

				return new ViewHolder(view);
			}

			public override int GetItemViewType(int position)
			{
				if (position == inputPosition) return (int)ViewType.Input;
				else return (int)ViewType.Dummy;
			}
		}

		private class ViewHolder : RecyclerView.ViewHolder
		{
			public ViewHolder(View view) : base(view) { }
		}
	}
}