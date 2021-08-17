using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Interfaces
{
    public interface ICommonFunctions
    {
        Task GoToUserProfile(int userId);
        Task GetUserInfoById();
        Task<int> VerifyIfConversationExists(int userid2);


    }
}
