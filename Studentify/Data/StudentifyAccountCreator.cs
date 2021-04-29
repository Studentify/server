using System;
using Studentify.Models;
using Studentify.Models.Authentication;

namespace Studentify.Data
{
    public class StudentifyAccountCreator
    {
        private readonly StudentifyDbContext _context;

        public StudentifyAccountCreator(StudentifyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates an account in database for an application user.
        /// </summary>
        /// <param name="user">
        /// User created with identity framework.
        /// </param>
        /// <returns>
        /// Auth response containing result of an operation
        /// </returns>
        public AuthResponse CreateAccount(StudentifyUser user)
        {
            try
            {
                var account = GetAccount(user);
                AddAccountToDatabase(account);
            }
            catch (SystemException e)
            {
                Console.WriteLine(e);
                return new AuthResponse { Status = "Error", Message = e.Message };
            }

            return new AuthResponse { Status = "Success", Message = "Account has been created" };
        }

        /// <summary>
        /// Add account to the database.
        /// </summary>
        /// <param name="account">
        ///     User account to be added.
        /// </param>
        private void AddAccountToDatabase(StudentifyAccount account)
        {
            _context.StudentifyAccounts.AddAsync(account);
            var addedAccounts = _context.SaveChanges();

            if (addedAccounts <= 0)
            {
                throw new SystemException("Could not create an account");
            }
        }

        /// <summary>
        /// Returns account created for a passed user
        /// </summary>
        private StudentifyAccount GetAccount(StudentifyUser user)
        {
            return new StudentifyAccount()
            {
                StudentifyUsername = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
    }
}
