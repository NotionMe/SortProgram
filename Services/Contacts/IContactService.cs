using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Practika2_OPAM_Ubohyi_Stanislav.Models;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services.Contacts
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllContactsAsync(bool includeDeleted = false);
        Task<Contact?> GetContactByIdAsync(Guid contactId);
        Task<Contact> AddContactAsync(Contact contact);
        Task<Contact> UpdateContactAsync(Contact contact);
        Task<bool> DeleteContactAsync(Guid contactId);
        Task<bool> PermanentlyDeleteContactAsync(Guid contactId);
        Task<bool> RestoreContactAsync(Guid contactId);
        Task<List<Contact>> GetTrashAsync();
        Task<List<Contact>> FindPotentialDuplicatesAsync(Contact contact);
        Task<Contact> MergeContactsAsync(List<Guid> contactIds);
    }
}