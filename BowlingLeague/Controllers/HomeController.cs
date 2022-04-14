using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BowlingLeague.Models;

namespace BowlingLeague.Controllers
{
    public class HomeController : Controller
    {
        private BowlingLeagueDbContext _context { get; set; }

        public HomeController(BowlingLeagueDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string teamName)
        {
            ViewBag.TeamName = teamName ?? "Home";

            var team = _context.Bowlers
                .Include(team => team.Team)
                .Where(team => team.Team.TeamName == teamName || teamName == null)
                .ToList();

            return View(team);
        }

        [HttpGet]
        public IActionResult CreateEditBowler()
        {
            ViewBag.Teams = _context.Teams.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CreateEditBowler(Bowler bowler)
        {
            if (ModelState.IsValid)
            {
                bowler.BowlerID = _context.Bowlers.Count() + 1;
                _context.Add(bowler);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(bowler);
        }

        [HttpGet]
        public IActionResult Edit(int bowlerId)
        {
            ViewBag.Added = false;
            ViewBag.Teams = _context.Teams.ToList();

            var bowler = _context.Bowlers.Single(bowler => bowler.BowlerID == bowlerId);

            return View("CreateEditBowler", bowler);
        }

        [HttpPost]
        public IActionResult Edit(Bowler bowler)
        {
            if (ModelState.IsValid)
            {
                _context.Update(bowler);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(bowler);
        }

        public IActionResult Delete(int bowlerId)
        {
            var bowler = _context.Bowlers.Single(bowler => bowler.BowlerID == bowlerId);

            _context.Bowlers.Remove(bowler);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
