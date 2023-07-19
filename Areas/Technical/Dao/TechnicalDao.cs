using DocumentFormat.OpenXml.Office2010.Excel;
using Nexus_Manegement.Areas.Technical.Dao;

using Nexus_Manegement.Models;
using Nexus_Manegement.Areas.Technical.ModelsView;
using System.Linq;
using Nexus_Management.Areas.Technical.ModelsView;

namespace Nexus_Manegement.Areas.Technical.Dao
{
    public class TechnicalDao
    {
        private static TechnicalDao instance = null;
        private TechnicalDao() { }
        internal static TechnicalDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TechnicalDao();
                }
                return instance;
            }
        }

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

        //
        public List<OrderView> GetYear()
        {
            var en = new NexusContext();
            var result = en.Orders.Where(x => x.OrderDate.Year != DateTime.Now.Year)
                .Select(x => new OrderView() { OrderDate = x.OrderDate.Year.ToString() }).Distinct().ToList();
            return result;
        }


        public List<OrderView> getOrder()
        {
            var en = new NexusContext();
            var result = en.Bills.Where(x => x.RegistrationDate.Year == DateTime.Now.Year)
                .Select(x => new OrderView()
                {
                    IDBill = x.ID,
                    IDOrder = x.OrderID,
                    CustomerName = x.BillOrder.OrderCustomer.Name,
                    ServiceName = x.BillOrder.OrderServicePlan.Name,
                    Price = x.BillOrder.OrderServicePlan.Price,
                    Quantity = x.BillOrder.Quantity,
                    UsagePackID = x.BillOrder.ExpiryDateID,

                    UsagePack = x.BillOrder.OrderExpiryDate.Name,
                    OrderDate = x.BillOrder.OrderDate.Month.ToString(),
                    OrderYear = x.BillOrder.OrderDate.Year.ToString(),

                    RegistrationDate = x.RegistrationDate.ToString(),
                    StatusService = x.BillOrder.StatusService,

                    ConnectionDate = x.ConnectionDate.ToString(),
                    ExpirationDate = x.ExpirationDate.ToString(),
                }).OrderByDescending(x => x.IDOrder).ToList();
            return result;
        }


        public List<OrderView> getOrderWithSelect()
        {
            var en = new NexusContext();
            var result = en.Bills
                .Select(x => new OrderView()
                {
                    IDBill = x.ID,

                    IDOrder = x.OrderID,
                    CustomerName = x.BillOrder.OrderCustomer.Name,
                    ServiceName = x.BillOrder.OrderServicePlan.Name,
                    Price = x.BillOrder.OrderServicePlan.Price,
                    Quantity = x.BillOrder.Quantity,
                    UsagePackID = x.BillOrder.ExpiryDateID,
                    UsagePack = x.BillOrder.OrderExpiryDate.Name,
                    OrderDate = x.BillOrder.OrderDate.Month.ToString(),
                    OrderYear = x.BillOrder.OrderDate.Year.ToString(),

                    RegistrationDate = x.RegistrationDate.ToString(),
                    StatusService = x.BillOrder.StatusService,

                    ConnectionDate = x.ConnectionDate.ToString(),
                    ExpirationDate = x.ExpirationDate.ToString(),
                }).OrderByDescending(x => x.IDOrder).ToList();
            return result;
        }

        public void NewConnect(int id, int idOrder, int usagepack)
        {
            DateTime dateConnect = DateTime.Now;
            var en = new NexusContext();
            Bill? bill = en.Bills.Find(id);
            bill.ConnectionDate = dateConnect;

            switch (usagepack)
            {
                case 1:
                    DateTime dateExpiry1 = dateConnect.AddMonths(1);
                    bill.ExpirationDate = dateExpiry1;
                    en.SaveChanges();
                    break;
                case 2:
                    DateTime dateExpiry2 = dateConnect.AddMonths(3);
                    bill.ExpirationDate = dateExpiry2;
                    en.SaveChanges();
                    break;
                case 3:
                    DateTime dateExpiry3 = dateConnect.AddMonths(6);
                    bill.ExpirationDate = dateExpiry3;
                    en.SaveChanges();
                    break;
                case 4:
                    DateTime dateExpiry4 = dateConnect.AddMonths(12);
                    bill.ExpirationDate = dateExpiry4;
                    en.SaveChanges();
                    break;
            }
            Order? order = en.Orders.Find(idOrder);
            order.StatusService = 1;
            en.SaveChanges();
        }

        public void CheckConnect()
        {
            var orderList = getOrder().Where(x => x.StatusService == 1).ToList();
            for (int i = 0; i < orderList.Count; i++)
            {
                if (DateTime.Now > Convert.ToDateTime(orderList[i].ExpirationDate))
                {
                    var en = new NexusContext();
                    Order? order = en.Orders.Find(orderList[i].IDOrder);
                    order.StatusService = 2;
                    en.SaveChanges();
                }
            }
        }

        public List<ExchangeView> Exchange()
        {
            var en = new NexusContext();
            var result = en.ProductExchanges.Select(
                x => new ExchangeView()
                {
                    ID = x.ID,
                    BillID = x.BillID,
                    CustomerName = x.ProductExchangeBill.BillOrder.OrderCustomer.Name,
                    Quantity = x.ProductExchangeBill.BillOrder.Quantity,
                    QuantityChange = x.QuantityChange,
                    UsagePack = x.ProductExchangeBill.BillOrder.OrderExpiryDate.Name,
                    ServicePlanName = x.ProductExchangeBill.BillOrder.OrderServicePlan.Name,
                    Status = x.Status,
                    ExchangeDate = x.ExchangeDate.ToString()
                }).ToList();
            return result;
        }
        public void SaveExchange(int id)
        {
            var en = new NexusContext();
            ProductExchange exchange = en.ProductExchanges.Find(id);
            exchange.Status = true;
            exchange.ExchangeDate = DateTime.Now;
            en.SaveChanges();
        }
        public int CountOrderExpiry()
        {
            var en = new NexusContext();
            var chk = en.Bills.Where(x => x.RegistrationDate.Year == DateTime.Now.Year)
                .Select(x => new OrderView()
                {
                    IDBill = x.ID,
                    IDOrder = x.OrderID,
                    CustomerName = x.BillOrder.OrderCustomer.Name,
                    ServiceName = x.BillOrder.OrderServicePlan.Name,
                    Price = x.BillOrder.OrderServicePlan.Price,
                    Quantity = x.BillOrder.Quantity,
                    UsagePackID = x.BillOrder.ExpiryDateID,
                    UsagePack = x.BillOrder.OrderExpiryDate.Name,
                    OrderDate = x.BillOrder.OrderDate.Month.ToString(),
                    OrderYear = x.BillOrder.OrderDate.Year.ToString(),

                    RegistrationDate = x.RegistrationDate.ToString(),
                    StatusService = x.BillOrder.StatusService,

                    ConnectionDate = x.ConnectionDate.ToString(),
                    ExpirationDate = x.ExpirationDate.ToString(),
                }).ToList();
            //var timeExpiry;
            List<TimeExpiry> list = new List<TimeExpiry>();

            for (int i = 0; i < chk.Count; i++)
            {
                TimeSpan time = Convert.ToDateTime(chk[i].ExpirationDate) - DateTime.Now.Date;
                int timeExpiry = time.Days;
                if (timeExpiry <= 7 && Convert.ToDateTime(chk[i].ExpirationDate) >= DateTime.Now)
                {
                    TimeExpiry model = new TimeExpiry();
                    model.IDOrder = chk[i].IDOrder;
                    model.IDBill = chk[i].IDBill;
                    //model.CustomerID = chk[i].CustomerID;
                    model.CustomerName = chk[i].CustomerName;
                    model.ServicePlanID = chk[i].ServicePlanID;
                    model.ServiceName = chk[i].ServiceName;
                    model.ConnectionDate = chk[i].ConnectionDate;
                    model.ExpirationDate = chk[i].ExpirationDate;
                    list.Add(model);
                }
            }
            return list.Count();
        }

        public List<TimeExpiry> MessOrderExpiry()
        {
            var en = new NexusContext();
            var chk = en.Bills.Where(x => x.RegistrationDate.Year == DateTime.Now.Year)
                .Select(x => new OrderView()
                {
                    IDBill = x.ID,
                    IDOrder = x.OrderID,
                    CustomerName = x.BillOrder.OrderCustomer.Name,
                    ServiceName = x.BillOrder.OrderServicePlan.Name,
                    MailCustomer = x.BillOrder.OrderCustomer.Email,
                    Price = x.BillOrder.OrderServicePlan.Price,
                    Quantity = x.BillOrder.Quantity,
                    UsagePackID = x.BillOrder.ExpiryDateID,
                    UsagePack = x.BillOrder.OrderExpiryDate.Name,
                    OrderDate = x.BillOrder.OrderDate.Month.ToString(),
                    OrderYear = x.BillOrder.OrderDate.Year.ToString(),

                    RegistrationDate = x.RegistrationDate.ToString(),
                    StatusService = x.BillOrder.StatusService,

                    ConnectionDate = x.ConnectionDate.ToString(),
                    ExpirationDate = x.ExpirationDate.ToString(),
                }).ToList();
            //var timeExpiry;
            List<TimeExpiry> list = new List<TimeExpiry>();

            for (int i = 0; i < chk.Count; i++)
            {
                TimeSpan time = Convert.ToDateTime(chk[i].ExpirationDate) - DateTime.Now.Date;
                int timeExpiry = time.Days;
                if (timeExpiry <= 7)
                {
                    TimeExpiry model = new TimeExpiry();
                    model.IDOrder = chk[i].IDOrder;
                    model.IDBill = chk[i].IDBill;
                    //model.CustomerID = chk[i].CustomerID;
                    model.CustomerName = chk[i].CustomerName;
                    model.MailCustomer = chk[i].MailCustomer;
                    model.ServicePlanID = chk[i].ServicePlanID;
                    model.ServiceName = chk[i].ServiceName;
                    model.ConnectionDate = chk[i].ConnectionDate;
                    model.ExpirationDate = chk[i].ExpirationDate;
                    model.TimeSpan = timeExpiry;
                    list.Add(model);
                }
            }
            return list.ToList();
        }

       
    }

   
}
