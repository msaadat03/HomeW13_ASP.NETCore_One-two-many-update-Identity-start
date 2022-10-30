﻿using EntityFrameworkProject.Data;
using EntityFrameworkProject.Helpers;
using EntityFrameworkProject.Models;
using EntityFrameworkProject.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkProject.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 2)
        {
            IEnumerable<Employee> employees = await _context.Employees
                .Where(m=> !m.IsDeleted)
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .Skip((page * take) - take)
                .Take(take)
                .ToListAsync();

            List<EmployeeListVM> mapDatas = GetMapDatas(employees);

            int count = await GetPageCount(take);

            Paginate<EmployeeListVM> result = new Paginate<EmployeeListVM>(mapDatas, page, count);

            return View(result);
        }

        private async Task<int> GetPageCount(int take)
        {
            int employeeCount = await _context.Employees.Where(m => !m.IsDeleted).CountAsync();

            return (int)Math.Ceiling((decimal)employeeCount / take);
        }

        private List<EmployeeListVM> GetMapDatas(IEnumerable<Employee> employees)
        {
            List<EmployeeListVM> employeeList = new List<EmployeeListVM>();

            foreach (var employee in employees)
            {
                EmployeeListVM newEmployee = new EmployeeListVM
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    Age = employee.Age,
                    Position = employee.Position,
                    IsActive = employee.IsActive
                };

                employeeList.Add(newEmployee);
            }

            return employeeList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetStatus(int id)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            if (employee is null) return NotFound();

            if (employee.IsActive)
            {
                employee.IsActive = false;
            }
            else
            {
                employee.IsActive = true;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _context.Employees.AddAsync(employee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();

            Employee employee = await _context.Employees.FindAsync(id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id is null) return BadRequest();

                Employee employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

                if (employee is null) return NotFound();

                return View(employee);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(employee);
                }

                Employee dbEmployee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (dbEmployee is null) return NotFound();

                if (dbEmployee.FullName.ToLower().Trim() == employee.FullName.ToLower().Trim()
                    && dbEmployee.Age == employee.Age
                    && dbEmployee.Position.ToLower().Trim() == employee.Position.ToLower().Trim())
                {
                    return RedirectToAction(nameof(Index));
                }

                //dbCategory.Name = category.Name;

                _context.Employees.Update(employee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            employee.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
