using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VueLogin.Data;
using VueLogin.Models;

namespace VueLogin.Controllers
{
    public class UserListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserList.ToListAsync());
        }

        // GET: UserLists/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userList = await _context.UserList
                .FirstOrDefaultAsync(m => m.Email == id);
            if (userList == null)
            {
                return NotFound();
            }

            return View(userList);
        }

        // GET: UserLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password")] UserList userList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userList);
        }

        // GET: UserLists/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userList = await _context.UserList.FindAsync(id);
            if (userList == null)
            {
                return NotFound();
            }
            return View(userList);
        }

        // POST: UserLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Email,Password")] UserList userList)
        {
            if (id != userList.Email)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserListExists(userList.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userList);
        }

        // GET: UserLists/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userList = await _context.UserList
                .FirstOrDefaultAsync(m => m.Email == id);
            if (userList == null)
            {
                return NotFound();
            }

            return View(userList);
        }

        // POST: UserLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userList = await _context.UserList.FindAsync(id);
            _context.UserList.Remove(userList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserListExists(string id)
        {
            return _context.UserList.Any(e => e.Email == id);
        }
    }
}
