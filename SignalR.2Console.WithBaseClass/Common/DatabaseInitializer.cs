using System.Collections.Generic;
using System.Data.Entity;

namespace Common
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<UserContext>
    {
        protected override void Seed(UserContext context)
        {
/*
            var topic1 = new Topic() {TopicId = 1, TopicName = "Great Topic"};
            var topic2 = new Topic() {TopicId = 2, TopicName = "Boring Topic"};
*/
            var topic1 = new Topic() { TopicName = "Great Topic" };
            var topic2 = new Topic() { TopicName = "Boring Topic" };


            var topics = new List<Topic> { topic1, topic2 };

            foreach (var topic in topics)
            {
                context.Topics.Add(topic);
            }

            // todo: add ad-hoc topics
/*

            var user1 = new User() { UserId = 1, UserName = "user1"};
            var user2 = new User() { UserId = 2, UserName = "user2", Topics = { topic1, topic2 } };
            var user3 = new User() { UserId = 2, UserName = "user3", Topics = { topic1 } };
            var user4 = new User() { UserId = 2, UserName = "user4", Topics = { topic1 } };
            var user5 = new User() { UserId = 2, UserName = "user5", Topics = { topic2 } };
            var user6 = new User() { UserId = 2, UserName = "user6", Topics = { topic1, topic2 } };

*/
            var user1 = new User() { UserName = "user1" };
            var user2 = new User() { UserName = "user2", Topics = { topic1, topic2 } };
            var user3 = new User() { UserName = "user3", Topics = { topic1 } };
            var user4 = new User() { UserName = "user4", Topics = { topic1 } };
            var user5 = new User() { UserName = "user5", Topics = { topic2 } };
            var user6 = new User() { UserName = "user6", Topics = { topic1, topic2 } };


            var users = new List<User> { user1, user2, user3, user4, user5, user6 };



            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}