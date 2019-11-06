namespace gamelogic
{
    public class TestLogic
    {
        private static Entities DB = null;
        public static void ConnectToDB() {
            DB = new Entities();
        }
        public static Entities GetContext()
        {
            if (DB == null) ConnectToDB();
            return DB;
        }
        public static bool UpgradeBase(int baseid) {
            DB = GetContext();
            bool result = false;
            try
            {
                var bas = DB.Bases.Find(baseid);
                bas.Level++;
                DB.SaveChanges();
                result = true;
            }
            catch
            {
            }
            return result;
        }
        public static string debug() {
            string output = "ACCOUNTS TABLE:\n";
            using (Entities db = new Entities())
                foreach (Account u in db.Accounts) output += u.Username + "\n";
            return output;
        }
        public static Account FindUser(string username) {
            DB = GetContext();
            return DB.Accounts.Find(username);
        }
        public static bool CreateUser(string username, string password)
        {
            DB = GetContext();
            bool result = false;
            try {
                Account user = new Account { Username = username, Password = password, Role = 0 };
                DB.Accounts.Add(user);
                DB.SaveChanges();
                int newIdentityValue = user.UserID;
                DB.Players.Add(new Player { UserID = newIdentityValue, Username = "Admin" });
                DB.Bases.Add(new Base { Basename = username+"Base", OwnerID = newIdentityValue, CoordX = 1, CoordY = 1, Level = 0 });
                DB.SaveChanges();
                result = true;
            }
            catch {
            }
            return result;
        }
        public string Print(string str) {
            System.Diagnostics.Debug.WriteLine("debug functions");
            return str;
        }
    }
}
