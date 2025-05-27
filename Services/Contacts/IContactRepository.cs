using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Practika2_OPAM_Ubohyi_Stanislav.Models;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services.Contacts
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllContactsAsync(string userId, bool includeDeleted = false);
        Task<Contact?> GetContactByIdAsync(string userId, Guid contactId);
        Task<Contact> AddContactAsync(string userId, Contact contact);
        Task<Contact> UpdateContactAsync(string userId, Contact contact);
        Task<bool> DeleteContactAsync(string userId, Guid contactId, bool permanent = false);
        Task<bool> RestoreContactAsync(string userId, Guid contactId);
        Task<List<Contact>> FindPotentialDuplicatesAsync(string userId, Contact contact);
    }
}