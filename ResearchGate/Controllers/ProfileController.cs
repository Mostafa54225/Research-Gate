using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResearchGate.Models;
using System.Data.Entity;
using System.Web.Security;
using System.IO;

namespace ResearchGate.Controllers
{
    public class ProfileController : Controller
    {

        [HttpGet]
        [Route("profile/{username?}")]
        public ActionResult Index(string username)
        {

            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                var user = (from obj in db.Authors where obj.Username.ToLower() == 
                            (username != null ? username.ToLower(): User.Identity.Name.ToLower())
                            select obj).FirstOrDefault();
                if(user != null)
                {
                    return View(user);
                }
                else
                {
                    ViewBag.ProfileNotFound = "Profile Not Found";
                    return View();
                }
                
                
            }
        }

        [HttpPost]
        [Authorize]
        [Route("Profile/{currentUser}/Edit")]
        public ActionResult Edit(Author account, string currentUser)
        {

            using (ResearchGateDBContext db = new ResearchGateDBContext())
            {
                if (User.Identity.Name == currentUser)
                {

                    if (ModelState.IsValid)
                    {
                        if (account != null)
                        {
                            var user = db.Authors.SingleOrDefault(c => c.Username == currentUser);
                            var hashedPassword = Utils.PasswordHashing.EncodePassword(account.Password, user.Salt);
                            if (hashedPassword != user.Password)
                            {
                                var salt = Utils.PasswordHashing.GeneratePassword(10);
                                var hp = Utils.PasswordHashing.EncodePassword(account.Password, salt);
                                user.Password = hp;
                                user.Salt = salt;
                            } else
                            {
                                account.Password = user.Password;
                                account.Salt = user.Salt;
                            }

                            user.FirstName = account.FirstName;
                            user.LastName = account.LastName;
                            user.Email = account.Email;
                            user.Department = account.Department;
                            user.University = account.University;
                            user.Username = account.Username;
                            user.Mobile = account.Mobile;
                            HttpPostedFileBase file = Request.Files["ProfileImage"];
                            if (file.ContentLength != 0)
                            {
                                user.Image = Utils.Helper.ConvertToBytes(file);
                            }
                            //if (ImageFile != null)
                            //{
                            //    string imageName = System.IO.Path.GetFileName(ImageFile.FileName);
                            //    string physicalPath = Server.MapPath("~/Images/" + imageName);
                            //    ImageFile.SaveAs(physicalPath);
                            //    user.ProfileImage = imageName;
                            //}
                            //user.ProfileImage = account.ProfileImage;


                            //user.ProfileImage = account.ProfileImage;
                            db.Entry(user).State = EntityState.Modified;

                            db.SaveChanges();
                            FormsAuthentication.SetAuthCookie(user.Username, false);
                            return RedirectToAction("Index");

                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error";
                        return View("Index");
                    }

                }

                return RedirectToAction("Index");
            }

        }
    }



}