using ShopSMS.DAL.Infrastructure.Interfaces;
using ShopSMS.DAL.Repositories;
using ShopSMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Service.Services
{
    public interface IMenuGroupService
    {
        IEnumerable<MenuGroup> GetAll();       
    }

    public class MenuGroupService : IMenuGroupService
    {
        private readonly IMenuGroupRepository menuGroupRepository;
        private readonly IUnitOfWork unitOfWork;
        public MenuGroupService(
            IMenuGroupRepository menuGroupRepository,
            IUnitOfWork unitOfWork)
        {
            this.menuGroupRepository = menuGroupRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<MenuGroup> GetAll()
        {
            return menuGroupRepository.GetAll();
        }
    }
}
