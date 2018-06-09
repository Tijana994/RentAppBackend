using RentApp.Models.Entities;
using RentApp.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RentApp.Controllers
{
    [RoutePrefix("api/Upload")]
    public class UploadController : ApiController
    {
        private IUnitOfWork db;

        public UploadController(IUnitOfWork db)
        {
            this.db = db;
        }
        [HttpPost]
        [Route("PostUserImage/{id}")]
        [Authorize]
        public IHttpActionResult PostUserImage(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                if (!db.AppUsers.Any(x => x.Id == id))
                {
                    return BadRequest("Id is wrong");
                }

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    string filePath = "";
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return BadRequest("Please Upload image of type .jpg,.gif,.png.");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 2 mb.");

                            dict.Add("error", message);
                            return BadRequest("Please Upload a file upto 2 mb");
                        }
                        else
                        {

                            filePath = HttpContext.Current.Server.MapPath("~/Images/ImageUsers/" + db.AppUsers.Get(id).Username + "_" +postedFile.FileName);

                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    try
                    {
                        db.AppUsers.Get(id).Path = filePath;
                        db.SaveChanges();
                    }
                    catch(Exception ex) {

                        return BadRequest("Somebody already change picture");
                    }

                    return Ok(filePath);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return BadRequest();
            }
            catch (Exception ex)
            {
                var res = string.Format("error");
                dict.Add("error", res);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("car/PostCarImage/{id}")]
        [Authorize]
        public IHttpActionResult PostCarImage(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            string filePath = "";

            try
            {
                if (!db.Vehicles.Any(x => x.Id == id))
                {
                    return BadRequest("Id is wrong");
                }

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return BadRequest("Please Upload image of type .jpg,.gif,.png.");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 2 mb.");

                            dict.Add("error", message);
                            return BadRequest("Please Upload a file upto 2 mb");
                        }
                        else
                        {
                            var car = db.Vehicles.Get(id);
                            var service = db.Services.Get(car.ServiceId);

                            filePath = HttpContext.Current.Server.MapPath("~/Images/ImageCars/" + service.Name + "_" + car.Id + "_" + postedFile.FileName);

                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully."); 

                    try
                    {
                        Pic pic = new Models.Entities.Pic();
                        pic.Path = filePath;
                        pic.VehicleId = id;
                        db.Vehicles.Get(id).Pics.Add(pic);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest("Somebody already change picture");
                    }

                    return Ok(filePath);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return BadRequest();
            }
            catch (Exception ex)
            {
                var res = string.Format("error");
                dict.Add("error", res);
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("PostServiceImage/{id}")]
        [Authorize]
        public IHttpActionResult PostServiceImage(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            string filePath = "";
            
            try
            {
                if (!db.Services.Any(x => x.Id == id))
                {
                    return BadRequest("Id is wrong");
                }

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return BadRequest("Please Upload image of type .jpg,.gif,.png.");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 2 mb.");

                            dict.Add("error", message);
                            return BadRequest("Please Upload a file upto 2 mb");
                        }
                        else
                        {



                            filePath = HttpContext.Current.Server.MapPath("~/Images/ImageServices/" + db.Services.Get(id).Name + "_" + postedFile.FileName);

                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    try
                    {
                        db.Services.Get(id).Path = filePath;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest("Somebody already change picture");
                    }

                    return Ok(filePath);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return BadRequest();
            }
            catch (Exception ex)
            {
                var res = string.Format("error");
                dict.Add("error", res);
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("PostBranchImage/{id}")]
        [Authorize]
        public IHttpActionResult PostBranchImage(int id)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            string filePath = "";

            try
            {
                if (!db.Branches.Any(x => x.Id == id))
                {
                    return BadRequest("Id is wrong");
                }

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return BadRequest("Please Upload image of type .jpg,.gif,.png.");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 2 mb.");

                            dict.Add("error", message);
                            return BadRequest("Please Upload a file upto 2 mb");
                        }
                        else
                        {
                            var branch = db.Branches.Get(id);
                            var service = db.Services.Get(branch.ServiceId);

                            filePath = HttpContext.Current.Server.MapPath("~/Images/ImageBranches/"  + service.Name +"_" + branch.Name + "_" + postedFile.FileName);

                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    try
                    {
                        db.Branches.Get(id).Path = filePath;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                        return BadRequest("Somebody already change picture");
                    }

                    return Ok(filePath);
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return BadRequest();
            }
            catch (Exception ex)
            {
                var res = string.Format("error");
                dict.Add("error", res);
                return BadRequest();
            }
        }
    }
}
