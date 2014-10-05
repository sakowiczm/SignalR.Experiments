using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Common
{
    public class SpencerStuartHub : Hub
    {
        public override Task OnConnected()
        {
            Console.WriteLine("OnConnected");

            // todo: make all operation async

            // this is just for this example in production code we will most likely use Authorize attribure
            string userName = GetUserName();
            Console.WriteLine("UserName: " + userName);


            SetUserGroups(userName);
            AddUserConnection(userName, Context.ConnectionId);

            return base.OnConnected();
        }

        private void RemoveUserConnection(string userName, string connectionId)
        {
            using (var db = new UserContext())
            {
                var connection = db.Connections.Find(Context.ConnectionId);
                connection.Connected = false;
                db.SaveChanges();
            }            
        }

        private void AddUserConnection(string userName, string connectionId)
        {
            using (var db = new UserContext())
            {
                var user = db.Users
                    .Include("Connections")
                    .SingleOrDefault(u => u.UserName == userName);

                if (user != null)
                {
                    user.Connections.Add(new Connection
                    {
                        ConnectionId = Context.ConnectionId,
                        UserAgent = Context.Request.Headers["User-Agent"],
                        Connected = true
                    });
                    db.SaveChanges();                    
                }
            }
        }

        private void SetUserGroups(string userName)
        {
            using (var db = new UserContext())
            {
                // it is not security check so having name 

                var user = db.Users
                    .Include("Topics")
                    .SingleOrDefault(u => u.UserName == userName);

                if (user != null)
                {
                    // Add to each assigned group.
                    foreach (var item in user.Topics)
                    {
                        Groups.Add(Context.ConnectionId, item.TopicName);
                    }
                }

            }
        }

        public void SubscribeUserToTopic(string userName, string topicName)
        {
            using (var db = new UserContext())
            {
                var room = db.Topics.Find(topicName);

                if (room != null)
                {
                    var user = new User() { UserName = userName };
                    db.Users.Attach(user);

                    room.Users.Add(user);
                    db.SaveChanges();
                    Groups.Add(Context.ConnectionId, topicName);
                }
            }
        }

        public void UnsubscribeUserFromTopic(string userName, string topicName)
        {
            using (var db = new UserContext())
            {
                var room = db.Topics.Find(topicName);
                if (room != null)
                {
                    var user = new User() { UserName = userName };
                    db.Users.Attach(user);

                    room.Users.Remove(user);
                    db.SaveChanges();
                    
                    Groups.Remove(Context.ConnectionId, topicName);
                }
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // todo: need to have some backgorund process that will be clearing 'old' connections that were just lost and not properly Disconnected

            Console.WriteLine("OnDisconnected");

            string userName = GetUserName();
            Console.WriteLine("UserName: " + userName);

            RemoveUserConnection(userName, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        private string GetUserName()
        {
            string userName = Context.Headers["userName"];
            return userName;
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("OnReconnected");
            return base.OnReconnected();
        }
    }
}