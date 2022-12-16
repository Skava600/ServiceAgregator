namespace ORM;

public class DbTable<T> where T : DbInstance, new()
{
    private readonly DbContract contract;
    private readonly string name;
    
    public DbTable(DbContract contract, string name)
    {
        this.contract = contract;
        this.name = name;
    }

    public async Task AddAsync(T instance)
    {
        await this.contract.AddInstance(name, TableMappingHelper.MapToDictionary(instance));
    }

    public async Task<IEnumerable<T>> GetByField(string name, string value)
    {
        List<T> instances = new List<T>();
        var data = await this.contract.GetAllByField(this.name, name, value);
        foreach (var row in data)
        {
            instances.Add(TableMappingHelper.MapToInstance<T>(row));
        }

        return instances;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return (await this.GetByField("Id", id.ToString())).First();
    }
    
    public async Task UpdateByIdAsync(Guid id, T instance)
    {
        await this.contract.UpdateByIdAsync(this.name, id.ToString(), TableMappingHelper.MapToDictionary(instance));
    }
    
    public async Task<IEnumerable<T>> ReadAll()
    {
        List<T> instances = new List<T>();
        var data = await this.contract.ReadAll(this.name);
        foreach (var row in data)
        {
            instances.Add(TableMappingHelper.MapToInstance<T>(row));
        }

        return instances;
    }
}