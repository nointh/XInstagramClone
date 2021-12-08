using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;

namespace InstagramClone.Models
{
    class FirebaseDB
    {
        FirebaseClient firebase = new FirebaseClient("https://xinstagramclone-default-rtdb.asia-southeast1.firebasedatabase.app/");
        FirebaseStorage firebaseStorage = new FirebaseStorage("xinstagramclone.appspot.com");

        public async Task<List<CommentModel>> GetPostComments(string postId)
        {
            return (
              await firebase
              .Child("post")
              .Child(postId)
              .Child("Comment")
              .OnceAsync<CommentModel>()).Select(item => new CommentModel
              {
                  Username = item.Object.Username,
                  UserImage = item.Object.UserImage,
                  CommentDetail = item.Object.CommentDetail,
                  PostTime = item.Object.PostTime,
                  CommentId = item.Key
              }
            ).ToList();
        }

        public async Task<List<UserLiked>> GetCommentUserLiked(string postId, string cmtId)
        {
            return (
              await firebase
              .Child("post")
              .Child(postId)
              .Child("Comment")
              .Child(cmtId)
              .Child("UserLiked")
              .OnceAsync<UserLiked>()).Select(item => new UserLiked
              {
                  Username = item.Object.Username,
              }
            ).ToList();
        }

        public async Task AddComment(string postId, CommentModel cmt)
        {
            await firebase
              .Child("post")
              .Child(postId)
              .Child("Comment")
              .PostAsync(new CommentModel() { Username = cmt.Username, UserImage = cmt.UserImage, PostTime = cmt.PostTime, CommentDetail = cmt.CommentDetail });
        }

        public async Task AddUserLikeForComment(string postId, string cmtId, UserLiked usr)
        {
            await firebase
              .Child("post")
              .Child(postId)
              .Child("Comment")
              .Child(cmtId)
              .Child("UserLiked")
              .PostAsync(usr);
        }

        public async Task DeleteUserLikeForComment(string postId, string cmtId, string usrname)
        {
            var toDeletePerson = (await firebase
              .Child("post")
              .Child(postId)
              .Child("Comment")
              .Child(cmtId)
              .Child("UserLiked")
              .OnceAsync<UserLiked>()).Where(a => a.Object.Username == usrname).FirstOrDefault();

            await firebase
                .Child("post")
                .Child(postId)
                .Child("Comment")
                .Child(cmtId)
                .Child("UserLiked")
                .Child(toDeletePerson.Key)
                .DeleteAsync();
        }

        public async Task DeleteComment(string postId, string cmtId)
        {
            await firebase
                .Child("post")
                .Child(postId)
                .Child("Comment")
                .Child(cmtId)
                .DeleteAsync();
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            await firebaseStorage
                    .Child(fileName)
                    .PutAsync(fileStream);

            return await GetFileUrl(fileName);
        }

        public async Task<string> GetFileUrl(string fileName)
        {
            return await firebaseStorage
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        public async Task AddPost(PostModelBasic post)
        {
            await firebase
              .Child("post")
              .PostAsync(post);
        }

        public async Task<string> GetPostId(PostModelBasic model)
        {
            var toAddPost = (await firebase
              .Child("post")
              .OnceAsync<PostModelBasic>()).Where(a => a.Object.Caption == model.Caption).FirstOrDefault();

            return toAddPost.Key;
        }

        public async Task AddMediaToPost(string postId, MediaContent media)
        {
            await firebase
              .Child("post")
              .Child(postId)
              .Child("MediaList")
              .PostAsync(media);
        }
    }
}
