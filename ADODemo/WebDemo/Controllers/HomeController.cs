using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _configuration;

        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlStudent");
            
        }

        public IActionResult Index()
        {
            //using SqlConnection conn = new SqlConnection(_connectionString);
            //string sql = "Select * from StudentInfo";
            //SqlDataAdapter adapter = new(sql,conn);
            //DataTable table = new DataTable();
            //adapter.Fill(table);

            string sql = "Select * from StudentInfo";
            DataTable? table = DbHelper.GetDataTable(sql);
            return View(table);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}