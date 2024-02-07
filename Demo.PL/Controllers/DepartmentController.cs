using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;


        public DepartmentController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string searchValue)
        {
            IEnumerable<Department> departments;
            if (string.IsNullOrWhiteSpace(searchValue))
                departments = await _unitOfWork.DepartmentRepository.GetAll();
            else
                departments = _unitOfWork.DepartmentRepository.SearchDepartmentByName(searchValue);
            var mappedDep = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDep);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            if (!ModelState.IsValid) return View(department);


            var mappedDep = _mapper.Map<DepartmentViewModel, Department>(department);

            await _unitOfWork.DepartmentRepository.Add(mappedDep);
            var count = await _unitOfWork.Complete();
            if (count > 0)
                TempData["Message"] = "Department Added Successfully.";
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var department = await _unitOfWork.DepartmentRepository.GetById(id.Value);
            var mappedDep =  _mapper.Map<Department, DepartmentViewModel>(department);
            if (department is null)
            {
                return NotFound();
            }

            return View(viewName, mappedDep);
        }

        public async Task<IActionResult> Edit(int? id)
        {


            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel department)
        {
            if (id != department.Id)
                return BadRequest();
            if (!ModelState.IsValid) return View(department);
            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(department);
                _unitOfWork.DepartmentRepository.Update(mappedDep);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(department);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel department)
        {
            if (id != department.Id)
                return BadRequest();
            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(department);

                _unitOfWork.DepartmentRepository.Delete(mappedDep);
               await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(department);
        }
    }
}
