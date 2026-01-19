using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class AdminManager : IAdminService
    {
        IAdminDal _adminDal;

        public AdminManager(IAdminDal admin)
        {
            _adminDal = admin;
        }

        public void TAdd(Admin t)
        {
            _adminDal.Insert(t);
        }

        public void TDelete(Admin t)
        {
            _adminDal.Delete(t);
        }

        public List<Admin> TGetList()
        {
            return _adminDal.GetList();
        }

        public Admin TGetByID(int id)
        {
            return _adminDal.GetByID(id);
        }

        public void TUpdate(Admin t)
        {
            _adminDal.Update(t);
        }

        public List<About> TGetListbyFilter()
        {
            throw new NotImplementedException();
        }

        List<Admin> IGenericService<Admin>.TGetListbyFilter()
        {
            throw new NotImplementedException();
        }
    }
}
