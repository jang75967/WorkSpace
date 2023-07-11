using MongoDB.Bson;
using MongoDB.Extensions.Migration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB.Migrations;

public class Migration : IMigration
{
    public int Version => 1;

    public void Up(BsonDocument document)
    {
        document["Name"] = document["Namee"];
    }

    public void Down(BsonDocument document)
    {
        
    }
}
