using Domain;

namespace DAL;

public class Repository
{
    private readonly AppDbContext _ctx;

    public Repository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public void AddTask(TodoTask task)
    {
        if (GetTask(task.TaskId) != null)
        {
            _ctx.Tasks.Update(task);
        }
        else
        {
            _ctx.Tasks.Add(task);
        }

        _ctx.SaveChanges();
    }

    public bool DeleteTask(Guid id)
    {
        var task = GetTask(id);
        if (task == null) return false;
        _ctx.Tasks.Remove(task);
        _ctx.SaveChanges();

        return true;
    }

    public TodoTask? GetTask(Guid id)
    {
        return _ctx.Tasks.Find(id);
    }

}