using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Nexus_Management.Areas.Accountant.ModelsView;
using Nexus_Manegement.Areas.Technical.ModelsView;
using Nexus_Manegement.Models;



namespace Nexus_Management.Areas.Accountant.Dao
{
    public class AccountantDao
    {
        private static AccountantDao instance = null;
        private AccountantDao() { }
        internal static AccountantDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountantDao();
                }
                return instance;
            }
        }
        #region Dashboard

        public ProfileView getEmp(int id)
        {
            var en = new NexusContext();
            var result = en.Employees.Where(x => x.ID == id)
                .Select(x => new ProfileView()
                {
                    Name = x.Name,
                    Phone = x.Phone,
                    Email = x.Email,
                    Username = x.Username,
                    Password = x.Password,
                    PositionName = x.EmployeePosition.Name
                }).FirstOrDefault();
            return result;
        }
        public void changeProfile(int id, string name, string username, string email, string phone)     //changeprofile
        {
            var en = new NexusContext();
            //Employee emp = new Employee();
            Employee emp = en.Employees.Find(id);
            emp.Name = name;
            emp.Username = username;
            emp.Email = email;
            emp.Phone = phone;

            en.SaveChanges();
        }

        public void changePassword(int id, string pass)
        {
            NexusContext en = new NexusContext();
            Employee emp = en.Employees.Find(id);
            emp.Password = pass;
            en.SaveChanges();
        }


        public List<OrderView> GetMonth()
        {
            var en = new NexusContext();
            var result = en.Orders.Where(x => x.OrderDate.Year == DateTime.Now.Year)
                .Select(x => new OrderView() { OrderDate = x.OrderDate.Month.ToString() }).Distinct().ToList();
            return result;
        }
        public List<OrderView> GetYear()
        {
            var en = new NexusContext();
            var result = en.Orders.Where(x => x.OrderDate.Year != DateTime.Now.Year)
                .Select(x => new OrderView() { OrderDate = x.OrderDate.Year.ToString() }).Distinct().ToList();
            return result;
        }
        public decimal GetPrice(string month)
        {
            var en = new NexusContext();
            var result = en.Orders.Where(x => x.OrderDate.Month.ToString() == month && x.OrderDate.Year == DateTime.Now.Year).Sum(x => x.Total);
            return result;
        }
        public List<RevenueChart> GetRevenueChart()
        {
            var month = GetMonth();
            var result = new List<RevenueChart>();

            for (int i = 0; i < month.Count; i++)
            {
                decimal price = GetPrice(month[i].OrderDate);
                result.Add(new RevenueChart()
                {
                    MonthDate = month[i].OrderDate,
                    SumRevenue = price,
                });
            }
            return result;
        }
        public int GetCount(string month)
        {
            var en = new NexusContext();
            var result = en.Orders.Where(x => x.OrderDate.Month.ToString() == month && x.OrderDate.Year == DateTime.Now.Year).Count();
            return result;
        }
        public List<RevenueChart> GetOrderChart()
        {
            var month = GetMonth();
            var result = new List<RevenueChart>();

            for (int i = 0; i < month.Count; i++)
            {
                int count = GetCount(month[i].OrderDate);
                result.Add(new RevenueChart()
                {
                    MonthDate = month[i].OrderDate,
                    CountOrder = count,
                });
            }
            return result;
        }
        public int SumCountOrder()
        {
            var en = new NexusContext();
            var result = en.Orders.Where(x => x.OrderDate.Year == DateTime.Now.Year).Count();
            return result;
        }
        //count bill containt category
        public List<Category> GetCategories()
        {
            var en = new NexusContext();
            var result = en.Categories.ToList();
            return result;
        }
        public int CountOrderPie(int idCate)
        {
            var en = new NexusContext();
            var countOrder = en.Orders.Where(x => x.OrderDate.Year == DateTime.Now.Year && x.OrderServicePlan.CategoryID == idCate).Count();
            return countOrder;
        }
        public List<RevenueChart> PieChart()
        {
            var en = new NexusContext();
            var cate = GetCategories();

            var result = new List<RevenueChart>();

            for (int i = 0; i < cate.Count; i++)
            {
                result.Add(new RevenueChart()
                {
                    NameCategory = cate[i].Name,
                    CountOrder = CountOrderPie(cate[i].ID),
                });
            }
            return result;
        }
        #endregion


        #region Revenue
        public decimal GetPriceMonthOfYear(string month, string year)
        {
            var en = new NexusContext();
            var result = en.Orders.Where(x => x.OrderDate.Month.ToString() == month && x.OrderDate.Year.ToString() == year).Sum(x => x.Total);
            return result;
        }
        public List<RevenueChart> GetRevenueChartOfYear(string year)
        {
            var en = new NexusContext();
            var monthOfYear = en.Orders.Where(x => x.OrderDate.Year.ToString() == year)
                .Select(x => new OrderView() { OrderDate = x.OrderDate.Month.ToString() }).Distinct().ToList();


            var result = new List<RevenueChart>();

            for (int i = 0; i < monthOfYear.Count; i++)
            {
                decimal price = GetPriceMonthOfYear(monthOfYear[i].OrderDate, year);
                result.Add(new RevenueChart()
                {
                    MonthDate = monthOfYear[i].OrderDate,
                    SumRevenue = price,
                });
            }
            return result;
        }
        public List<OrderView> GetRevenue()
        {
            var en = new NexusContext();
            var result = en.Bills.Where(x => x.RegistrationDate.Year == DateTime.Now.Year)
                .Select(x => new OrderView()
                {
                    IDOrder = x.OrderID,
                    IDBill = x.ID,
                    CustomerName = x.BillOrder.OrderCustomer.Name,
                    ServiceName = x.BillOrder.OrderServicePlan.Name,
                    Price = x.BillOrder.OrderServicePlan.Price,
                    Quantity = x.BillOrder.Quantity,
                    UsagePack = x.BillOrder.OrderExpiryDate.Name,
                    Total = x.BillOrder.Total,
                    OrderDate = x.BillOrder.OrderDate.Month.ToString(),
                    OrderYear = x.BillOrder.OrderDate.Year.ToString(),
                    StatusService = x.BillOrder.StatusService,
                    RegistrationDate = x.RegistrationDate.ToString(),
                    ConnectionDate = x.ConnectionDate.ToString(),
                    ExpirationDate = x.ExpirationDate.ToString(),
                }).OrderByDescending(x => x.IDBill).ToList();
            return result;
        }
        public List<OrderView> GetRevenueWithSelect()
        {
            var en = new NexusContext();
            // ID,ServicePlanName,Price/month, price/...months, paid for .. month ,unpaid money
            var result = en.Bills
                .Select(x => new OrderView()
                {
                    IDOrder = x.OrderID,
                    IDBill = x.ID,
                    CustomerName = x.BillOrder.OrderCustomer.Name,
                    ServiceName = x.BillOrder.OrderServicePlan.Name,
                    Price = x.BillOrder.OrderServicePlan.Price,
                    Quantity = x.BillOrder.Quantity,
                    UsagePack = x.BillOrder.OrderExpiryDate.Name,
                    Total = x.BillOrder.Total,
                    OrderDate = x.BillOrder.OrderDate.Month.ToString(),
                    OrderYear = x.BillOrder.OrderDate.Year.ToString(),
                    StatusService = x.BillOrder.StatusService,

                    RegistrationDate = x.RegistrationDate.ToString(),
                    ConnectionDate = x.ConnectionDate.ToString(),
                    ExpirationDate = x.ExpirationDate.ToString(),
                }).OrderByDescending(x => x.IDBill).ToList();
            return result;
        }
        #endregion
    }
}
