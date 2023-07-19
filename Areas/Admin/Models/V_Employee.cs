namespace Nexus_Manegement.Entities.Model
{
    public class V_Employee
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        //public string OTP { get; set; }

        //----
        //fK
        public int Position { get; set; }
    }
}
