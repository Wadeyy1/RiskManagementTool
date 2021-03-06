﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RiskManagementTool.Models;

namespace RiskManagementTool.Controllers
{
    public class RisksController : Controller
    {
        private RiskDAL riskDAL = new RiskDAL();

        public IActionResult Index()
        {
            List<RiskInfo> riskList = new List<RiskInfo>();
            riskList = riskDAL.GetRisks().ToList();
            return View(riskList);
        }

        public IActionResult Summary()
        {
            List<RiskSummary> riskSummary = new List<RiskSummary>();
            riskSummary = riskDAL.RiskSummary();
            return View(riskSummary);
        }

        public IActionResult Login([Bind] UserLogin login)
        {
            if (login.Username == null)
            {
                return View();
            }
            else
            {
                int Result = riskDAL.LoginCheck(login);
                if (Result == 1)
                {
                    login.Valid = 1;

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Name = "Username or Password incorrect. ";
                    login.Valid = 0;
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] RiskInfo objris)
        {
            if (ModelState.IsValid)
            {
                riskDAL.AddRisk(objris);
                return RedirectToAction("Index");
            }
            return View(objris);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RiskInfo ris = riskDAL.GetRiskById(id);
            if (ris == null)
            {
                return NotFound();
            }
            return View(ris);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, [Bind] RiskInfo objris)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                riskDAL.UpdateRisk(objris);
                return RedirectToAction("Index");
            }
            return View(riskDAL);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RiskInfo ris = riskDAL.GetRiskById(id);
            if (ris == null)
            {
                return NotFound();
            }
            return View(ris);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RiskInfo ris = riskDAL.GetRiskById(id);
            if (ris == null)
            {
                return NotFound();
            }
            return View(ris);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRisk(int? id)
        {
            riskDAL.DeleteRisk(id);
            return RedirectToAction("Index");
        }
    }
}