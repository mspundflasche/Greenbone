using GreenboneApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace GreenboneApp.Data
{
    public class Notification
    {
        public async Task NotifyAdmin(string _user, int _numberOfDevices)
        {
            using( var client = new HttpClient())
            {
                // Set new URI to the REST API
                // this port set when the notification docker image is startet 12345:8080
                client.BaseAddress = new System.Uri("http://localhost:12345/");
                // Tell the Server to accept json data format
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string message = "User " + _user + " has " + _numberOfDevices + " assigned";

                NotificationModel model = new NotificationModel() { Level = "warning", EmployeeAbbreviation = _user, Message = message };

                HttpResponseMessage response = await client.PostAsJsonAsync("api/notify", model);

                response.EnsureSuccessStatusCode();                
            }
        }
    }
}
