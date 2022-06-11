using DataAccess;
using DataAccess.General.Interface;
using Domain;
using GlobusTest.Controllers;
using GlobusTest.Settings;
using GlobusTest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestProject
{
    public class CustomerControllerTest
    {
        private readonly Mock<ICustomerRepository> _customerRepository;
        private readonly Mock<IOtpRepository> _otpRepository;
        private readonly IOptions<LgaSettings> lgaSettings;
        private readonly CustomerController _controller;
        public CustomerControllerTest()
        {
            _customerRepository = new Mock<ICustomerRepository>();
            _otpRepository = new Mock<IOtpRepository>();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", false)
           .Build();
            lgaSettings = Options.Create(configuration.GetSection("LgaSettings").Get<LgaSettings>());
            _controller = new CustomerController(lgaSettings,_customerRepository.Object,_otpRepository.Object);
        }

        [Fact]
        public void OnboardCustomer_ShouldReturnOk_CustomerModelIsValid ()
        {
            var customers = OnboardSampleCustomers();
            var newCustomer = customers[0];
            var actionResult = _controller.Onboard(newCustomer);
            var result = actionResult.Result;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void OnboardCustomer_ShouldReturnBadRequest_CustomerModelIsInValid()
        {
            var customers = OnboardSampleCustomers();
            var newCustomer = customers[1];
            var actionResult = _controller.Onboard(newCustomer);
            var result = actionResult.Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ActivateCustomer_ShouldReturnOk_ProvidedCorrectOtpAndId()
        {
            var sampleCustomer = GetSampleCustomers();
            _customerRepository.Setup(x => x.GetCustomer(2)).ReturnsAsync(sampleCustomer[0]);            
            var actionResult = _controller.ActivateCustomer(2,"98765");
            var result = actionResult.Result;
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ActivateCustomer_ShouldReturnNotFound_ProvidedCorrectOtpAndIncorrectId()
        {
            var sampleCustomer = GetSampleCustomers();
            _customerRepository.Setup(x => x.GetCustomer(2)).ReturnsAsync(sampleCustomer[0]);
            var actionResult = _controller.ActivateCustomer(1, "98765");
            var result = actionResult.Result;
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ActivateCustomer_ShouldReturnBadRequest_ProvidedWrongOtpAndCorrectId()
        {
            var sampleCustomer = GetSampleCustomers();
            _customerRepository.Setup(x => x.GetCustomer(2)).ReturnsAsync(sampleCustomer[0]);
            var actionResult = _controller.ActivateCustomer(2, "98769");
            var result = actionResult.Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Customers_ShouldReturnOk()
        {
            var sampleCustomer = GetSampleCustomers();
            _customerRepository.Setup(x => x.GetAll()).Returns(sampleCustomer);
            var actionResult = _controller.Customers();
            var result = actionResult.Result as OkObjectResult;
            var actual = result.Value as ResponseMessage<IEnumerable<Customer>>;
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(sampleCustomer.Count, actual.Data.Count());
        }

        [Fact]
        public void States_ShouldReturnOk()
        {
            var actionResult = _controller.States();
            var result = actionResult as OkObjectResult;
            Assert.IsType<OkObjectResult>(result);
        }

        private List<OnboardCustomer> OnboardSampleCustomers()
        {
            List<OnboardCustomer> output = new List<OnboardCustomer>
            {
                new OnboardCustomer
                {
                    State = "FCT",
                    LGA = "Kuje",
                    Phonenumber = "01682616789",
                    Password = "Hydnn7ji",
                    Email = "jhon@gmail.com",
                },
                 new OnboardCustomer
                {
                    State = "Test",
                    LGA = "Kuje",
                    Phonenumber = "01682616789",
                    Password = "Hydnn7ji",
                    Email = "jhon@gmail.com",
                }
            };
            return output;
        }

        private List<Customer> GetSampleCustomers()
        {
            List<Customer> output = new()
            {
                new Customer
                {
                    Id = 2,
                    State = "FCT",
                    LGA = "Kuje",
                    Phonenumber = "01682616789",
                    Password = "Hydnn7ji",
                    Email = "jhon@gmail.com",
                    Activated = false,
                    Otp = new Otp
                    {
                        Id = 2,
                        OtpCode = "98765",
                        CustomerId = 2,                        
                    }
                }
            };
            return output;
        }
    }
}
