// See https://aka.ms/new-console-template for more information

using MongoDB.Driver;
using Todo.ConsoleDev;
using Todo.Domain.Roles;
using Todo.Domain.Users;


var client = new MongoClient("mongodb://localhost:27017/");
var database = client.GetDatabase("MyTodo");

var roles = database.GetCollection<Role>("Roles");

using (var session = client.StartSession())
{
    session.StartTransaction();
    roles.InsertOne(new Role()
    {
        Name = "a11v1111111c",
        Code = "abs"
    });

}



