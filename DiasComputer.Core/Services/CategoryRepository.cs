using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.Groups;

namespace DiasComputer.Core.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private DiasComputerContext _context;

        public CategoryRepository(DiasComputerContext context)
        {
            _context = context;
        }

        public List<Group> GetAllCategories()
        {
            return _context.Groups
                .ToList();
        }

        public string GetGroupNameByGroupId(int? groupId)
        {
            return _context.Groups.Single(g => g.GroupId == groupId).GroupTitle;
        }

        public bool AddNewCategory(Group group)
        {
            try
            {
                _context.Groups.Add(group);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCategory(Group group)
        {
            try
            {
                _context.Update(group);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCategory(int groupId)
        {
            try
            {
                var group = GetGroupById(groupId);
                group.IsDelete = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Group GetGroupById(int groupId)
        {
            return _context.Groups
                .Find(groupId);
        }
    }
}
