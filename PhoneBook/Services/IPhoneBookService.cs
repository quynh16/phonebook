﻿using PhoneBook.Model;

namespace PhoneBook.Services
{
    public interface IPhoneBookService
    {
        void Add(PhoneBookEntry phoneBookEntry);
        void Add(string name, string phoneNumber);
        void DeleteByName(string name);
        void DeleteByNumber(string number);
        IEnumerable<PhoneBookEntry> List();
    }
}