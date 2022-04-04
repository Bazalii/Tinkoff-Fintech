﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniBank.Core.Domains.BankAccounts.Repositories;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Core.Exceptions;

namespace MiniBank.Core.Domains.Users.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IBankAccountRepository _bankAccountRepository;
        
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(UserCreationModel model)
        {
            await _userRepository.Add(new User
            {
                Id = Guid.NewGuid(),
                Login = model.Login,
                Email = model.Email
            });
            
            await _unitOfWork.SaveChanges();
        }

        public async Task<User> GetById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task Update(User user)
        {
            await _userRepository.Update(user);
            await _unitOfWork.SaveChanges();
        }

        public async Task DeleteById(Guid id)
        {
            var check = await _bankAccountRepository.ExistsForUser(id);
            
            if (check)
            {
                throw new ValidationException($"User with id: {id} has connected accounts!");
            }

            await _userRepository.DeleteById(id);
            await _unitOfWork.SaveChanges();
        }
    }
}