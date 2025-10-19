using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCuentas
    {
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta)
        {
            using var connection = new SqlConnection(connectionString);

            var sql = @"INSERT INTO Cuentas (Nombre, TipoCuentaId, Balance, Descripcion)
                VALUES (@Nombre, @TipoCuentaId, @Balance, @Descripcion);
                SELECT SCOPE_IDENTITY();";

            var id = await connection.QuerySingleAsync<int>(sql, cuenta);
            cuenta.Id = id;
        }
        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"SELECT Cuentas.Id, Cuentas.Nombre, Balance, tc.Nombre AS TipoCuenta
                        FROM Cuentas
                        INNER JOIN TiposCuentas tc
                        ON tc.Id = Cuentas.TipoCuentaId
                        WHERE tc.UsuarioId = @UsuarioId
                        ORDER BY tc.Orden";
            return await connection.QueryAsync<Cuenta>(sql, new { usuarioId });

        }
    }
}
