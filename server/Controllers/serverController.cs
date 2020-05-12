using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using server.Models.Messages;
using server.Models.Users;

namespace server.Controllers
{
    public class Credentials
    {
        public string Login { set; get; }
        public string Password { set; get; }
    }

    public class KeyRequest
    {
        public string Username { set; get; }
        public string PubKey { set; get; }
    }

    public class SingleMessage
    {
        public DateTime Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }
    public class ServerController : ApiController
    {
        // POST api/Sever/PostlogIn
        [HttpPost]
        public UserEntry PostlogIn([FromBody]Credentials cred)
        //public IHttpActionResult Postlog(string login, string password)
        {
            bool success = false;
            UserEntry user;
            using (var r = new UsersRepository())
            {
                user = r.Get(cred.Login);
                if (user != null)
                {
                    success = cred.Password == user.Pass;
                }
            }

            if (success == true)
                return user;
            return null;
        }

        [HttpPost]
        public IHttpActionResult PostRegister([FromBody]UserEntry user)
        //public IHttpActionResult Postlog(string login, string password)
        {
            bool success = false;
            string error = "";
            using (var r = new UsersRepository())
            {
                try
                {
                    r.Add(user);
                    success = true;
                }
                catch (DbUpdateException)
                {
                    error = "the user name is already in use";
                }
                catch
                {
                    error = "Unknown Error";
                }
            }

            if (success == true)
                return Ok();
            return BadRequest(error);
        }

        [HttpPost]
        public IHttpActionResult PostChangePupKey([FromBody]KeyRequest keyRequest)
        {
            bool success = true;
            string error = "";
            using (var r = new UsersRepository())
            {
                var user = r.Get(keyRequest.Username);
                if (user == null)
                {
                    success = false;
                    error = "Provided user doesn't exist";
                }
                else
                {
                    r.ModifyKey(keyRequest.Username, keyRequest.PubKey);
                }
            }

            if (success == true)
                return Ok();
            return BadRequest(error);
        }

        [HttpPost]
        public IEnumerable<MessageEntry> PostDownloadReceived([FromBody] string name)
        {
            IEnumerable<MessageEntry> messages;
            using (var r = new MessagesRepository())
            {
                messages = r.GetRecieved(name, true);
                return messages.ToList();
            }
            
        }

        [HttpPost]
        public IEnumerable<MessageEntry> PostDownloadSent([FromBody] string name)
        {
            IEnumerable<MessageEntry> messages;
            using (var r = new MessagesRepository())
            {
                messages = r.GetSent(name, true);
                return messages.ToList();
            }
            
        }


        [HttpPost]
        public IHttpActionResult PostUpload([FromBody] SingleMessage message)
        {

            using (var r = new MessagesRepository())
            {
                var m = new MessageEntry()
                {
                    Time = message.Time,
                    To = message.To,
                    From = message.From,
                    Message = message.Message,
                    Downloaded = false
                };

                r.Add(m);
            }
            return Ok();
        }

        public IHttpActionResult ConfirmDownloadOfMessages(List<int> ids)
        {
            using (var r = new MessagesRepository())
            {
                r.MarkAsDownlaoded(ids);
            }
            return Ok();
        }
    }
}
