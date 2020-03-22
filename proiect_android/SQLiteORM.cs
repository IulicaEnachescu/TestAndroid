using Android.App;
using SQLite;
using System.Collections.Generic;
using System.IO;

namespace AndroidTest01
{
    public class SQLiteORM
    {
        [Table("Motti")]
        public class Motti
        {
            public string Motto { get; set; }
        }

        public string CaleDb { get; set; }

        public SQLiteORM(string dbName)
        {
            string dbPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName);

            if (!File.Exists(dbPath))
            {
                using (BinaryReader br = new BinaryReader(Application.Context.Assets.Open(dbName)))
                {
                    using (BinaryWriter bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int len = 0;
                        while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, len);
                        }
                    }
                }
            }

            CaleDb = dbPath;
        }

        public List<string> GetMotto()
        {
            List<string> motti = new List<string>();

            using(var db = new SQLiteConnection(this.CaleDb))
            {
                foreach (var s in db.Table<Motti>())
                    motti.Add(s.Motto);
            }

            return motti;
        }

        public void InsertMotto(string motto)
        {
            using (var db = new SQLiteConnection(this.CaleDb))
            {
                db.Insert(new Motti { Motto = motto });
            }
        }

        public void Sterge()
        {
            using (var db = new SQLiteConnection(this.CaleDb))
            {
                db.DeleteAll<Motti>();
            }
        }

    }
}