namespace DataAccess.DAOs;

public abstract class SingletonBase<T> where T : class, new()
{
    private static T _instance;

    private static readonly object _lock = new();

    public static BanHangContext _context = new();

    public static T Instance
    {
        get
        {
            if (_instance == null)
                lock (_lock)
                {
                    if (_instance == null) _instance = new T();
                }

            return _instance;
        }
    }
}