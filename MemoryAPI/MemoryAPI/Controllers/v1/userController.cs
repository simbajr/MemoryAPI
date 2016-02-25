using MemoryAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Amazon.S3;
using Amazon.S3.Model;

namespace MemoryAPI.Controllers.v1
{
    [Authorize]
    public class userController : ApiController
    {
        // /api/v1/user/<username>/media-objects
        // GET: Returns a list of media objects for the user. 
        /*public List<Media> Get(String username, String modifier)
        {
            if (modifier.Equals("media-objects"))
            {
                using (var db = new MemoryDB())
                {
                    var dbUser = (from us in db.User where us.username == username select us).SingleOrDefault();
                    if (dbUser == null)
                    {
                        var responseMsg = new HttpResponseMessage { Content = new StringContent("There was no user found.") };
                        throw new HttpResponseException(responseMsg);
                    }

                    var dbMedia = (from me in db.Media where me.user.id == dbUser.id select me).ToList();
                    if (dbMedia == null)
                    {
                        var responseMsg = new HttpResponseMessage { Content = new StringContent("There were no media files found.") };
                        throw new HttpResponseException(responseMsg);
                    }
                    else
                    {
                        return dbMedia;
                    }
                }
            }
            else
            {
                var responseMsg = new HttpResponseMessage { Content = new StringContent("The modifier was not correct.") };
                throw new HttpResponseException(responseMsg);
            }
        }*/


