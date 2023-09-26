using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Repository.IRepository;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Models;
using System.Data;
using System.Net;
using System.Security.Claims;
using SchoolManagementSystem.Repository;
using Azure;
using System;

namespace SchoolManagementSystem.Controllers
{
    [ApiController]
    public class ModuleAPIController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public ModuleAPIController(IModuleRepository moduleRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/MenuAPIController/GetMenus")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetMenus()
        {


            try
            {
                IEnumerable<Module> module = await _moduleRepository.GetAllAsync();
                IEnumerable<ModuleDTO> modulesDTO = _mapper.Map<List<ModuleDTO>>(module);
                foreach (var menus in modulesDTO)
                {
                    if (menus.ParentId!=0)
                    {
                        var response = await _moduleRepository.GetAsync(u => u.ModuleId == menus.ParentId);

                        menus.ParentName = response.Menus;

                    }
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = (modulesDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/MenuAPIController/GetMenu")]
        [Authorize(Roles = "Admin")]

        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetMenu([FromBody] int ModuleId)
        {
            if (ModuleId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                Module module = await _moduleRepository.GetAsync(u => (u.ModuleId == ModuleId && u.StatusFlag == false));


                if (module == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<Module>(module);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]

        [Authorize(Roles = "Admin")]
        [Route("api/MenuAPIController/Create")]

        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] ModuleDTO moduleDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if (await _moduleRepository.GetAsync(u => u.Menus.ToLower() == moduleDTO.Menus.ToLower()) != null)

                {
                    ModelState.AddModelError("ErrorMessages", "Role Already Exists");
                    return BadRequest(ModelState);
                }
                if (moduleDTO == null)
                {
                    return BadRequest(moduleDTO);


                }

                Module module = _mapper.Map<Module>(moduleDTO);



                await _moduleRepository.CreateAsync(module, _loginUserid);

                _response.Result = _mapper.Map<ModuleDTO>(module);
                _response.StatusCode = HttpStatusCode.Created;

                return Ok(moduleDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }
            return _response;
        }




        [HttpDelete]
        [Authorize(Roles = "Admin")]

        [Route("api/MenuAPIController/DeleteMenu")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete( [FromBody] int ModuleId)
        {
            if (ModuleId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (ModuleId == 0)
                {
                    return BadRequest();
                }


                var Module = await _moduleRepository.GetAsync(u => u.ModuleId == ModuleId);
                if (Module == null)
                {
                    return NotFound();
                }

                Module.StatusFlag = true;
                await _moduleRepository.UpdateAsync(Module, _loginUserid);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }
            return _response;
        }



        [HttpPut]
        [Authorize(Roles = "Admin")]

        [Route("api/MenuAPIController/UpdateMenu")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] Module module)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_moduleRepository.IsUniqueName(module.Menus, module.ModuleId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("User Email already exists");
                return BadRequest(_response);

            }
            try
            {
                if (module == null)
                {
                    return BadRequest();

                }

                Module model = _mapper.Map<Module>(module);

                await _moduleRepository.UpdateAsync(model, _loginUserid);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]

        [Route("api/MenuAPIController/EnableMenu")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> EnableMenu( [FromBody]int ModuleId)
        {
            try
            {
                if (ModuleId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var ModuleDTO = await _moduleRepository.GetAsync(u => u.ModuleId == ModuleId && u.StatusFlag == true);

                if (ModuleDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                Module module = _mapper.Map<Module>(ModuleDTO);



                module.StatusFlag = false;
                await _moduleRepository.UpdateAsync(module, _loginUserid);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
