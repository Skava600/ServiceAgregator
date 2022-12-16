using TestApp;
using TestApp.Instances;

ApplicationDbContract applicationDbContract = new ApplicationDbContract("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=cleantest;Pooling=true;");
await applicationDbContract.MigrateAsync();

await applicationDbContract.Users.AddAsync(new User()
{
    Id = Guid.NewGuid(),
    Age = 18,
    IsMale = true,
    Name = "Gabe",
});
await applicationDbContract.Users.AddAsync(new User()
{
    Id = Guid.NewGuid(),
    Age = 15,
    IsMale = false,
    Name = "Anna",
});
await applicationDbContract.Users.AddAsync(new User()
{
    Id = Guid.NewGuid(),
    Age = 76,
    IsMale = true,
    Name = "Anton",
});
await applicationDbContract.Users.AddAsync(new User()
{
    Id = Guid.NewGuid(),
    Age = 34,
    IsMale = false,
    Name = "Lisa",
});
IEnumerable<User> users = await applicationDbContract.Users.ReadAll();
IEnumerable<User> lisas = await applicationDbContract.Users.GetByField(nameof(User.Name), "Lisa");
User any = await applicationDbContract.Users.GetByIdAsync(users.Last().Id);

any.Age = 228;
any.Name = "NotLisa";
any.IsMale = true;
await applicationDbContract.Users.UpdateByIdAsync(any.Id, any);
User anyUpdated = await applicationDbContract.Users.GetByIdAsync(any.Id);