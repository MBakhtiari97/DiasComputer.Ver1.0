using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Groups;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface ICategoryRepository
    {
        public List<Group> GetAllCategories();
        string GetGroupNameByGroupId(int? groupId);

        #region Admin

        bool AddNewCategory(Group group);
        bool UpdateCategory(Group group);
        bool DeleteCategory(int groupId);
        Group GetGroupById(int groupId);

        #endregion
    }
}
