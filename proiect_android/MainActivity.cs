using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidTest01
{
    [Activity(Label = "AndroidTest02", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        SQLiteORM o = null;
        EditText  t = null;
        ListView lv = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Button b1 = FindViewById<Button>(Resource.Id.button1);
            Button b2 = FindViewById<Button>(Resource.Id.button2);

            lv = FindViewById<ListView>(Resource.Id.listView1);
            t  = FindViewById<EditText>(Resource.Id.editText1);

            o = new SQLiteORM("test.db");
            RefreshAdapter();

            b1.Click += B1_Click;
            b2.Click += B2_Click;

            t.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    B1_Click(sender, e);
                    e.Handled = true;
                }
            };
        }

        private void RefreshAdapter()
        {
            lv.Adapter = new ArrayAdapter<string>(lv.Context, Resource.Layout.lvItem, o.GetMotto().ToArray());
        }

        private void B2_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Confirmati stergerea?");
            alert.SetMessage("Intregul continut al tabelului va fi sters");
            alert.SetPositiveButton("Continua", (senderAlert, args) => {
                o.Sterge();
                RefreshAdapter();
            });

            alert.SetNegativeButton("Anulare", (senderAlert, args) => {  });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void B1_Click(object sender, EventArgs e)
        {
            if (o != null) 
            {
                if (t.Text != "")
                {
                    o.InsertMotto(t.Text);
                    RefreshAdapter();
                    t.Text = "";
                } else
                {
                    Toast.MakeText(this, "Trebuie introdus un sir", ToastLength.Short).Show();
                }
                t.RequestFocus();                
            }
        }


    }
}

