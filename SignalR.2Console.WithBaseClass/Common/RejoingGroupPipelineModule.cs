using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Common
{
    public class RejoingGroupPipelineModule : HubPipelineModule
    {
        public override Func<HubDescriptor, IRequest, IList<string>, IList<string>>
            BuildRejoiningGroups(Func<HubDescriptor, IRequest, IList<string>, IList<string>>
            rejoiningGroups)
        {
            rejoiningGroups = (hb, r, l) =>
            {
                var assignedTopics = new List<string>();
                using (var db = new UserContext())
                {
                    var user = db.Users.Include("Topics")
                        .Single(u => u.UserName == r.User.Identity.Name);

                    foreach (var item in user.Topics)
                    {
                        assignedTopics.Add(item.TopicName);
                    }
                }
                return assignedTopics;
            };

            return rejoiningGroups;
        }
    }
}