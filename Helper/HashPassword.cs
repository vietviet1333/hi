namespace Nexus_Manegement.Helper
{
    public class HashPassword
    {
        private static HashPassword _instance = null;
        private HashPassword()
        {

        }
        public static HashPassword GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HashPassword();
                }
                return _instance;
            }
        }
        //Ma hoa password
        public string HashPass(string password)
        {
            //tao muoi ngau nhien
            //int salt = 12;
            //ma hoa
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            //string muoi = BCrypt.Net.BCrypt.GenerateSalt();
            //string newhashedPassword = BCrypt.Net.BCrypt.HashPassword(password, muoi);
            //$2b$10$qsdavf4rvaoMcXfC2efLJO4PtXUAWh5gzYaUy6JiR8ntTd2N.LRW2


            //test
            //string mern = password + muoi;
            //bool checkmern = BCrypt.Net.BCrypt.Verify(mern, newhashedPassword);
            //Console.WriteLine("verify: " + checkmern);

            return hashedPassword;
        }
        //Kiem tra password
        public bool CheckPass(string password, string hashpass)
        {
            //string newPass= password+12;
            return BCrypt.Net.BCrypt.Verify(password, hashpass);
        }





        //------
    }
}
