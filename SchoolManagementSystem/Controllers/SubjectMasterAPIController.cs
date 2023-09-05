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
    public class SubjectMasterAPIController:ControllerBase
    {
        private readonly ISubjectMasterRepository _subjectRepository;
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly int _loginUserid;


        public SubjectMasterAPIController(ISubjectMasterRepository subjectRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
            _response = new();
            _loginUserid = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        }

        [HttpGet]
        [Authorize(Roles = "Register")]
        [Route("api/SubjectMasterAPIController/GetAllSubject")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetAllSubject()
        {


            try
            {
                List<SubjectMaster> SubjectListDTO = await _subjectRepository.GetAllAsync(u=> u.StatusFlag == false);
                if (SubjectListDTO == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<List<SubjectMasterDTO>>(SubjectListDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Messages.Add(ex.Message);
            }

            return _response;
        }

        [HttpGet]
        [Route("api/SubjectMasterAPIController/GetSubject")]
        [Authorize(Roles = "Register")]
       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetSubject(int subjectId)
        {
            if (subjectId == 0)
            {

                _response.Messages.Add("Role ID is Null");
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            try
            {
                SubjectMaster subjectDetails= await _subjectRepository.GetAsync(u => (u.SubjectId == subjectId&& u.StatusFlag == false));


                if (subjectDetails == null)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Messages = new List<string>() { "record not found" };
                    return BadRequest(_response);
                }


                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Messages.Add("Role Details Showed");
                _response.Result = _mapper.Map<SubjectMaster>(subjectDetails);
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
        [Route("api/SubjectMasterAPIController/Create")]
        [Authorize(Roles = "Register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] SubjectMasterDTO subjectDTO)
        {

            try
            {
                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }

                if (await _subjectRepository.GetAsync(u => u.SubjectName.ToLower() == subjectDTO.SubjectName.ToLower()) != null)

                {
                    ModelState.AddModelError("ErrorMessages", "Category Name Already Exists");
                    return BadRequest(ModelState);
                }
                if (subjectDTO == null)
                {
                    return BadRequest(subjectDTO);


                }

                SubjectMaster subject= _mapper.Map<SubjectMaster>(subjectDTO);



                await _subjectRepository.CreateAsync(subject, _loginUserid);

                _response.Result = _mapper.Map<SubjectMasterDTO>(subject);
                _response.StatusCode = HttpStatusCode.Created;

                return Ok(subjectDTO);
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
      
        [Route("api/SubjectMasterAPIController/Delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Delete(int subjectId)
        {
            if (subjectId == null)
            {
                _response.Messages.Add("Error while Adding");
            }
            try
            {
                if (subjectId == 0)
                {
                    return BadRequest();
                }


                var subject = await _subjectRepository.GetAsync(u => u.SubjectId == subjectId);
                if (subject == null)
                {
                    return NotFound();
                }

                subject.StatusFlag = true;
                await _subjectRepository.UpdateAsync(subject, _loginUserid);
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
       
        [Route("api/SubjectMasterAPIController/Update")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<APIResponse>> Update([FromBody] SubjectMaster subject)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (subject == null)
                {
                    return BadRequest();

                }

                SubjectMaster model = _mapper.Map<SubjectMaster>(subject);

                await _subjectRepository.UpdateAsync(model, _loginUserid);
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
