using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Practika2_OPAM_Ubohyi_Stanislav.Models
{
    public class Contact : INotifyPropertyChanged
    {
        private Guid _id;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phone;
        private string _address;
        private string _notes;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private bool _isDeleted;

        public Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set
            {
                if (_updatedAt != value)
                {
                    _updatedAt = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDeleted
        {
            get => _isDeleted;
            set
            {
                if (_isDeleted != value)
                {
                    _isDeleted = value;
                    OnPropertyChanged();
                }
            }
        }

        // Helper method to check if this contact is a potential duplicate of another
        public bool IsPotentialDuplicateOf(Contact other)
        {
            if (other == null) return false;
            
            // Check for matching email (if both have email)
            if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(other.Email) && 
                Email.Equals(other.Email, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            // Check for matching phone (if both have phone)
            if (!string.IsNullOrWhiteSpace(Phone) && !string.IsNullOrWhiteSpace(other.Phone) && 
                Phone.Equals(other.Phone, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            // Check for matching name (if both first and last name match)
            if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(other.FirstName) && !string.IsNullOrWhiteSpace(other.LastName) &&
                FirstName.Equals(other.FirstName, StringComparison.OrdinalIgnoreCase) &&
                LastName.Equals(other.LastName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            return false;
        }

        // Create a new contact by merging this contact with another
        public Contact MergeWith(Contact other)
        {
            if (other == null) return this;
            
            var merged = new Contact
            {
                Id = Id,
                FirstName = FirstName ?? other.FirstName,
                LastName = LastName ?? other.LastName,
                Email = Email ?? other.Email,
                Phone = Phone ?? other.Phone,
                Address = Address ?? other.Address,
                Notes = string.IsNullOrWhiteSpace(Notes) 
                    ? other.Notes 
                    : string.IsNullOrWhiteSpace(other.Notes) 
                        ? Notes 
                        : $"{Notes}\n\n{other.Notes}",
                CreatedAt = CreatedAt < other.CreatedAt ? CreatedAt : other.CreatedAt,
                UpdatedAt = DateTime.Now,
                IsDeleted = false
            };
            
            return merged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}