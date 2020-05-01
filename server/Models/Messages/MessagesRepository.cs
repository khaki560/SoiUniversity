using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace server.Models.Messages
{
    public class MessagesRepository : IDisposable
    {
        private readonly MessagesContext db = new MessagesContext(ConfigurationManager.ConnectionStrings["UsersConnectionStr"].ConnectionString);
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public IEnumerable<MessageEntry> GetRecieved(string to)
        {
            var query = from b in db.Entries
                        where b.To == to
                        select b;

            var messages = query.AsEnumerable().Select(item =>
                new MessageEntry()
                {
                    Id = item.Id,
                    Time = item.Time,
                    From = item.From,
                    To = item.To,
                    Message = item.Message
                }
            );

            if (_log.IsDebugEnabled)
            {
                _log.Debug("GetRecieved returned:");
                foreach (var message in messages)
                {
                    _log.DebugFormat("Time:{0}, From:{1}, To:{2}, Message{3}", message.Time.ToString(), message.From, message.To, message.Message);
                }
            }
            return messages;
        }

        public IEnumerable<MessageEntry> GetSent(string f)
        {
            var query = from b in db.Entries
                        where b.From == f
                        select b;

            var messages = query.AsEnumerable().Select(item =>
                new MessageEntry()
                {
                    Id = item.Id,
                    Time = item.Time,
                    From = item.From,
                    To = item.To,
                    Message = item.Message
                }
            );
            if(_log.IsDebugEnabled)
            { 
                _log.Debug("GetSent returned:");
                foreach (var message in messages)
                {
                    _log.DebugFormat("Time:{0}, From:{1}, To:{2}, Message{3}", message.Time.ToString(), message.From, message.To, message.Message);
                }
            }

            return messages;
        }

        public IEnumerable<MessageEntry> GetAll()
        {
            var query = from b in db.Entries
                        select b;

            var messages = query.AsEnumerable().Select(item =>
                new MessageEntry()
                {
                    Id = item.Id,
                    Time = item.Time,
                    From = item.From,
                    To = item.To,
                    Message = item.Message
                }
            );

            if (_log.IsDebugEnabled)
            {
                _log.Debug("GetAll returned:");
                foreach (var message in messages)
                {
                    _log.DebugFormat("Time:{0}, From:{1}, To:{2}, Message{3}", message.Time.ToString(), message.From, message.To, message.Message);
                }
            }

            return messages;
        }

        public void Add(MessageEntry message)
        {
            db.Entries.Add(message);
            db.SaveChanges();
            _log.DebugFormat("Added message Time:{0}, From:{1}, To:{2}, Message{3}", message.Time.ToString(), message.From, message.To, message.Message);
        }
        public void RemoveAll()
        {
            db.Entries.RemoveRange(db.Entries);
            db.SaveChanges();
            _log.Debug("Removed all messages");
        }

        public void Dispose()
        {
            _log.Debug("MessagesContext Dispose");
            throw new NotImplementedException();
        }
    }
}