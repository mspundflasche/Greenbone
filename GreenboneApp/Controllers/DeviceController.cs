using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using GreenboneApp.Models;
using GreenboneApp.Data;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenboneApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        const string ConnectionString = "server=127.0.0.1;uid=root;pwd=12345;database=test";

        // GET: api/<DeviceController>
        [HttpGet]
        public IEnumerable<DeviceModel> Get()
        {
            DBAccess db = new DBAccess();
            return db.LoadDataFromDatabase<DeviceModel, dynamic>("SELECT * FROM DeviceTable", new { }, ConnectionString);
        }

        // GET api/<DeviceController>/5
        [HttpGet("{id}")]
        public List<DeviceModel> Get(string macAddress)
        {
            DBAccess db = new DBAccess();
            return db.LoadDataFromDatabase<DeviceModel, dynamic>("SELECT * WHERE MACAddress=" + macAddress + " FROM DeviceTable", new { }, ConnectionString);
        }

        // POST api/<DeviceController>
        [HttpPost]
        public void Post([FromBody] DeviceModel device)
        {
            DBAccess dB = new DBAccess();

            List<DeviceModel> deviceModel = dB.LoadDataFromDatabase<DeviceModel, dynamic>
                ("SELECT * WHERE MACAddress=" + device.MACAddress + " FROM DeviceTable", new { }, ConnectionString);

            if (deviceModel is null)
            {
                string sql = "INSERT INTO DeviceTable VALUES (" + device.MACAddress + ", " + 
                    device.DeviceName + ", " + device.IPAddress + ", " + device.Abbrevation + "," + device.Description + ")";
                int ret = dB.SaveDataInDatabase(sql, new { }, ConnectionString);
            }
        }

        // PUT api/<DeviceController>/5
        [HttpPut("{id}")]
        public void Put(string macAddress, [FromBody] DeviceModel device)
        {
            DBAccess dB = new DBAccess();


        }

        // DELETE api/<DeviceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
