using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.Extensions.Options;

namespace Library.Controllers
{
    [Authorize (Roles = "SysAdmin")]
    public class ParametersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<MailKitOptions> _mailOptions;

        public ParametersController(ApplicationDbContext context, IOptions<MailKitOptions> mailOptions)
        {
            _context = context;
            _mailOptions = mailOptions;
        }

        // GET: Parameters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parameters.ToListAsync());
        }

        // GET: Parameters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parameter = await _context.Parameters.FindAsync(id);
            if (parameter == null)
            {
                return NotFound();
            }
            return View(parameter);
        }

        // POST: Parameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Value,Description,MemberId,LastChanged,Obsolete")] Parameter parameter)
        {
            if (id != parameter.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    parameter.LastChanged = DateTime.Now;
                    _context.Update(parameter);
                    await _context.SaveChangesAsync();
                    Parameter.Parameters[parameter.Name] = parameter;
                    Parameter.ConfigureMail(_mailOptions.Value);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParameterExists(parameter.Name))
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
            return View(parameter);
        }

        private bool ParameterExists(string id)
        {
            return _context.Parameters.Any(e => e.Name == id);
        }
    }
}
