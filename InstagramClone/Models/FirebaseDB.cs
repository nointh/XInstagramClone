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
        public static async Task<UserModel> GetCurentUserInfo()
        {
            UserModel currentUser = new UserModel();
            try
            {
                currentUser = (await firebaseClient
                .Child("user")
                .OrderByKey()
                .StartAt(CurrentUserId)
                .LimitToFirst(1)
                .OnceAsync<UserModel>()).ToList().Select(i => new UserModel { 
                    UID = i.Key,
                    ImageUri = i.Object?.ImageUri,
                    Email = i.Object?.Email,
                    Fullname = i.Object?.Fullname,
                    Username = i.Object?.Username,
                    DOB = i.Object?.DOB,
                    Gender = i.Object?.Gender,
                    ProfileDescription = i.Object?.ProfileDescription,
                    Phone = i.Object?.Phone,
                    Website = i.Object?.Website
                }).FirstOrDefault();
                return currentUser;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return currentUser;
            }
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
                //Get media and userlike of the post
                foreach(var item in list)
                {
                    //get media
                    ObservableCollection<Media> mediaList = new ObservableCollection<Media>();
                    foreach (var mediaContent in await GetMediaListOfPost(UID, item.PostId))
                    {
                        mediaList.Add(Media.ParseContent(mediaContent));
                    }
                    item.MediaList = mediaList;
                    //get liked users
                    List<UserLiked> likedUsers = await GetLikedUsersOfPost(item.PostId, item.OwnerId);
                    item.LikedUsers = likedUsers;
                    foreach(var userliked in likedUsers)
                    {
                        if (userliked.UserId == CurrentUserId)
                            item.IsLiked = true;
                    }
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
                //get posts of current user
                List<PostModel> listPostOfOwnUser = await GetAllPostOfUser(UID);
                foreach (var post in listPostOfOwnUser)
                    resultList.Add(post);
                return resultList.OrderByDescending(post => post.PostTime).ToList() ;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return resultList;
        }
        public static async Task<List<UserLiked>> GetLikedUsersOfPost(string postId, string ownerId)
        {
            List<UserLiked> likedUsers = new List<UserLiked>();
            try
            {
                likedUsers = (await firebaseClient
                  .Child("post")
                  .Child(ownerId)
                  .Child(postId)
                  .Child("UserLiked")
                  .OnceAsync<UserLiked>()).Select(item => new UserLiked
                  {
                      UserId = item.Object.UserId,
                  }
                ).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return likedUsers;
        }
        public static async Task SetLikedToPost(PostModel post)
        {
            await firebaseClient.Child("post")
                .Child(post.OwnerId)
                .Child(post.PostId)
                .Child("UserLiked")
                .PostAsync<UserLiked>(new UserLiked { UserId = CurrentUserId });
            //SendNotificationToUser 
            UserModel currentUser = await GetCurentUserInfo();
            NotificationModel noti = new NotificationModel
            {
                UserId = CurrentUserId,
                PostId = post.PostId,
                PostCaption = post?.Caption,
                Type = "postlike",
                Username = currentUser.Username,
                Image = currentUser?.ImageUri,
                Time = (DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
            };
            await SendNotificationToUser(noti, post.OwnerId);
        }
        public static async Task SetUnlikedToPost(PostModel post)
        {
            var likedUser = (await firebaseClient
                .Child("post")
                .Child(post.OwnerId)
                .Child(post.PostId)
                .Child("UserLiked")
                .OnceAsync<UserLiked>())
                .Where(item => item.Object.UserId == CurrentUserId).FirstOrDefault();

            await firebaseClient
                .Child("post")
                .Child(post.OwnerId)
                .Child(post.PostId)
                .Child("UserLiked")
                .Child(likedUser.Key)
                .DeleteAsync();
        }
        public static async Task SendNotificationToUser(NotificationModel noti, string OwnerId)
        {
            try
            {
                await firebaseClient.Child("notification")
                    .Child(OwnerId)
                    .PostAsync(noti);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        //End Nội

        //Dũng
        public async Task<List<UserModel>> getAllUser()
        {
            try
            {
                return (await firebaseClient
                .Child("user")
                .OnceAsync<UserModel>()).Select(item => new UserModel
                {
                    UID = item.Key,
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

            return users.Where(user => user.UID == key).FirstOrDefault();
        }
        public async Task addUser(UserModel user)
        {
            await firebaseClient
              .Child("user")
              .PostAsync(user);

            UserModel u = await getUser(user.Username);

            await firebaseClient
              .Child("user")
              .Child(u.UID)
              .PutAsync(user);
        }
        public async Task updateUser(UserModel user)
        {
            await firebaseClient
              .Child("user")
              .Child(user.UID)
              .PutAsync(new UserModel()
              {
                  Username = user.Username,

              });
        }
        public async Task<List<FollowUser>> getFollower(string UserKey)
        {
            return (await firebaseClient
                .Child("follower")
                .Child(UserKey)
                .OnceAsync<FollowUser>()).Select(item => new FollowUser()
                {
                    UserKey = item.Object.UserKey,
                }).ToList();
        }
        public async Task<List<FollowUser>> getFollowing(string UserKey)
        {
            return (await firebaseClient
                .Child("following")
                .Child(UserKey)
                .OnceAsync<FollowUser>()).Select(item => new FollowUser()
                {
                    UserKey = item.Object.UserKey,
                }).ToList();
        }
        public async Task<Boolean> checkIsFollow(string UserKey1, string UserKey2)
        {
            var result = (await firebaseClient
                .Child("follower")
                .Child(UserKey2)
                .OnceAsync<FollowUser>()).Where(a => a.Object.UserKey == UserKey1).FirstOrDefault();

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
                var toDeleteFollowing = (await firebaseClient
                    .Child("following")
                    .Child(user1.UserKey)
                    .OnceAsync<FollowUser>()).Where(a => a.Object.UserKey == user2.UserKey).FirstOrDefault();

                var toDeleteFollower = (await firebaseClient
                    .Child("follower")
                    .Child(user2.UserKey)
                    .OnceAsync<FollowUser>()).Where(a => a.Object.UserKey == user1.UserKey).FirstOrDefault();

                await firebaseClient.Child("following")
                    .Child(user1.UserKey)
                    .Child(toDeleteFollowing.Key)
                    .DeleteAsync();

                await firebaseClient.Child("follower")
                    .Child(user2.UserKey)
                    .Child(toDeleteFollower.Key)
                    .DeleteAsync();

                return "unfollow";
            }
            else
            {
                await firebaseClient
                       .Child("follower")
                       .Child(user2.UserKey)
                       .PostAsync(user1);

                await firebaseClient
                       .Child("following")
                       .Child(user1.UserKey)
                       .PostAsync(user2);

                return "follow";
            }
        }

        //End Dũng

        //Danh
        public static async Task<SearchUserModel> GetUserById(string id)
        {
            return (
                await firebaseClient
                .Child("user")
                //.Child(id)
                .OnceAsync<SearchUserModel>()).Select(item => new SearchUserModel
                {
                    Id = item.Key,
                    Fullname = item.Object.Fullname,
                    Username = item.Object.Username,
                    ImageUri = item.Object.ImageUri,
                }).ToList().Where(a => a.Id==id).FirstOrDefault();
        }

        public static async Task<List<SearchUserModel>> GetUserOnSearchInput(string input)
        {
            return (
                await firebaseClient
                .Child("user")
                .OnceAsync<SearchUserModel>()).Select(item => new SearchUserModel
                {
                    Id = item.Key,
                    Fullname = item.Object.Fullname,
                    Username = item.Object.Username,
                    ImageUri = item.Object.ImageUri,
                }).ToList().Where(a => a.Username.Contains(input)).ToList();
        }

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
            //Sending notification for the post owner
            UserModel currentUser = await GetCurentUserInfo();
            NotificationModel noti = new NotificationModel
            {
                UserId = CurrentUserId,
                PostId = postId,
                CommentContent = cmt.CommentDetail,
                Type = "postcomment",
                Username = currentUser.Username,
                Image = currentUser?.ImageUri,
                Time = (DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
            };
            await SendNotificationToUser(noti, ownerId);


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

        public static async Task DeletePost(string postid)
        {
            await firebaseClient
              .Child("post")
              .Child(CurrentUserId)
              .Child(postid)
              .DeleteAsync();
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
