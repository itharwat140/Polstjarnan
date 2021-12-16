using System;
using System.Collections.Generic;
using VHS.Entity;

namespace VHSBackend.Core.Repository
{
    public interface IUserRepository
    {
        IList<User> GetUserList();
        IList<User> SearchUser(string searchString);
        Guid CreateUser(UserFull user);
        Enums.PersistUserTagRelationStatus PersistUserTagRelation(Guid userId, UserRfIdRelation dto);
        UserWithTags GetUserTags(Guid userId);
        bool TryLogin(string userName, string password, out UserAuthenticated user);
        bool ValidateToken(string token, out User user);
        User GetUserById(Guid userId);

    }
}
