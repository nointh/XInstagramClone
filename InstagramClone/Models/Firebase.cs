using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using InstagramClone.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;


namespace InstagramClone.Models
{
    public class FirebaseDB
    {
        public static string FirebaseClient = "https://xinstagramclone-default-rtdb.asia-southeast1.firebasedatabase.app/";
        public static string FirebaseSecret = "Xd9r0ce0BzVfmNEkDJoFPWGB1JOMCPONrO6P18iq";

        public FirebaseClient firebase = new FirebaseClient(FirebaseClient,
                                   new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(FirebaseSecret) });

        public async Task<List<UserModel>> getAllUser()
        {
            try
            {
                return (await firebase
                .Child("user")
                .OnceAsync<UserModel>()).Select(item => new UserModel
                {
                    Key = item.Key,
                    Fullname = item.Object.Fullname,
                    Username = item.Object.Username,
                    ImageUri = item.Object.ImageUri,
                    Gender = item.Object.Gender,
                    DOB = item.Object.DOB,
                    Email = item.Object.Email,
                    Password = item.Object.Password,
                    Phone = item.Object.Phone,
                    Website = item.Object.Website,
                    ProfileDescription = item.Object.ProfileDescription,
                }).ToList();
            }
            catch (NullReferenceException ex)
            {
                return null;
            }
            
        }
        public async Task<UserModel> getUser(string username)
        {
            var users = await getAllUser();

            return users.Where(user => user.Username == username).FirstOrDefault();
        }
        public async Task<UserModel> getUserByKey(string key)
        {
            var users = await getAllUser();

            return users.Where(user => user.Key == key).FirstOrDefault();
        }
        public async Task addUser(UserModel user)
        {
            await firebase
              .Child("user")
              .PostAsync(user);

            UserModel u = await getUser(user.Username);

            await firebase
              .Child("user")
              .Child(u.Key)
              .PutAsync(user);
        }
        public async Task updateUser(UserModel user)
        {
            await firebase
              .Child("user")
              .Child(user.Key)
              .PutAsync(new UserModel() { 
                  Username = user.Username,

              });
        }
        public async Task<List<FollowUser>> getFollower(string UserKey)
        {
            return (await firebase
                .Child("follower")
                .Child(UserKey)
                .OnceAsync<FollowUser>()).Select(item => new FollowUser()
                {
                    UserKey = item.Object.UserKey,
                }).ToList();
        }
        public async Task<List<FollowUser>> getFollowing(string UserKey)
        {
            return (await firebase
                .Child("following")
                .Child(UserKey)
                .OnceAsync<FollowUser>()).Select(item => new FollowUser()
                {
                    UserKey = item.Object.UserKey,
                }).ToList();
        }
        public async Task<Boolean> checkIsFollow(string UserKey1, string UserKey2)
        {
            var result = (await firebase
                .Child("follower")
                .Child(UserKey2)
                .OnceAsync<FollowUser>()).Where(a => a.Key == UserKey1).FirstOrDefault();
            
            if (result != null) 
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
        public async Task<string> updateFollow(FollowUser user1, FollowUser user2)
        {
            if (await checkIsFollow(user1.UserKey, user2.UserKey))
            {
                var toDeleteFollowing = (await firebase
                    .Child("following")
                    .Child(user1.UserKey)
                    .OnceAsync<FollowUser>()).Where(a => a.Object.UserKey == user2.UserKey).FirstOrDefault();

                var toDeleteFollower = (await firebase
                    .Child("follower")
                    .Child(user2.UserKey)
                    .OnceAsync<FollowUser>()).Where(a => a.Object.UserKey == user1.UserKey).FirstOrDefault();

                await firebase.Child("following")
                    .Child(user1.UserKey)
                    .Child(user2.UserKey)
                    .DeleteAsync();

                await firebase.Child("follower")
                    .Child(user2.UserKey)
                    .Child(user1.UserKey)
                    .DeleteAsync();

                return "unfollow";
            }
            else
            {
                await firebase
                       .Child("follower")
                       .Child(user2.UserKey)
                       .PostAsync(user1);

                await firebase
                       .Child("following")
                       .Child(user1.UserKey)
                       .PostAsync(user2);

                return "follow";
            }
        }
    
    }
}
