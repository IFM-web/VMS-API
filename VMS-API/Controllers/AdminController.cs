using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VMS_API;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace VMS_API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        db_Utility util = new db_Utility();


        [Route("Login")]
        [HttpPost]
        public IActionResult Login()
        {
            string Email = Request.Form["Email"].ToString();
            string Password = Request.Form["Password"].ToString();
            string encptpass = CreateMD5(Password);
            var ds = util.Fill("exec udp_BranchLogin @BranchEmail='"+Email+"',@BranchPassword='" + encptpass + "'");

            if (ds.Tables[0].Rows[0]["status"].ToString() != "fail")
            {
                HttpContext.Session.SetString("CompanyId", ds.Tables[0].Rows[0]["company_id"].ToString());
                HttpContext.Session.SetString("BranchId", ds.Tables[0].Rows[0]["branch_id"].ToString());
                HttpContext.Session.SetString("UserType", ds.Tables[0].Rows[0]["Usertype"].ToString());

            }
            else
            {
                HttpContext.Session.Clear();
            }
            
         
            return Content(JsonConvert.SerializeObject(ds.Tables[0]),"application/json");
           
        }

        [Route("DDList")]
        public IActionResult DDList(string Name)
        {
            var ds = util.Fill("exec DDList @Id1='"+Name+"'");
            return Content(JsonConvert.SerializeObject(ds.Tables[0]),"application/json");
        }
        public string CreateMD5(string password)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
