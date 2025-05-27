using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Practika2_OPAM_Ubohyi_Stanislav.Models;

namespace Practika2_OPAM_Ubohyi_Stanislav.Services.Contacts
{
    public class ContactRepository : IContactRepository
    {
        private readonly string _basePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public ContactRepository()
        {
            _basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "DataBase", "Contacts");
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            
            // Ensure the contacts directory exists
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        private string GetUserContactsFilePath(string userId)
        {
            return Path.Combine(_basePath, $"{userId}_contacts.json");
        }

        private async Task<List<Contact>> LoadContactsAsync(string userId)
        {
            var filePath = GetUserContactsFilePath(userId);
            
            if (!File.Exists(filePath))
            {
                return new List<Contact>();
            }

            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var contacts = JsonSerializer.Deserialize<List<Contact>>(json, _jsonOptions);
                return contacts ?? new List<Contact>();
            }
            catch (Exception)
            {
                // If there's an error reading the file, return an empty list
                return new List<Contact>();
            }
        }

        private async Task SaveContactsAsync(string userId, List<Contact> contacts)
        {
            var filePath = GetUserContactsFilePath(userId);
            var json = JsonSerializer.Serialize(contacts, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<List<Contact>> GetAllContactsAsync(string userId, bool includeDeleted = false)
        {
            var contacts = await LoadContactsAsync(userId);
            
            if (!includeDeleted)
            {
                contacts = contacts.Where(c => !c.IsDeleted).ToList();
            }
            
            return contacts;
        }

        public async Task<Contact?> GetContactByIdAsync(string userId, Guid contactId)
        {
            var contacts = await LoadContactsAsync(userId);
            return contacts.FirstOrDefault(c => c.Id == contactId);
        }

        public async Task<Contact> AddContactAsync(string userId, Contact contact)
        {
            var contacts = await LoadContactsAsync(userId);
            
            // Ensure the contact has an ID
            if (contact.Id == Guid.Empty)
            {
                contact.Id = Guid.NewGuid();
            }
            
            // Set created and updated timestamps
            contact.CreatedAt = DateTime.Now;
            contact.UpdatedAt = DateTime.Now;
            
            contacts.Add(contact);
            await SaveContactsAsync(userId, contacts);
            
            return contact;
        }

        public async Task<Contact> UpdateContactAsync(string userId, Contact contact)
        {
            var contacts = await LoadContactsAsync(userId);
            var existingIndex = contacts.FindIndex(c => c.Id == contact.Id);
            
            if (existingIndex == -1)
            {
                throw new KeyNotFoundException($"Contact with ID {contact.Id} not found");
            }
            
            // Update the timestamp
            contact.UpdatedAt = DateTime.Now;
            
            contacts[existingIndex] = contact;
            await SaveContactsAsync(userId, contacts);
            
            return contact;
        }

        public async Task<bool> DeleteContactAsync(string userId, Guid contactId, bool permanent = false)
        {
            var contacts = await LoadContactsAsync(userId);
            var existingIndex = contacts.FindIndex(c => c.Id == contactId);
            
            if (existingIndex == -1)
            {
                return false;
            }
            
            if (permanent)
            {
                contacts.RemoveAt(existingIndex);
            }
            else
            {
                // Mark as deleted (move to trash)
                contacts[existingIndex].IsDeleted = true;
                contacts[existingIndex].UpdatedAt = DateTime.Now;
            }
            
            await SaveContactsAsync(userId, contacts);
            return true;
        }

        public async Task<bool> RestoreContactAsync(string userId, Guid contactId)
        {
            var contacts = await LoadContactsAsync(userId);
            var existingIndex = contacts.FindIndex(c => c.Id == contactId && c.IsDeleted);
            
            if (existingIndex == -1)
            {
                return false;
            }
            
            contacts[existingIndex].IsDeleted = false;
            contacts[existingIndex].UpdatedAt = DateTime.Now;
            
            await SaveContactsAsync(userId, contacts);
            return true;
        }

        public async Task<List<Contact>> FindPotentialDuplicatesAsync(string userId, Contact contact)
        {
            var contacts = await LoadContactsAsync(userId);
            var duplicates = new List<Contact>();
            
            foreach (var existingContact in contacts.Where(c => !c.IsDeleted && c.Id != contact.Id))
            {
                if (contact.IsPotentialDuplicateOf(existingContact))
                {
                    duplicates.Add(existingContact);
                }
            }
            
            return duplicates;
        }
    }
}