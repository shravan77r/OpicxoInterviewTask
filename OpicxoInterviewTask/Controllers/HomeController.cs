using OpicxoInterviewTask.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OpicxoInterviewTask.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        Sql db = new Sql();
        public ActionResult List()
        {
            return View();
        }
        public JsonResult ListGetData()
        {
            List<Person> list = new List<Person>();
            try
            {
                DataTable dt = db.Getdata("List_Person");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var GenderName = "Female";
                        if(Convert.ToInt32(dt.Rows[i]["Gender"].ToString())==1)
                            GenderName = "Male";

                        list.Add(new Person
                        {
                            SrNo = (i+1),
                            Id = Convert.ToInt32(dt.Rows[i]["Id"]),
                            PersonName = dt.Rows[i]["PersonName"].ToString(),
                            PersonHeight = Convert.ToDecimal(dt.Rows[i]["Height"].ToString()),
                            PersonWeight = Convert.ToDecimal(dt.Rows[i]["Weight"].ToString()),
                            Gender = Convert.ToInt32(dt.Rows[i]["Gender"].ToString()),
                            GenderName = GenderName,
                            BMI = Convert.ToDecimal(dt.Rows[i]["BMI"].ToString()),                            
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int? Id)
        {
            var obj = new Person();
            obj.Gender = 1;
            if (Id > 0)
            {
                DataTable dt = db.Getdata("Select * FROM Tbl_Person WHERE Id = "+ Id);
                if (dt.Rows.Count > 0)
                {
                    obj.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    obj.PersonName = dt.Rows[0]["PersonName"].ToString();
                    obj.PersonHeight = Convert.ToDecimal(dt.Rows[0]["Height"].ToString());
                    obj.PersonWeight = Convert.ToDecimal(dt.Rows[0]["Weight"].ToString());
                    obj.Gender = Convert.ToInt32(dt.Rows[0]["Gender"].ToString());
                }
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Index(Person obj)
        {
            try
            {
                if (obj.Id > 0)
                    obj.ACTION = "UPDATE";
                else
                    obj.ACTION = "INSERT";

                db.INSERT_UPDATE_DELETE(obj);
            }
            catch (Exception e)
            {
            }
            return RedirectToAction("List", "Home");
        }
        [HttpPost]
        public JsonResult DeletePersonRecord(Person obj)
        {
            string res = string.Empty;
            try
            {
                obj.ACTION = "DELETE";
                db.INSERT_UPDATE_DELETE(obj);
                res = "Deleted";
            }
            catch (Exception)
            {
                res = "FAILED";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public PartialViewResult GetActivityList(int PersonId, int IsAll)
        {
            List<Activities> list = new List<Activities>();
            try
            {
                DataTable dt = db.GetListViewActivities(PersonId, IsAll);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(new Activities
                        {
                            Id = Convert.ToInt32(dt.Rows[i]["Id"]),
                            PersonId = Convert.ToInt32(dt.Rows[i]["PersonId"]),
                            ActivityDate = dt.Rows[i]["ActivityDate"].ToString(),
                            WakeUpTime = dt.Rows[i]["WakeUpTime"].ToString(),
                            IsGym = Convert.ToInt32(dt.Rows[i]["IsGym"]),
                            IsMeditation = Convert.ToInt32(dt.Rows[i]["IsMeditation"]),
                            MeditationMinutes = dt.Rows[i]["MeditationMinutes"].ToString(),
                            IsRead = Convert.ToInt32(dt.Rows[i]["IsRead"]),
                            ReadPages = dt.Rows[i]["ReadPages"].ToString(),
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
            return PartialView("_ActivityListPartial", list);
        }
        [HttpPost]
        public JsonResult SaveActivity(Activities Activities)
        {
            string res = string.Empty;
            try
            {
                if (Activities.Id > 0)
                    Activities.ACTION = "UPDATE";
                else
                    Activities.ACTION = "INSERT";
                db.INSERT_UPDATE_DELETE_Activities(Activities);
                res = "Saved";
            }
            catch (Exception)
            {
                res = "FAILED";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetActivityDataById(int Id)
        {
            Activities obj = new Activities();
            if (Id > 0)
            {
                DataTable dt = db.Getdata("GetActivityDataById " + Id);
                if (dt.Rows.Count > 0)
                {
                    obj.PersonId = Convert.ToInt32(dt.Rows[0]["PersonId"]);
                    obj.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    obj.PersonName = dt.Rows[0]["PersonName"].ToString();
                    obj.PersonHeight = Convert.ToDecimal(dt.Rows[0]["Height"].ToString());
                    obj.PersonWeight = Convert.ToDecimal(dt.Rows[0]["Weight"].ToString());
                    obj.ActivityDate = Convert.ToString(dt.Rows[0]["ActivityDate"].ToString());
                    obj.WakeUpTime = Convert.ToString(dt.Rows[0]["WakeUpTime"].ToString());
                    obj.IsGym = Convert.ToInt32(dt.Rows[0]["IsGym"]);
                    obj.IsMeditation = Convert.ToInt32(dt.Rows[0]["IsMeditation"]);
                    obj.MeditationMinutes = Convert.ToString(dt.Rows[0]["MeditationMinutes"].ToString());
                    obj.IsRead = Convert.ToInt32(dt.Rows[0]["IsRead"]);
                    obj.ReadPages = Convert.ToString(dt.Rows[0]["ReadPages"].ToString());
                }
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePersonActivities(int Id)
        {
            string res = string.Empty;
            try
            {
                var Activities = new Activities();
                Activities.Id = Id;
                Activities.ACTION = "DELETE";
                db.INSERT_UPDATE_DELETE_Activities(Activities);
                res = "Deleted";
            }
            catch (Exception)
            {
                res = "FAILED";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
       
    }
}
