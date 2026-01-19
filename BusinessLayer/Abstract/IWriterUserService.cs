using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IWriterUserService : IGenericService<WriterUser>
    {
        WriterUser GetByEmail(string mail);
    }
}
