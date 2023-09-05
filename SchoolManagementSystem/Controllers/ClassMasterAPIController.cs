using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.DTO;
using SchoolManagementSystem.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    public class ClasMasterAPIController:ControllerBase
    {
        private readonly IClassMasterRepository _classRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public ClasMasterAPIController(IClassMasterRepository classRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _classRepository = classRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Authorize(Roles = "Register")]
        [Route("api/ClasMasterAPI/GetAllClass")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllClass()
        {


            try
            {
                List<ClassMaster> ClassListDTO = await _classRepository.GetAllAsync(u=> u.StatusFlag == false);
                if (ClassListDTO == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<ClassMasterDTO>>(ClassListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/ClasMasterAPI/GetClass")]
        [Authorize(Roles = "Register")]
       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetClass(int classId)
        {
            if (classId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                ClassMaster classDetails= await _classRepository.GetAsync(u => (u.ClassId == classId && u.StatusFlag == false));


                if (classDetails == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<ClassMaster>(classDetails);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Register")]
        [Route("api/ClassMasterAPI/Create")]
        [Authorize(Roles = "Register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] ClassMasterDTO classDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if (await _classRepository.GetAsync(u => u.ClassName.ToLower() == classDTO.ClassName.ToLower()) != null)

                {
                    ModelState.AddModelError("ErrorMessages", "Category Name Already Exists");
                    return BadRequest(ModelState);
                }
                if (classDTO == null)
                {
                    return BadRequest(classDTO);


                }

                ClassMaster classmaster= _mapper.Map<ClassMaster>(classDTO);



                await _classRepository.CreateAsync(classmaster, _loginUserid);

                _response.Result = _mapper.Map<CategoryDTO>(classmaster);
                _response.StatusCode = HttpStatusCode.Created;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages = new List<string>() { ex.ToString() };
            }
            return _response;
        }




        [HttpDelete]
        [Authorize(Roles = "Register")]
      
        [Route("api/ClassMasterAPI/Delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete(int classId)
        {
            if (classId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (classId == 0)
                {
                    return BadRequest();
                }


                var classDetails = await _classRepository.GetAsync(u => u.ClassId == classId);
                if (classDetails == null)
                {
                    return NotFound();
                }

                classDetails.StatusFlag = true;
                await _classRepository.UpdateAsync(classDetails, _loginUserid);
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
        [Authorize(Roles = "Register")]
       
        [Route("api/ClassMasterAPI/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] ClassMaster classMaster)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (classMaster == null)
                {
                    return BadRequest();

                }

                ClassMaster model = _mapper.Map<ClassMaster>(classMaster);

                await _classRepository.UpdateAsync(model, _loginUserid);
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
