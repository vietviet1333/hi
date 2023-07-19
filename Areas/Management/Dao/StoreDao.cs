using Nexus_Manegement.Models;

namespace Nexus_Manegement.Areas.Management.Dao
{
    public class StoreDao
    {
        private static StoreDao? instance = null;
        private StoreDao() { }

        internal static StoreDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StoreDao();
                }
                return instance;
            }
        }
        public void Addbranch(string Name, int DistrictID, string Ward, bool status)
        {
            Store st = new Store();
            st.Name = Name;
            st.DistrictID = DistrictID;
            st.Ward = Ward;
            st.status = status;
            NexusContext context = new NexusContext();
            context.Stores.Add(st);
            context.SaveChanges();
        }
        public List<Store> Stores
        {
            get

            {
                NexusContext f = new NexusContext();
                var result = (from s in f.Stores
                              join d in f.Districts on s.DistrictID equals d.ID

                              select new Store()
                              {
                                  ID = s.ID,
                                  Name = s.Name,
                                  Distric = d.Name,
                                  Ward = s.Ward,
                                  status = s.status

                              }).OrderByDescending(x=>x.ID).ToList();
                return result;
            }
        }
        public void Disactive(int Id)
        {NexusContext f= new NexusContext();
            Store st = f.Stores.Find(Id);
            st.status = false;
            f.SaveChanges();

        }
        public void Active(int Id)
        {
            NexusContext f = new NexusContext();
            Store st = f.Stores.Find(Id);
            st.status = true;
            f.SaveChanges();

        }
        public Store Edit(int Id)
        {
            NexusContext f= new NexusContext();
            var result = (from s in f.Stores
                          join d in f.Districts on s.DistrictID equals d.ID

                          select new Store()
                          {
                              ID = s.ID,
                              Name = s.Name,
                              Distric = d.Name,
                              Ward = s.Ward,
                              status = s.status

                          }).ToList();
            var st = result.Find(x => x.ID == Id);
            return st;
        }
        public void Update(int Id, string name, int districtid, string ward)
        {
            NexusContext f=new NexusContext();
            Store st = f.Stores.Find(Id);
            st.Name=name;
            st.DistrictID= districtid;
            st.Ward= ward;
            f.SaveChanges();
        }
        public List<Store> Filter(int disid)
        {
            NexusContext f= new NexusContext();
            var result = (from s in f.Stores
                          join d in f.Districts on s.DistrictID equals d.ID
                          where s.DistrictID == disid
                          select new Store()
                          {
                              ID = s.ID,
                              Name = s.Name,
                              Distric = d.Name,
                              Ward = s.Ward,
                              status = s.status

                          }).ToList();
            return result;
        }
    }
}
