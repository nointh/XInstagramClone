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
            return (await firebase
                .Child("user")
                .OnceAsync<UserModel>()).Select(item => new UserModel
                {
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
                    //Follower = item.Object.Follower,
                    //Following = item.Object.Following,
                }).ToList();
        }

        public async Task<UserModel> getUser(string username)
        {
            var users = await getAllUser();

            return users.Where(user => user.Username == username).FirstOrDefault();
        }
        
        public async Task addUser(UserModel user)
        {
            await firebase
              .Child("user")
              .PostAsync(user);
        }

        public async Task updateUser(UserModel user)
        {
            var toUpdateUser = (await firebase
              .Child("user")
              .OnceAsync<UserModel>()).Where(u => u.Object.Username == user.Username).FirstOrDefault();

            await firebase
              .Child("user")
              .Child(toUpdateUser.Key)
              .PutAsync(user);
        }

        public async Task<List<FollowUser>> getFollower(string Username)
        {
            return (await firebase
                .Child("user")
                .Child(Username)
                .Child("Follower")
                .OnceAsync<FollowUser>()).Select(item => new FollowUser()
                {
                    Username = item.Object.Username,
                    ImageUri = item.Object.ImageUri,
                }).ToList();
        }

        public async Task<List<FollowUser>> getFollowing(string Username)
        {
            return (await firebase
                .Child("user")
                .Child(Username)
                .Child("Following")
                .OnceAsync<FollowUser>()).Select(item => new FollowUser()
                {
                    Username = item.Object.Username,
                    ImageUri = item.Object.ImageUri,
                }).ToList();
        }
    }
}
