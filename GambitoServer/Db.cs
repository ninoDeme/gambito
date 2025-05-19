using Microsoft.EntityFrameworkCore;

namespace GambitoServer;

public class Db(DbContextOptions<Db> options) : DbContext(options)
{
}
