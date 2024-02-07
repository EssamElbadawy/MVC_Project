using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string searchValue)
        {
            var employees = string.IsNullOrWhiteSpace(searchValue) ? await _unitOfWork.EmployeeRepository.GetAll() : _unitOfWork.EmployeeRepository.SearchEmployeesByName(searchValue);

            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);


            return View(mappedEmployees);
        }


        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid) return View(employee);
            employee.ImageName = await DocumentSettings.UploadFileAsync(employee.Image, "Images");

            var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employee);
           await _unitOfWork.EmployeeRepository.Add(mappedEmp);
            var count = await _unitOfWork.Complete();
            if (count > 0)
                TempData["Message"] = "Employee Added Successfully.";
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var employee = await _unitOfWork.EmployeeRepository.GetById(id.Value);
            if (employee is null)
                return NotFound();

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewName, mappedEmp);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
                return BadRequest();
            if (!ModelState.IsValid) return View(employee);
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employee);
                _unitOfWork.EmployeeRepository.Update(mappedEmp);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
                return BadRequest();
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employee);
                mappedEmp.Department = null;
                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                var count = await _unitOfWork.Complete();
                if (count> 0)
                    DocumentSettings.DeleteFile(mappedEmp.ImageName, "Images");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(employee);
        }
    }

}
