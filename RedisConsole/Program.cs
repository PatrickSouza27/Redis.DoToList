// See https://aka.ms/new-console-template for more information

using StackExchange.Redis;

namespace RedisCourse.Console;



class Program
{
    static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-18288.c245.us-east-1-3.ec2.redns.redis-cloud.com:18288,password=*******");
    
    static async Task Main(string[] args)
    {
        var db = redis.GetDatabase();
    }
}