        // /api/v1/user/<username>/<modifier> (sounds, videos, pictures)
        // POST: Create new media file. In body must be either "sound-file", "video-file" or "picture-file".
        public IHttpActionResult Post(String id, String modifier,[FromBody]String id2)
        {
            string username = id;
            string friendsname = id2;
            string bucketName = "memorybucket";
            string awsAccessKeyId = "";
            string awsSecretAccessKey = "";
            string URLforFile = "http://memoryapi-dev.elasticbeanstalk.com/";
            var DB = new MemoryDB();
            User userObj = DB.User.FirstOrDefault(x => x.username == username);

            if(modifier.Equals("pictures"))
            {
                var uploadFile = HttpContext.Current.Request.Files["picture-file"];
                if ((uploadFile != null) && (uploadFile.ContentLength > 0))
                {
                    byte[] dataArr = new byte[uploadFile.ContentLength];
                    uploadFile.InputStream.Read(dataArr, 0, uploadFile.ContentLength);
                    Stream stream = new MemoryStream(dataArr);
                    var fileName = Convert.ToString(DateTime.Now.ToFileTime());
                    string file = uploadFile.FileName;
                    string fileType = file.Substring(file.IndexOf('.') + 1);

                    var pictureObj = new PictureMedia
                    {
                        fileUrl = URLforFile + fileName + "." + fileType,
                        container = uploadFile.ContentType,
                        width = Convert.ToInt32(HttpContext.Current.Request.QueryString["width"]),
                        height = Convert.ToInt32(HttpContext.Current.Request.QueryString["height"])
                    };

                    try
                    {
                        using (var ac = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, Amazon.RegionEndpoint.EUCentral1))
                        {
                            ac.PutObject(new PutObjectRequest()
                            {
                                InputStream = stream,
                                BucketName = bucketName,
                                Key = fileName + "." + fileType
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        var errorResponseMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                        throw new HttpResponseException(errorResponseMsg);
                    }           
                    using (var db = new MemoryDB())
                    {
                        try
                        {
                            userObj.mediaList.Add(pictureObj);
                            db.SaveChanges();
                            return Ok("The picture file has been uploaded.");
                        }
                        catch (Exception e)
                        {
                            var errorResponseMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                            throw new HttpResponseException(errorResponseMsg);
                        }
                    }
                }
            }
            
            if (modifier.Equals("sounds"))
            {
                var uploadFile = HttpContext.Current.Request.Files["sound-file"];
                if ((uploadFile != null) && (uploadFile.ContentLength > 0))
                {
                    byte[] dataArr = new byte[uploadFile.ContentLength];
                    uploadFile.InputStream.Read(dataArr, 0, uploadFile.ContentLength);
                    Stream stream = new MemoryStream(dataArr);
                    var fileName = Convert.ToString(DateTime.Now.ToFileTime());
                    string file = uploadFile.FileName;
                    string fileType = file.Substring(file.IndexOf('.') + 1);

                    var soundObj = new SoundMedia
                    {
                        fileUrl = URLforFile + fileName + "." + fileType,
                        container = uploadFile.ContentType,
                        duration = Convert.ToInt32(HttpContext.Current.Request.QueryString["duration"]),
                        codec = Convert.ToString(HttpContext.Current.Request.QueryString["codec"]),
                        bitRate = Convert.ToInt32(HttpContext.Current.Request.QueryString["bitrate"]),
                        channels = Convert.ToInt32(HttpContext.Current.Request.QueryString["channels"]),
                        samplingRate = Convert.ToInt32(HttpContext.Current.Request.QueryString["samplingrate"])
                    };
                    try
                    {
                        using (var ac = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, Amazon.RegionEndpoint.EUCentral1))
                        {
                            ac.PutObject(new PutObjectRequest()
                            {
                                InputStream = stream,
                                BucketName = bucketName,
                                Key = fileName + "." + fileType
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        var errorResponseMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                        throw new HttpResponseException(errorResponseMsg);
                    }
                    using (var db = new MemoryDB())
                    {
                        try
                        {
                            userObj.mediaList.Add(soundObj);
                            db.SaveChanges();
                            return Ok("The sound file has been uploaded.");
                        }
                        catch (Exception e)
                        {
                            var errorResponseMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                            throw new HttpResponseException(errorResponseMsg);
                        }
                    }
                }
            }
            
            if (modifier.Equals("videos"))
            {
                var uploadFile = HttpContext.Current.Request.Files["video-file"];
                if ((uploadFile != null) && (uploadFile.ContentLength > 0))
                {
                    byte[] dataArr = new byte[uploadFile.ContentLength];
                    uploadFile.InputStream.Read(dataArr, 0, uploadFile.ContentLength);
                    Stream stream = new MemoryStream(dataArr);
                    var fileName = Convert.ToString(DateTime.Now.ToFileTime());
                    string file = uploadFile.FileName;
                    string fileType = file.Substring(file.IndexOf('.'), +1);

                    var videoObj = new VideoMedia
                    {
                        fileUrl = URLforFile + fileName + "." + fileType,
                        container = uploadFile.ContentType,
                        width = Convert.ToInt32(HttpContext.Current.Request.QueryString["width"]),
                        height = Convert.ToInt32(HttpContext.Current.Request.QueryString["height"]),
                        videoCodec = Convert.ToString(HttpContext.Current.Request.QueryString["videocodec"]),
                        videoBitRate = Convert.ToInt32(HttpContext.Current.Request.QueryString["videobitrate"]),
                        frameRate = Convert.ToInt32(HttpContext.Current.Request.QueryString["framerate"]),
                        audioCodec = Convert.ToString(HttpContext.Current.Request.QueryString["audiocodec"]),
                        audioBitRate = Convert.ToInt32(HttpContext.Current.Request.QueryString["audiobitrate"]),
                        samplingRate = Convert.ToInt32(HttpContext.Current.Request.QueryString["samplingRate"])
                    };
                    try
                    {
                        using (var ac = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, Amazon.RegionEndpoint.EUCentral1))
                        {
                            ac.PutObject(new PutObjectRequest()
                            {
                                InputStream = stream,
                                BucketName = bucketName,
                                Key = fileName + "." + fileType
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        var errorResponseMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                        throw new HttpResponseException(errorResponseMsg);
                    }
                    using (var db = new MemoryDB())
                    {
                        try
                        {
                            userObj.mediaList.Add(videoObj);
                            db.SaveChanges();
                            return Ok("The video file has been uploaded.");
                        }
                        catch (Exception e)
                        {
                            var errorResponseMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                            throw new HttpResponseException(errorResponseMsg);
                        }
                    }
                }
                if (modifier.Equals("friends"))
                {
                    using (var db = new MemoryDB())
                    {
                        var dbUser = (from us in db.User.Include("friendList") where us.username == username select us).SingleOrDefault();
                        var dbFriend = (from us in db.User where us.username == friendsname select us).SingleOrDefault();

                        if ((dbUser != null) && (dbFriend != null))
                        {
                            try
                            {
                                dbUser.friendList.Add(dbFriend);
                                db.SaveChanges();
                                return Ok("Friend was successfully added.");
                            }
                            catch (Exception e)
                            {
                                var responseErrorMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                                throw new HttpResponseException(responseErrorMsg);
                            }
                        }
                        else
                        {
                            var responseMsg = new HttpResponseMessage { Content = new StringContent("User not found or friend not found.") };
                            throw new HttpResponseException(responseMsg);
                        }
                    }
                }
                else
                {
                    var responseMsg = new HttpResponseMessage { Content = new StringContent("You did not use the correct modifier.") };
                    throw new HttpResponseException(responseMsg);
                }
            }

            var lastErrorMsg = new HttpResponseMessage { Content = new StringContent(string.Format("The modifier was wrong, please state the correct one!")) };
            throw new HttpResponseException(lastErrorMsg);
        }


        // /api/v1/user/<username>
        // GET: Returns the user with username <username>.
        public User Get(String username)
        {
            using (var db = new MemoryDB())
            {
                var dbUser = (from us in db.User where us.username == username select us).FirstOrDefault();
                if (dbUser == null)
                {
                    var responseMsg = new HttpResponseMessage { Content = new StringContent("There was no user found.") };
                    throw new HttpResponseException(responseMsg);
                }
                else
                {
                    return dbUser;
                }
            }
        }

        // DELETE: Deletes everything related to the user with username <username>.
        public IHttpActionResult Delete(String username)
        {
            using (var db = new MemoryDB())
            {
                var dbUser = (from us in db.User.Include("mediaList") where us.username == username select us).FirstOrDefault();
                if (dbUser != null)
                {
                    try
                    {
                        foreach (var media in dbUser.mediaList)
                    {
                        dbUser.mediaList.Remove(media);
                    }

                    db.User.Remove(dbUser);
                    db.SaveChanges();
                    return Ok("User has been successfully deleted.");
                    }
                    catch (Exception e)
                    {
                        var responseErrorMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
                        throw new HttpResponseException(responseErrorMsg);
                    }
                }
                else
                {
                    var responseMsg = new HttpResponseMessage { Content = new StringContent("The user was not found.") };
                    throw new HttpResponseException(responseMsg);
                }
            }
        }


        // /api/v1/user/<username>/<modifier> (friends, media, media-object)
        // GET (friends): Returns a list of friends the user with username <username> has.
        // GET (media): Returns a list of media the user with username <username> has.
        public object Get(String id, String modifier)
        {
            string username = id;

            if (modifier.Equals("friends"))
            {
                using (var db = new MemoryDB())
                {
                    var dbUser = (from us in db.User.Include("friendList") where us.username == username select us).SingleOrDefault();
                    if (dbUser != null)
                    {
                        return dbUser.friendList.ToList();
                    }
                    else
                    {
                        var responseMsg = new HttpResponseMessage { Content = new StringContent("The user was not found.") };
                        throw new HttpResponseException(responseMsg);
                    }
                }
            }
            else if (modifier.Equals("media"))
            {
                using (var db = new MemoryDB())
                {
                    var dbUser = (from us in db.User.Include("mediaList") where us.username == username select us).SingleOrDefault();
                    if (dbUser != null)
                    {
                        return dbUser.mediaList.ToList();
                    }
                    else
                    {
                        var responseMsg = new HttpResponseMessage { Content = new StringContent("The user was not found.") };
                        throw new HttpResponseException(responseMsg);
                    }
                }
            }
            else
            {
                var responseMsg = new HttpResponseMessage { Content = new StringContent("You did not use the correct modifier.") };
                throw new HttpResponseException(responseMsg);
            }
        }

        // POST: Adds a user as a friend to the user with username <username>.
        //public IHttpActionResult Post(String id, String modifier, [FromBody]String friendsname)
        //{
            
        //    string username = id;

        //    if (modifier.Equals("friends"))
        //    {
        //        using (var db = new MemoryDB())
        //        {
        //            var dbUser = (from us in db.User.Include("friendList") where us.username == username select us).SingleOrDefault();
        //            var dbFriend = (from us in db.User where us.username == friendsname select us).SingleOrDefault();

        //            if ((dbUser != null) && (dbFriend != null))
        //            {
        //                try
        //                {
        //                    dbUser.friendList.Add(dbFriend);
        //                    db.SaveChanges();
        //                    return Ok("Friend was successfully added.");
        //                }
        //                catch (Exception e)
        //                {  
        //                    var responseErrorMsg = new HttpResponseMessage { Content = new StringContent(string.Format("This is wrong: {0}", e)) };
        //                    throw new HttpResponseException(responseErrorMsg);
        //                }
        //            }
        //            else
        //            {
        //                var responseMsg = new HttpResponseMessage { Content = new StringContent("User not found or friend not found.") };
        //                throw new HttpResponseException(responseMsg);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var responseMsg = new HttpResponseMessage { Content = new StringContent("You did not use the correct modifier.") };
        //        throw new HttpResponseException(responseMsg);
        //    }
        //}


        // /api/v1/user/<username>/media
        // GET: Returns a list of media the user with username <username> has.
        /*public List<Media> Get(String username, String modifier)
        {
            if (modifier.Equals("media"))
            {
                using (var db = new MemoryDB())
                {
                    var dbUser = (from us in db.User.Include("mediaList") where us.username == username select us).SingleOrDefault();
                    if (dbUser != null)
                    {
                        return dbUser.mediaList.ToList();
                    }
                    else
                    {
                        var responseMsg = new HttpResponseMessage { Content = new StringContent("The user was not found.") };
                        throw new HttpResponseException(responseMsg);
                    }
                }
            }
            else
            {
                var responseMsg = new HttpResponseMessage { Content = new StringContent("You did not use the correct modifier.") };
                throw new HttpResponseException(responseMsg);
            }
        }*/


        // /api/v1/user/<username>/friends/<friendsUsername>
        // DELETE: Removes the user with username <friendsUsername> as a friend to the user with username <username>.
        public IHttpActionResult Delete(String id, String modifier, String id2)
        {
            string username = id;
            string friendsUsername = id2;
            if (modifier.Equals("friends"))
            {
                using (var db = new MemoryDB())
                {
                    var dbUser = (from us in db.User.Include("friendList") where us.username == username select us).SingleOrDefault();
                    var dbFriend = (from us in db.User where us.username == friendsUsername select us).SingleOrDefault();

                    if ((dbUser != null) && (dbFriend != null))
                    {
                        dbUser.friendList.Remove(dbFriend);
                        db.SaveChanges();
                        return Ok("Friend has been successfully removed.");
                    }
                    else
                    {
                        var responseMsg = new HttpResponseMessage { Content = new StringContent("User not found or friend not found.") };
                        throw new HttpResponseException(responseMsg);
                    }
                }
            }
            else
            {
                var responseMsg = new HttpResponseMessage { Content = new StringContent("The modifier was incorrect.") };
                throw new HttpResponseException(responseMsg);
            }
        }


    }
}
