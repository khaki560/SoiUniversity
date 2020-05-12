using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace server.Models.Users
{
    public class UsersRepository : IDisposable
    {
        private readonly UsersContext db = new UsersContext(ConfigurationManager.ConnectionStrings["UsersConnectionStr"].ConnectionString);
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public UserEntry Get(string userName)
        {
            var query = from b in db.Entries
                        where b.Name == userName
                        select b;

            if (_log.IsDebugEnabled)
            {
                var users =  query.AsEnumerable().Select(item =>
                new UserEntry()
                {
                    Name = item.Name,
                    Pass = item.Pass,
                    PubKey = item.PubKey,
                    PrivateKey = item.PrivateKey
                });

                _log.Debug("Get Returned:");
                foreach(var user in users)
                {
                    _log.DebugFormat("Name {0}, Password {1}, PubKey:{2} PrivateKey:{3}", user.Name, user.Pass, user.PubKey, user.PrivateKey);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<UserEntry> GetAll()
        {
            var query = from b in db.Entries
                        select b;

            var users = query.AsEnumerable().Select(item =>
                new UserEntry()
                {
                    Name = item.Name,
                    Pass = item.Pass,
                    PubKey = item.PubKey,
                    PrivateKey = item.PrivateKey
                });

            if (_log.IsDebugEnabled)
            {
                _log.Debug("GetAll Returned:");
                foreach (var user in users)
                {
                    _log.DebugFormat("Name {0}, Password {1}, PubKey:{2} PrivateKey:{3}", user.Name, user.Pass, user.PubKey, user.PrivateKey);
                }
            }

            return users;
        }

        public void Add(UserEntry user)
        {
            db.Entries.Add(user);
            db.SaveChanges();
            _log.DebugFormat("Name {0}, Password {1}, PubKey:{2} PrivateKey:{3}", user.Name, user.Pass, user.PubKey, user.PrivateKey);
        }

        public void ModifyKey(string userName, string newPubKey)
        {
            var query = from b in db.Entries
                        where b.Name == userName
                        select b;

            var user = query.FirstOrDefault();
            var oldPubKey = user.PubKey;
            user.PubKey = newPubKey;
            db.SaveChanges();
            _log.DebugFormat("Modified User Name {0}, Password {1}, PubKey: :{2} -> {3}", user.Name, user.Pass, oldPubKey, user.PubKey);

        }

        public void Remove(string userName)
        {
            var query = from b in db.Entries
                        where b.Name == userName
                        select b;

            var user = query.FirstOrDefault();
            if(user != null)
            {
                db.Entries.Remove(user);
                db.SaveChanges();
                _log.DebugFormat("Name {0}, Password {1}, PubKey:{2} PrivateKey:{3}", user.Name, user.Pass, user.PubKey, user.PrivateKey);
            }
            else
            {
                _log.DebugFormat("User to remove not found Name: {0}", userName);
            }
        }

        public void RemoveAll()
        {
            db.Entries.RemoveRange(db.Entries);
            db.SaveChanges();
            _log.Debug("All users removed");
        }
        public void Dispose()
        {
            _log.Debug("UsersRepository Dispose");
            db.Dispose();
        }
    }
}