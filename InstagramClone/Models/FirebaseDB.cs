using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public static FirebaseStorage firebaseStorage = new FirebaseStorage("xinstagramclone.appspot.com");
        public static FirebaseAuthProvider GetAuthProvider()
        {
            return new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
        }

        //Nội
        public static async Task AddUser(UserModel user)
        {
            //await firebaseClient
            //  .Child("user")
            //  .Child(user.UID)
            //  .PutAsync(user.GetValue());
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
                        PostTime = item.Object.PostTime,
                        OwnerId = item.Object.OwnerId
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
        //End Nội

        //Danh
        public static async Task<List<CommentModel>> GetPostComments(string ownerId, string postId)
        {
            return (
              await firebaseClient
              .Child("post")
              .Child(ownerId)
              .Child(postId)
              .Child("Comment")
              .OnceAsync<CommentModel>()).Select(item => new CommentModel
              {
                  Username = item.Object.Username,
                  UserImage = item.Object.UserImage,
                  CommentDetail = item.Object.CommentDetail,
                  PostTime = item.Object.PostTime,
                  CommentId = item.Key,
                  OwnerId = item.Object.OwnerId
              }
            ).ToList();
        }

        public static async Task<List<UserLiked>> GetCommentUserLiked(string ownerId, string postId, string cmtId)
        {
            return (
              await firebaseClient
              .Child("post")
              .Child(ownerId)
              .Child(postId)
              .Child("Comment")
              .Child(cmtId)
              .Child("UserLiked")
              .OnceAsync<UserLiked>()).Select(item => new UserLiked
              {
                  UserId = item.Object.UserId,
              }
            ).ToList();
        }

        public static async Task AddComment(string ownerId, string postId, CommentModel cmt)
        {
            await firebaseClient
              .Child("post")
              .Child(ownerId)
              .Child(postId)
              .Child("Comment")
              .PostAsync(new CommentModel() { Username = cmt.Username, UserImage = cmt.UserImage, PostTime = cmt.PostTime, CommentDetail = cmt.CommentDetail, OwnerId = CurrentUserId });
        }

        public static async Task AddUserLikeForComment(string ownerId, string postId, string cmtId, UserLiked usr)
        {
            await firebaseClient
              .Child("post")
              .Child(ownerId)
              .Child(postId)
              .Child("Comment")
              .Child(cmtId)
              .Child("UserLiked")
              .PostAsync(usr);
        }

        public static async Task DeleteUserLikeForComment(string ownerId, string postId, string cmtId, string uid)
        {
            var toDeletePerson = (await firebaseClient
              .Child("post")
              .Child(ownerId)
              .Child(postId)
              .Child("Comment")
              .Child(cmtId)
              .Child("UserLiked")
              .OnceAsync<UserLiked>()).Where(a => a.Object.UserId == uid).FirstOrDefault();

            await firebaseClient
                .Child("post")
                .Child(ownerId)
                .Child(postId)
                .Child("Comment")
                .Child(cmtId)
                .Child("UserLiked")
                .Child(toDeletePerson.Key)
                .DeleteAsync();
        }

        public static async Task DeleteComment(string ownerId, string postId, string cmtId)
        {
            await firebaseClient
                .Child("post")
                .Child(ownerId)
                .Child(postId)
                .Child("Comment")
                .Child(cmtId)
                .DeleteAsync();
        }

        public static async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            await firebaseStorage
                    .Child(CurrentUserId + "_" + fileName)
                    .PutAsync(fileStream);

            return await GetFileUrl(fileName);
        }

        public static async Task<string> GetFileUrl(string fileName)
        {
            return await firebaseStorage
                .Child(CurrentUserId + "_" + fileName)
                .GetDownloadUrlAsync();
        }

        public static async Task AddPost(PostModelBasic post)
        {
            await firebaseClient
              .Child("post")
              .Child(CurrentUserId)
              .PostAsync(post);
        }

        public static async Task<string> GetPostId(PostModelBasic model)
        {
            var toAddPost = (await firebaseClient
              .Child("post")
              .Child(CurrentUserId)
              .OnceAsync<PostModelBasic>()).Where(a => a.Object.Caption == model.Caption).FirstOrDefault();

            return toAddPost.Key;
        }

        public static async Task AddMediaToPost(string postId, MediaContent media)
        {
            await firebaseClient
              .Child("post")
              .Child(CurrentUserId)
              .Child(postId)
              .Child("MediaContent")
              .PostAsync(media);
        }
        //End Danh

    }
}
