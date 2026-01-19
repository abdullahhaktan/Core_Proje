using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class ExperienceManager : IExperienceService
    {
        IExperienceDal _experienceDal;

        public ExperienceManager(IExperienceDal experienceDal)
        {
            _experienceDal = experienceDal;
        }

        public void TAdd(Experience t)
        {
            _experienceDal.Insert(t);
        }

        public void TDelete(Experience t)
        {
            _experienceDal.Delete(t);
        }

        public List<Experience> TGetList()
        {
            return _experienceDal.GetList();
        }

        public Experience TGetByID(int id)
        {
            return _experienceDal.GetByID(id);
        }

        public void TUpdate(Experience t)
        {
            _experienceDal.Update(t);
        }

        public List<Experience> TGetListbyFilter()
        {
            throw new NotImplementedException();
        }
    }
}