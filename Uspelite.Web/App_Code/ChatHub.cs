//namespace Uspelite.Web
//{
//    using System.Threading.Tasks;
//    using Microsoft.AspNet.SignalR;
//    using System.Collections.Generic;
//    using System.Linq;
//    using Models;

//    public class ChatHub : Hub
//    {
//        private static List<UserDetails> ConnectedUsers = new List<UserDetails>();
//        private static List<MessageDetails> CurrentMessage = new List<MessageDetails>();

//        public void Connect(string userName)
//        {
//            var id = this.Context.ConnectionId;

//            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
//            {
//                ConnectedUsers.Add(new UserDetails { ConnectionId = id, UserName = userName });
//                // send to caller
//                this.Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);
//                // send to all except caller client
//                this.Clients.AllExcept(id).onNewUserConnected(id, userName);
//            }
//        }

//        public void SendMessageToAll(string userName, string message)
//        {
//            // store last 100 messages in cache
//            this.AddMessageinCache(userName, message);
//            // Broadcast message
//            this.Clients.All.messageReceived(userName, message);
//        }

//        public void SendPrivateMessage(string toUserId, string message)
//        {
//            string fromUserId = this.Context.ConnectionId;

//            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
//            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

//            if (toUser != null && fromUser != null)
//            {
//                // send to 
//                this.Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

//                // send to caller user
//                this.Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
//            }

//        }

//        public override Task OnDisconnected(bool stopCalled)
//        {
//            var user = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == this.Context.ConnectionId);
//            if (user != null)
//            {
//                ConnectedUsers.Remove(user);
//                var id = this.Context.ConnectionId;
//                this.Clients.All.onUserDisconnected(id, user.UserName);
//            }
//            return base.OnDisconnected(stopCalled);
//        }

//        private void AddMessageinCache(string userName, string message)
//        {
//            CurrentMessage.Add(new MessageDetails { UserName = userName, Message = message });

//            if (CurrentMessage.Count > 100)
//                CurrentMessage.RemoveAt(0);
//        }
//    }
//}