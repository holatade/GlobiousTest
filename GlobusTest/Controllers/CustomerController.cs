using DataAccess;
using DataAccess.General.Interface;
using Domain;
using GlobusTest.Settings;
using GlobusTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobusTest.Controllers
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly LgaSettings _lgaSettings;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOtpRepository _otpRepository;

        public CustomerController(IOptions<LgaSettings> LgaSettings, ICustomerRepository customerRepository, IOtpRepository otpRepository)
        {
            _lgaSettings = LgaSettings.Value;
            _customerRepository = customerRepository;
            _otpRepository = otpRepository;
        }

        /// <summary>
        /// Onboard Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(ResponseMessage<Customer>))]
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        [HttpPost("[action]")]
        public async Task<IActionResult> Onboard([FromBody] OnboardCustomer customer)
        {
            if (ModelState.IsValid)
            {
                var states = _lgaSettings.SateList.Select(x => x.States);
                var state = states.FirstOrDefault(x => x.Name == customer.State);
                if(state != null)
                {
                    var lga = state.Locals.Where(x => x.Name == customer.LGA).SingleOrDefault();
                    if(lga is null) return BadRequest(new ResponseMessage { ResponseCode = "12", ResponseDescription = "Lga is invalid" });
                    var otp = GenerateOTP();
                    var customerDomain = new Customer
                    {
                        Email = customer.Email,
                        LGA = customer.LGA,
                        State = customer.State,
                        Phonenumber = customer.Phonenumber,
                        Password = customer.Password
                    };
                    _customerRepository.Create(customerDomain);
                    await _customerRepository.Save();
                    _otpRepository.Create(new Otp
                    {
                        CustomerId = customerDomain.Id,
                        OtpCode = otp
                    });
                    await _otpRepository.Save();
                    await SendOTP(otp);
                    return Ok(new ResponseMessage<Customer> {ResponseCode="00", ResponseDescription="Approved or Completed Successfully" ,
                        Data = customerDomain });
                }
                return BadRequest(new ResponseMessage { ResponseCode = "12", ResponseDescription = "State is invalid" });
            }
            //return validation errors
            var errors = new List<string>();
            var errorList = ModelState.Values.SelectMany(m => m.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            foreach (var error in errorList)
            {
                errors.Add(error);
            }
            return BadRequest(new ResponseMessage {ResponseDescription = errors.FirstOrDefault().ToString(), ResponseCode= "30" });
        }

        /// <summary>
        /// Activate Customer with otp
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(ResponseMessage))]
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        [ProducesResponseType(404, Type = typeof(ResponseMessage))]
        [HttpGet("[action]")]
        public async Task<IActionResult> ActivateCustomer(long customerId, string otpCode)
        {
            var customer = await _customerRepository.GetCustomer(customerId);

            if (customer is null) return NotFound(new ResponseMessage { ResponseCode = "25", ResponseDescription = "No record found" });
            if(customer.Otp != null)
            {
                if(customer.Otp.OtpCode == otpCode)
                {
                    customer.Activated = true;
                    _customerRepository.Update(customer);
                    await _customerRepository.Save();
                    return Ok(new ResponseMessage { ResponseDescription = "Approved or completed successfully", ResponseCode = "00" });
                }
                return BadRequest(new ResponseMessage { ResponseCode = "12", ResponseDescription = "Invalid Otp" });
            }
            return NotFound(new ResponseMessage { ResponseCode = "25", ResponseDescription = "No record found" });
        }

        /// <summary>
        /// Get States
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(ResponseMessage<List<string>>))]
        [HttpGet("[action]")]
        public ActionResult States()
        {
            var states = _lgaSettings.SateList.Select(x => x.States.Name).ToList();
            return Ok(new ResponseMessage<List<string>> { ResponseDescription = "Approved or Completed successfully", ResponseCode = "00", Data = states });
        }

        /// <summary>
        /// Get Localgovernments
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(ResponseMessage<List<string>>))]
        [ProducesResponseType(400, Type = typeof(ResponseMessage))]
        [HttpGet("[action]")]
        public ActionResult LGA(string state)
        {
            var lg = _lgaSettings.SateList.Where(x => x.States.Name == state).Select(x => x.States.Locals).FirstOrDefault();
            if (!lg.Any()) return BadRequest(new ResponseMessage { ResponseCode="12", ResponseDescription="Invalid State"});
            var lgs = lg.Select(x => x.Name).ToList();
            return Ok(new ResponseMessage<List<string>> { ResponseDescription = "Approved or Completed successfully", ResponseCode = "00", Data = lgs });
        }

        /// <summary>
        /// Get All Customers
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(ResponseMessage<IEnumerable<Customer>>))]
        [HttpGet("[action]")]
        public ActionResult<ResponseMessage<IEnumerable<Customer>>> Customers()
        {
            var customers = _customerRepository.GetAll();
            return Ok(new ResponseMessage<IEnumerable<Customer>> { ResponseCode = "00", ResponseDescription = "Approved or completed successfully", Data = customers });
        }
       
        /// <summary>
        /// Generate OTP
        /// </summary>
        /// <returns></returns>
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(10000, 99999).ToString("00000");
        }

        /// <summary>
        /// Mock sending OTP
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        private async Task SendOTP(string otp)
        {
            Random random = new Random();
            random.Next(10000, 99999).ToString("00000");
            await Task.CompletedTask;
        }
    }
}
