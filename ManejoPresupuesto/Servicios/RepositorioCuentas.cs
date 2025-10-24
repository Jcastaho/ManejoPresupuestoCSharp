using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModel cuentaCreacionViewModel);
        Task Borrar(int id);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
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
        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, Cuentas.TipoCuentaId
                        FROM Cuentas
                        INNER JOIN TiposCuentas tc
                        ON tc.Id = Cuentas.TipoCuentaId
                        WHERE tc.UsuarioId = @UsuarioId AND Cuentas.Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(sql, new { id, usuarioId });
        }
        public async Task Actualizar(CuentaCreacionViewModel cuentaCreacionViewModel)
        {
            using var connection = new SqlConnection(connectionString);
            var sql = @"UPDATE Cuentas
                        SET Nombre = @Nombre,
                            Balance = @Balance,
                            Descripcion = @Descripcion,
                            TipoCuentaId = @TipoCuentaId
                        WHERE Id = @Id";
            await connection.ExecuteAsync(sql, cuentaCreacionViewModel);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Cuentas WHERE Id = @Id", new { id });
        }

    }
}
