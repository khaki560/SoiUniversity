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

            return query.FirstOrDefault();
        }

        public IEnumerable<UserEntry> GetAll()
        {
            var query = from b in db.Entries
                        select b;

            return query.AsEnumerable().Select(item =>
                new UserEntry()
                {
                    Name = item.Name,
                    Pass = item.Pass,
                    PubKey = item.PubKey
                });
        }

        public void Add(UserEntry user)
        {
            db.Entries.Add(user);
            db.SaveChanges();
        }

        public void ModifyKey(string userName, string newPubKey)
        {
            var query = from b in db.Entries
                        where b.Name == userName
                        select b;

            var user = query.FirstOrDefault();
            user.PubKey = newPubKey;
            db.SaveChanges();
        }

        public void Remove(string userName)
        {
            var query = from b in db.Entries
                        where b.Name == userName
                        select b;

            var user = query.FirstOrDefault();
            db.Entries.Remove(user);
            db.SaveChanges();
        }

        public void RemoveAll()
        {
            db.Entries.RemoveRange(db.Entries);
            db.SaveChanges();
        }
        public void Dispose()
        {
            _log.Debug("UsersRepository Dispose");
            throw new NotImplementedException();
        }
    }
}