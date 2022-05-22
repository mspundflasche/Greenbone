using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using GreenboneApp.Models;
using GreenboneApp.Data;
using System;

namespace GreenboneApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        const string ConnectionString = "server=127.0.0.1;uid=root;pwd=test;database=greenbone";

        // GET: api/<DeviceController>
        [HttpGet]
        public IEnumerable<DeviceModel> Get()
        {
            DBAccess dB = new DBAccess();
            return dB.LoadDataFromDatabase<DeviceModel, dynamic>("SELECT * FROM devicetable;", new { }, ConnectionString);
        }

        // GET api/<DeviceController>/computer/5
        [HttpGet("/computer/{macAddress}")]
        public List<DeviceModel> GetComputer(string macAddress)
        {
            DBAccess dB = new DBAccess();
            return dB.LoadDataFromDatabase<DeviceModel, dynamic>("SELECT * FROM devicetable WHERE MACAddress='" + macAddress + "';", new { }, ConnectionString);
        }

        // GET api/<DeviceController>/user/5
        [HttpGet("/user/{abbrevation}")]
        public List<DeviceModel> GetUser(string abbrevation)
        {
            DBAccess dB = new DBAccess();
            return dB.LoadDataFromDatabase<DeviceModel, dynamic>("SELECT * FROM devicetable WHERE Abbrevation='" + abbrevation + "';", new { }, ConnectionString);
        }

        // POST api/<DeviceController>
        [HttpPost]
        public async void Post([FromBody] DeviceModel device)
        {
            DBAccess dB = new DBAccess();

            List<DeviceModel> deviceModel = dB.LoadDataFromDatabase<DeviceModel, dynamic>
                ("SELECT * FROM devicetable WHERE MACAddress='" + device.MACAddress + "';", new { }, ConnectionString);

            Console.WriteLine(deviceModel.Count);

            if (deviceModel.Count == 0)
            {
                string sql = "INSERT INTO devicetable (MACAddress, ComputerName, IPAddress, Abbrevation, Description) VALUES ('" + device.MACAddress + "', '" + 
                    device.DeviceName + "', '" + device.IPAddress + "', '" + device.Abbrevation + "','" + device.Description + "');";
                int ret = dB.SaveDataInDatabase(sql, new { }, ConnectionString);
            }

            // Notefy the admin if a user has more than 3 computers assigned to him/her
            List<DeviceModel> usersComputers = dB.LoadDataFromDatabase<DeviceModel, dynamic>
                ("SELECT * FROM devicetable WHERE Abbrevation='" + device.Abbrevation + "';", new { }, ConnectionString);

            if (usersComputers is not null && usersComputers.Count > 3)
            {
                Notification notification = new Notification();

                await notification.NotifyAdmin(device.Abbrevation, usersComputers.Count);
            }

        }

        // PUT api/<DeviceController>/5
        [HttpPut("{macAddress}")]
        public void Put(string macAddress, [FromBody] DeviceModel device)
        {
            DBAccess dB = new DBAccess();

            string sql = "INSERT INTO devicetable WHERE MACAddress='" + macAddress + "' VALUES ('" + device.MACAddress + "', '" +
                    device.DeviceName + "', '" + device.IPAddress + "', '" + device.Abbrevation + "', '" + device.Description + "');";

            dB.SaveDataInDatabase(sql, new { }, ConnectionString);
        }

        // DELETE api/<DeviceController>/5
        [HttpDelete("{macAddress}")]
        public void Delete(string macAddress)
        {
            DBAccess dB = new DBAccess();

            string sql = "DELETE * FROM devicetable WHERE MACAddress='" + macAddress + "';";

            dB.SaveDataInDatabase(sql, new { }, ConnectionString);
        }
    }
}
