﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add()
        {
            var entry = new Entry()
            {
                Date = DateTime.Today
            };

            ViewBag.ActivitiesSelectListItems = new SelectList(Data.Data.Activities, "Id", "Name");

            return View(entry);
        }

        [HttpPost]
        public ActionResult Add(Entry entry)
        {
            //Left in as an example of a Global Error Message
            //ModelState.AddModelError("", "This is a global message");

            //If there aren't any Duration field validation errors
            //Then make sure that the Duration is greater than 0
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "The Duration field value must be greater than '0'");
            }

            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);

                return RedirectToAction("Index");
            }

            ViewBag.ActivitiesSelectListItems = new SelectList(Data.Data.Activities, "Id", "Name");
            
            return View(entry);
        }
        
        //[HttpPost]
        //public ActionResult Add(DateTime? date, int? activityId, double? duration, Entry.IntensityLevel? intensity, bool? exclude, string notes)
        //{
        //    //ViewBag.Date = ModelState["date"].Value.AttemptedValue;
        //    //ViewBag.ActivityId = ModelState["activityId"].Value.AttemptedValue;
        //    //ViewBag.Duration = ModelState["duration"].Value.AttemptedValue;
        //    //ViewBag.Intensity = ModelState["intensity"].Value.AttemptedValue;
        //    //ViewBag.Exclude = ModelState["exclude"].Value.AttemptedValue;
        //    //ViewBag.Notes = ModelState["notes"].Value.AttemptedValue;

        //    return View();
        //}

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }
    }
}