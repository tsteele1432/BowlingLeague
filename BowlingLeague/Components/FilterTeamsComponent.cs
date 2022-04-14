using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BowlingLeague.Models;

namespace BowlingLeague.Components
{
    public class FilterTeamsComponent : ViewComponent
    {
        private BowlingLeagueDbContext _context { get; set; }

        public FilterTeamsComponent(BowlingLeagueDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Teams = RouteData?.Values["teamName"] ?? "";

            var teams = _context.Bowlers.Select(bowler => bowler.Team.TeamName).Distinct().OrderBy(team => team);

            return View(teams);
        }
    }
}
