﻿using System;
using System.Collections.Generic;
using System.Linq;
using MiniBank.Core.Domains.Users;
using MiniBank.Core.Domains.Users.Repositories;
using MiniBank.Data.Exceptions;

namespace MiniBank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserDbModel> _users = new();

        public void Add(User user)
        {
            _users.Add(new UserDbModel
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        public User GetById(Guid id)
        {
            var dbModel = _users.FirstOrDefault(currentUser => currentUser.Id == id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"User with id: {id} is not found!");
            }

            return new User
            {
                Id = dbModel.Id,
                Login = dbModel.Login,
                Email = dbModel.Email
            };
        }

        public IEnumerable<User> GetAll()
        {
            return _users.Select(user => new User
            {
                Id = user.Id,
                Login = user.Login,
                Email = user.Email
            });
        }

        public void Update(User user)
        {
            var dbModel = _users.FirstOrDefault(currentUser => currentUser.Id == user.Id);
            if (dbModel == null)
            {
                throw new ObjectNotFoundException($"User with id: {user.Id} is not found!");
            }

            dbModel.Login = user.Login;
            dbModel.Email = user.Email;
        }

        public void DeleteById(Guid id)
        {
            _users.RemoveAt(GetIndex(id));
        }

        public bool Exists(Guid id)
        {
            var index = GetIndex(id);
            return index != -1;
        }

        private int GetIndex(Guid id)
        {
            var index = _users.FindIndex(currentUser => currentUser.Id == id);
            return index;
        }
    }
}