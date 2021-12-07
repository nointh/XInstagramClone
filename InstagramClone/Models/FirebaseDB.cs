﻿using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InstagramClone.Models
{
    class FirebaseDB
    {
        private static readonly string WebAPIKey = "AIzaSyCc-Lrg3ue3OTaFHfYhtQZtgvQZHtsJAUs";
        private static readonly string _firebaseDatabaseURL = "https://xinstagramclone-default-rtdb.asia-southeast1.firebasedatabase.app/";
        private static string _currentUserId;
        public static string CurrentUserId {
            get
            {
                if (_currentUserId == null)
                {
                    return Convert.ToString(Preferences.Get("UID", ""));
                }
                return _currentUserId;
            }
            set {
                _currentUserId = value;
            } }
        public static FirebaseClient firebaseClient = new FirebaseClient(_firebaseDatabaseURL);
        public static FirebaseAuthProvider GetAuthProvider()
        {
            return new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
        }
        public static async Task AddUser(UserModel user)
        {
            await firebaseClient
              .Child("user")
              .Child(user.UID)
              .PutAsync(user.GetValue());
        }
        public static async Task<List<PostModel>> GetAllPostOfUser(string UID)
        {
            List<PostModel> list = new List<PostModel>();
            try
            {
                var post = await firebaseClient
                    .Child("post")
                    .Child(UID)
                    .OnceAsync<PostModel>();
                var ownerUser = (await firebaseClient
                    .Child("user")
                    .OrderByKey()
                    .StartAt(UID)
                    .LimitToFirst(1)
                    .OnceAsync<UserModel>()
                    ).Select(item => new UserModel
                    {
                        Username = item.Object.Username,
                        ImageUri = item.Object.ImageUri
                    }).FirstOrDefault();
                list = post.Select((item) => {
                    return new PostModel
                    {
                        OwnerUsername = ownerUser.Username,
                        OwnerImage = ownerUser.ImageUri,
                        PostId = item.Key,
                        Caption = item.Object.Caption,
                        PostTime = item.Object.PostTime
                    };
                }).ToList();
                foreach(var item in list)
                {
                    ObservableCollection<Media> mediaList = new ObservableCollection<Media>();
                    foreach (var mediaContent in await GetMediaListOfPost(UID, item.PostId))
                    {
                        mediaList.Add(Media.ParseContent(mediaContent));
                    }
                    item.MediaList = mediaList;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return list;
        }
        public static async Task<List<MediaContent>> GetMediaListOfPost(string UID, string postId)
        {
            var mediaList = await firebaseClient
                .Child("post")
                .Child(UID)
                .Child(postId)
                .Child("MediaContent")
                .OnceAsync<MediaContent>();
            List<MediaContent> resultList = mediaList.Select(item => new MediaContent
            { 
                Type = item.Object.Type,
                Url = item.Object.Url,
            }).ToList();

            return resultList;
        }
        public static async Task<List<UserModel>> GetFollowingUser(string UID)
        {
            List<UserModel> resultList = new List<UserModel>();
            try
            {
                var users = await firebaseClient
                .Child("user")
                .Child(UID)
                .Child("following")
                .OnceAsync<UserModel>();
                resultList = users.Select(item => new UserModel
                {
                    UID = item.Key,
                    Fullname = item.Object.Fullname,
                    Username = item.Object.Username,
                    ImageUri = item.Object.ImageUri
                }).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }          

            return resultList;
        }
        public static async Task<List<PostModel>> GetNewsfeedPost()
        {
            List<PostModel> resultList = new List<PostModel>();
            string UID = CurrentUserId;
            try
            {
                var followingList = await GetFollowingUser(UID);
                foreach(var user in followingList)
                {
                    List<PostModel> postList = await GetAllPostOfUser(user.UID);
                    foreach(var post in postList)
                    {
                        resultList.Add(post);
                    }
                }
                return resultList.OrderByDescending(post => post.PostTime).ToList() ;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return resultList;
        }
    }
}
