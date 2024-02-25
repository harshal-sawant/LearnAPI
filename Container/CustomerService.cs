﻿using AutoMapper;
using LearnAPI.Helper;
using LearnAPI.Modal;
using LearnAPI.Repos;
using LearnAPI.Repos.Models;
using LearnAPI.Service;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LearnAPI.Container
{
    public class CustomerService : ICustomerService
    {
        private readonly LearndataContext context;
        private readonly IMapper mapper;
        private readonly ILogger<CustomerService> logger;

        public CustomerService(LearndataContext context, IMapper mapper, ILogger<CustomerService> logger) {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }
        
        public async Task<List<CustomerModal>> GetAll()
        {
            List<CustomerModal> _response= new List<CustomerModal>();
            var _data = await this.context.TblCustomers.ToListAsync();
            if(_data != null)
            {
                _response = this.mapper.Map<List<TblCustomer>,List<CustomerModal>>(_data);
            }
            return _response;
        }

        public async Task<APIResponse> Create(CustomerModal data)
        {
            APIResponse response= new APIResponse();
            try
            {
                this.logger.LogInformation("Create Begins");
                TblCustomer _customer = this.mapper.Map<CustomerModal, TblCustomer>(data);
                await this.context.TblCustomers.AddAsync( _customer );
                await this.context.SaveChangesAsync();
                response.ResponseCode= 201;
                response.Result = data.Code;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.ErrorMessage = ex.Message;
                this.logger.LogError(ex.Message, ex);
            }
            return response;
        }

        public async Task<CustomerModal>  GetByCode(string code)
        {
           CustomerModal _response = new CustomerModal();
            var _data = await this.context.TblCustomers.FindAsync(code);
            if (_data != null)
            {
                _response = this.mapper.Map<TblCustomer,CustomerModal>(_data);
            }
            return _response;
        }

        public async Task<APIResponse> Remove(string code)
        {
            APIResponse response = new APIResponse();
            try
            {
               var _customer = await this.context.TblCustomers.FindAsync(code);
                if (_customer != null)
                {
                    this.context.TblCustomers.Remove( _customer );
                    await this.context.SaveChangesAsync() ;
                    response.ResponseCode = 200;
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.ErrorMessage= "Data not found";
                }

                
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> Update(CustomerModal data, string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this.context.TblCustomers.FindAsync(code);
                if (_customer != null)
                {
                    _customer.Name = data.Name;
                    _customer.Email= data.Email;
                    _customer.Phone= data.Phone;
                    _customer.IsActive= data.IsActive;
                    _customer.Creditlimit= data.Creditlimit;
                    
                    await this.context.SaveChangesAsync();

                    response.ResponseCode = 200;
                    response.Result = code;
                }
                else
                {
                    response.ResponseCode = 404;
                    response.ErrorMessage = "Data not found";
                }


            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
