using Npgsql;

var regions = new[] { "us-east-1", "us-west-1", "us-east-2", "eu-west-1", "eu-west-2", "eu-central-1", "ap-southeast-1", "ap-southeast-2", "ap-northeast-1", "ap-northeast-2", "ap-south-1", "sa-east-1" };

foreach (var region in regions)
{
    var cs = $"Host=aws-0-{region}.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.ycncdwtgxngwxawgrsad;Password=TPeygMvmy8HOAW0e;SSL Mode=Require;Trust Server Certificate=true;Timeout=5";
    try
    {
        await using var conn = new NpgsqlConnection(cs);
        await conn.OpenAsync();
        Console.WriteLine($"SUCCESS: {region}");
        break;
    }
    catch (Exception ex)
    {
        var msg = ex.InnerException?.Message ?? ex.Message;
        Console.WriteLine($"{region}: {msg}");
    }
}
