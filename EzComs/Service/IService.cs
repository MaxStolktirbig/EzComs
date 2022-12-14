namespace EzComs.Service
{
    public interface IService<T>
    {
        static abstract T create(T entity);
        static abstract T save(T entity);
        static abstract T get(String id);
        static abstract Boolean exists(String id);
        static abstract Boolean delete(T entity);
    }
}
