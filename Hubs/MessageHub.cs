using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using mixtape.Models;
using Mixtape.Models;

namespace mixtape.Hubs
{

    public class User
    {
        public string ID { get; set; }
        public int Time { get; set; }
        public bool First { get; set; }
    }

    public class MessageHub: Hub
    {
        private static ConcurrentQueue<User> users = new ConcurrentQueue<User>();

        public override Task OnConnectedAsync()
        {
            //Need to lock this incase two people join at once and are "both" the first
            lock(users)
            {
                string id = Context.ConnectionId;

                //first guy in!
                if (users.IsEmpty)
                {
                    User u = new User
                    {
                        ID = id,
                        Time = 0,
                        First = true
                    };

                    users.Enqueue(u);
                    Clients.Client(u.ID).SendAsync("startSongAt", u);
                }
                else //If we already have someone in the queue, set their time to the first guy's time
                {
                    User u = new User
                    {
                        ID = id,
                        Time = users.First().Time,
                        First = false
                    };

                    users.Enqueue(u);
                    Clients.Client(u.ID).SendAsync("startSongAt", u);
                }
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            lock (users)
            {
                string id = Context.ConnectionId;

                //If there is only one person, then we want to just return after dequeuing
                if (users.Count == 1)
                {
                    User u;
                    users.TryDequeue(out u);
                    return base.OnDisconnectedAsync(exception);
                }


                User firstUser;
                users.TryPeek(out firstUser);

                User popped;

                //If the user leaving is in the front (best case) we pop them and set the next person to first and update them
                if (firstUser.First && firstUser.ID == id)
                {
                    users.TryDequeue(out firstUser); //remove the top one
                    users.TryPeek(out popped); //Get ref to the now front, set its value

                    //Set the next person in the queue's time to the first persons
                    popped.First = true;
                    popped.Time = firstUser.Time;

                    Clients.Client(popped.ID).SendAsync("setUserToFirst", popped);
                }
                else //If the user to leave isn't first we must rebuild the queue (worst case) 
                {
                    ConcurrentQueue<User> newQ = new ConcurrentQueue<User>();

                    bool found = false;
                    foreach (User u in users.ToList()) //for each connected user
                    {
                        users.TryDequeue(out popped);

                        if (!found) //if we haven't found them yet, lets try to
                        {
                            if(popped.ID == id) //we found them, so lets not add them
                            {
                                found = true;
                                continue;
                            }
                            else
                            {
                                newQ.Enqueue(popped);
                            }
                        }
                        else
                        {
                            newQ.Enqueue(popped);
                        }
                    }

                    users = newQ;
                }
            }
            
            return base.OnDisconnectedAsync(exception);
        }

        public void Update(MessageData message) //called when we add a song
        {
            Clients.All.SendAsync("updateSong", message);
        }

        public void ListUpdate(List<MessageData> messages) //called when we make an edit to the list of songs
        {
            Clients.All.SendAsync("broadcastMessage", messages);
        }

        public void NextSongUpdate(List<MessageData> messages) //called when we move to the next song
        {
            User popped;
            users.TryPeek(out popped);

            popped.Time = 0;

            Clients.All.SendAsync("broadcastMessage", messages);
        }

        public void GetHeartbeat(int time)
        {
            User popped;
            users.TryPeek(out popped);

            popped.Time += time;
        }
    }
}
