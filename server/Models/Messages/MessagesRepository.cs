﻿using System;
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

        private IEnumerable<MessageEntry> ConvertToColection(IQueryable<MessageEntry> query)
        {
            return query.AsEnumerable().Select(item =>
                new MessageEntry()
                {
                    Id = item.Id,
                    Time = item.Time,
                    From = item.From,
                    To = item.To,
                    Message = item.Message,
                    Key = item.Key,
                    Downloaded = item.Downloaded,
                    Title = item.Title
                }
            );
        }
        public IEnumerable<MessageEntry> GetRecieved(string to, bool onlyNew)
        {
            var query = from b in db.Entries
                        where b.To == to
                        select b;

            var messages = ConvertToColection(query);
            if (onlyNew)
            {
                messages = messages.Where(x => x.Downloaded == false);
            }

            if (_log.IsDebugEnabled)
            {
                _log.Debug("GetRecieved returned:");
                foreach (var message in messages)
                {
                    _log.DebugFormat("Time:{0}, From:{1}, To:{2}, Message:{3}, Downloaded:{4}", message.Time.ToString(), message.From, message.To, message.Message, message.Downloaded);
                }
            }
            return messages;
        }

        public IEnumerable<MessageEntry> GetSent(string f, bool onlyNew)
        {
            var query = from b in db.Entries
                        where b.From == f
                        select b;

            var messages = ConvertToColection(query);
            if (onlyNew)
            {
                messages = messages.Where(x => x.Downloaded == false);
            }

            if (_log.IsDebugEnabled)
            { 
                _log.Debug("GetSent returned:");
                foreach (var message in messages)
                {
                    _log.DebugFormat("Time:{0}, From:{1}, To:{2}, Message:{3}, Downloaded:{4}", message.Time.ToString(), message.From, message.To, message.Message, message.Downloaded);
                }
            }

            return messages;
        }

        public IEnumerable<MessageEntry> GetAll()
        {
            var query = from b in db.Entries
                        select b;

            var messages = ConvertToColection(query);

            if (_log.IsDebugEnabled)
            {
                _log.Debug("GetAll returned:");
                foreach (var message in messages)
                {
                    _log.DebugFormat("Time:{0}, From:{1}, To:{2}, Message:{3}, Downloaded:{4}", message.Time.ToString(), message.From, message.To, message.Message, message.Downloaded);
                }
            }

            return messages;
        }

        public void Add(MessageEntry message)
        {
            db.Entries.Add(message);
            db.SaveChanges();
            _log.DebugFormat("Added message Time:{0}, From:{1}, To:{2}, Message:{3}, Downloaded:{4}", message.Time.ToString(), message.From, message.To, message.Message, message.Downloaded);
        }
        public void RemoveAll()
        {
            db.Entries.RemoveRange(db.Entries);
            db.SaveChanges();
            _log.Debug("Removed all messages");
        }

        public void MarkAsDownlaoded(List<int> ids)
        {
            foreach (var id in ids)
            {
                var query = from b in db.Entries
                            where b.Id == id
                            select b;

                var message = query.Single();
                var oldstatus = message.Downloaded;
                message.Downloaded = true;
                _log.DebugFormat("Changed \"Downloaded\" status of  Message: Time:{0}, From:{1}, To:{2}, Message:{3}, From {4} To {5}", message.Time.ToString(), message.From, message.To, message.Message, oldstatus, message.Downloaded);
            }

            db.SaveChanges();
        }

        public void Dispose()
        {
            _log.Debug("MessagesContext Dispose");
            db.Dispose();
        }
    }
}