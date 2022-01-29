namespace API.Data.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly HostelContext db;

        public UserRepo(HostelContext db)
        {
            this.db = db;
        }

        public async Task<Users> Authenticate(string username, string password)
        {
            var user = await db.users.FirstOrDefaultAsync(x => (x.FirstName + " " + x.LastName) == username);

            if (user == null || user.PasswordKey == null)
            {
                return null; // return somethineg else which will be used in controller to distinguish between an invalid user
                // and an expired token
            }

            if(!MatchPasswordHash(password, user.Password, user.PasswordKey))
            {
                return null;
            }

            return user;
        }


        private bool MatchPasswordHash(string passwordText, byte[] password, byte[] passwordKey)
        {
            using(var hmac = new HMACSHA512(passwordKey))
            {
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordText));

                for(int i=0; i<passwordHash.Length; i++)
                {
                    if(passwordHash[i] != password[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }


        public void Register(AccountsDto userDto)
        {
            byte[] passwordHash, passwordKey;

            using(var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            }

            Users user = new();
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;
            user.Address = userDto.Address;
            user.Phone = userDto.Phone;

            db.users.Add(user);
        }


        public async Task<bool> UserAlreadyExists(string username, string email)
        {
            return await db.users.AnyAsync(x => (x.FirstName + ' ' + x.LastName) == username && x.Email == email);
        }


        #region DeleteUser
        public void DeleteUser(int id)
        {
            var user = db.users.Find(id);

            if(user != null)
            {
                db.users.Remove(user);
            }
        }
        #endregion

        #region GetUsersAsync
        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            return await db.users.ToListAsync();
        }
        #endregion

        #region GetUsersById
        public async Task<Users> GetUsersById(int id)
        {
            return await db.users.FindAsync(id);
        }
        #endregion
    }
}
