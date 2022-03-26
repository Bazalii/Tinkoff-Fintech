﻿using System;
using System.Collections.Generic;

namespace MiniBank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        void Add(BankAccount bankAccount);

        BankAccount GetById(Guid id);

        IEnumerable<BankAccount> GetAll();

        void Update(BankAccount bankAccount);

        void UpdateAccountMoney(Guid id, double amountOfMoney);

        void DeleteById(Guid id);

        bool ExistsForUser(Guid accountId);
    }
}