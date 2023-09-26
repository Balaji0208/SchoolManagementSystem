using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository;
using SchoolManagementSystem.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    public class ModuleRoleMappingController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IModuleRoleMappingRepository _modelRoleRepository;
        private readonly IModuleRepository _moduleRepository;
        protected APIResponse _response;
        private readonly int _loginUserid;
        
        private readonly IMapper _mapper;
        public ModuleRoleMappingController(IModuleRoleMappingRepository modelRoleRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUserRepository userRepository,IModuleRepository moduleRepository)
        {
            _modelRoleRepository = modelRoleRepository;
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _response = new();
            _mapper = mapper;
         
            _userRepository = userRepository;
            _moduleRepository = moduleRepository;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Register")]
        [Route("api/ModuleRoleMappingController/GetAllModuleRoles")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllModuleRoles()
        {


            try
            {
                IEnumerable<ModuleRoleMapping> moduleRoleDTO = await _modelRoleRepository.GetAllUserAsync( includeProperties: "RoleDetails,Module");

                
                if (moduleRoleDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("User Details Showed");
                _response.Result = _mapper.Map<List<ModuleRoleMappingDTO>>(moduleRoleDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }
        [HttpGet]
        [Route("api/ModuleRoleMappingController/GetAllModuleRole")]
        [Authorize(Roles = "Admin,Register")]

        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllModuleRole([FromBody] int ModuleRoleId)
        {
            if (ModuleRoleId == 0)
            {

                _response.Messages.Add("User ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                ModuleRoleMapping moduleRoleMapping = await _modelRoleRepository.GetAsync(u => (u.RoleMapId == ModuleRoleId && u.StatusFlag == false), includeProperties: "RoleDetails,Module");


                if (moduleRoleMapping == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<ModuleRoleMapping>(moduleRoleMapping);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [Route("api/ModuleRoleMappingController/moduleRegister")]
        [HttpPost]
        [Authorize(Roles = "Admin,Register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> moduleRegister([FromBody] ModuleRoleMappingDTO model)
        {

            await _modelRoleRepository.ModuleRegister(model, _loginUserid);


            if (model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Error while registering");
                return BadRequest(_response);



            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin,Register")]

        [Route("api/ModuleRoleMappingController/DeleteRoleModule")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> DeleteRoleModule( [FromBody] int RoleMapId)
        {
            if (RoleMapId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (RoleMapId == 0)
                {
                    return BadRequest();
                }


                var Role = await _modelRoleRepository.GetAsync(u => u.RoleMapId == RoleMapId && u.StatusFlag == false);

                if (Role == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var menuId = Role.ModuleId;
                var roleId = Role.RoleId; 

                // Retrieve the associated menu from the Menu table
                var menu = await _moduleRepository.GetAsync(u => u.ModuleId == menuId);

                if (menu != null && menu.ParentId == 0)
                {
                    // Retrieve all child menus with ParentId = menuId
                    var childMenus = await _moduleRepository.GetAllAsync(u => u.ParentId == menuId);

                    // Update IsDeleted property for child menu role mappings in the MenuRoleMapping table
                    var childMenuIds = childMenus.Select(child => child.ModuleId).ToList();

                    // Filter child menu role mappings by MenuId and RoleId
                    var childMenuRoleMappings = await _modelRoleRepository.GetAllUserAsync(u => childMenuIds.Contains(u.ModuleId) && u.RoleId == roleId);

                    foreach (var childMenuRoleMapping in childMenuRoleMappings)
                    {
                        childMenuRoleMapping.StatusFlag = true;
                        await _modelRoleRepository.UpdateAsync(childMenuRoleMapping,_loginUserid);
                    }
                }

                // Soft delete the current menu role mapping
                Role.StatusFlag = true;
                await _modelRoleRepository.UpdateAsync(Role,_loginUserid);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPut]
        [Authorize(Roles = "Admin,Register")]

        [Route("api/ModuleRoleMappingController/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] ModuleRoleMapping module)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (module == null)
                {
                    return BadRequest();

                }

                ModuleRoleMapping model = _mapper.Map<ModuleRoleMapping>(module);

                await _modelRoleRepository.UpdateAsync(model, _loginUserid);
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
        [Authorize(Roles = "Admin,Register")]

        [Route("api/ModuleRoleMappingController/Enable")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> EnableUser([FromBody] int RoleMapId)
        {
            try
            {
                if (RoleMapId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var moduleRoleDTO = await _modelRoleRepository.GetAsync(u => u.RoleMapId == RoleMapId && u.StatusFlag == true);

                if (moduleRoleDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                ModuleRoleMapping moduleRole = _mapper.Map<ModuleRoleMapping>(moduleRoleDTO);



                moduleRole.StatusFlag = false;
                await _modelRoleRepository.UpdateAsync(moduleRole, _loginUserid);

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
        [HttpGet]
        [Route("api/ModuleRoleMappingController/GetAllMenuByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllMenuByRoleId([FromBody]int RoleId)
        {
            try
            {

        
                if (RoleId == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                IEnumerable<ModuleRoleMapping> menuList = await _modelRoleRepository.GetAllUserAsync(u => u.RoleId == RoleId && u.StatusFlag == false, includeProperties: "RoleDetails,Module");
                List<Module> menu = new List<Module>(); 
                foreach (var menuItem in menuList)
                {
                    var MenuList = await _moduleRepository.GetAsync(u => u.ModuleId == menuItem.ModuleId);
                    menu.Add(MenuList);
                }

                _response.Result = (menu);
                _response.StatusCode = HttpStatusCode.OK;
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


