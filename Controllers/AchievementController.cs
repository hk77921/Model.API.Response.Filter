using API.Response.Filter.Authorization;
using API.Response.Filter.Models;
using API.Response.Filter.Service;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace API.Response.Filter.Controllers
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AchievementController : ControllerBase
    {
        private readonly RequestValidator _requestValidator;
        private readonly IConfiguration _configuration;
        private readonly Service.UserProfile _userProfile;

        public AchievementController(RequestValidator requestValidator, IConfiguration configuration, Service.UserProfile userProfile)
        {
            _requestValidator = requestValidator;
            _configuration = configuration;
            _userProfile = userProfile;
        }



        [HttpGet]
        [MapToApiVersion("1.0")]
        public ActionResult GetClientProfile(string CertificateNumber, string Surname, string DateOfBirth, string CourseCode)
        {
            ClientProfile clientProfile = new ClientProfile();
            Params _param = new Params();

            if (string.IsNullOrEmpty(CertificateNumber) || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(DateOfBirth) || string.IsNullOrEmpty(CourseCode))
                return BadRequest();
            if (DateOfBirth.Trim().Length > 8)
                return BadRequest("Date of birth parameter is  not valid");



            var _dob = DateTime.ParseExact(DateOfBirth, "ddMMyyyy", CultureInfo.InvariantCulture);
            string _formattedDOB = _dob.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                  


            _param.CertificateNumber = CertificateNumber;
            _param.Surname = Surname;
            _param.DateOfBirth = _formattedDOB;
            _param.CourseCode = CourseCode;
            var _baseUrl = _configuration.GetValue<string>("LMS_Base_URL");

            if (_requestValidator.IsValidToken(HttpContext))
            {
                var data = Task.FromResult(_userProfile.GetAllUserProfile(_baseUrl, _param));
                if (data.Result.Result != null)
                    return Ok(data.Result.Result);
                else
                    return NotFound();
            }
            else
            {
                return Unauthorized();
            }




        }


    }
}
