using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VMS_API;
using System.Text;
using Microsoft.AspNetCore.Http;
using VMS_API.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace VMS_API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        db_Utility util = new db_Utility();



        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = 400,
                    Message = "Invalid login data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }



            string encptpass = util.CreateMD5(obj.Password);
            var ds = util.Fill("exec udp_BranchLogin @BranchEmail='" + obj.UserName + "',@BranchPassword='" + encptpass + "'");

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
            return Content(JsonConvert.SerializeObject(ds.Tables[0]), "application/json");
        }


    


    [HttpGet("GetEmployeeName")]
    public IActionResult DDList(string Name)
    {
        var ds = util.Fill("exec DDList @Id1='" + Name + "'");
        if (ds.Tables[0].Rows.Count != 0)
        {
            return Content(JsonConvert.SerializeObject(ds.Tables[0]), "application/json");
        }
        else
        {
            return NotFound(new { Massage = "Record Not Found !!", Status = "404" });
        }
    }


}
}
