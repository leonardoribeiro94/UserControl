using System;
using System.Linq;

namespace UserControl.DataAccessLayer
{
    public class UsuarioRepositorio
    {
        private static readonly UsuarioSqlServerDataContext DataContext = new UsuarioSqlServerDataContext();

        public static void InsertUsuario(Usuario usuario)
        {
            var usuariEncontrado = GetUsuarioByEmail(usuario.Email.ToLower());

            if (usuariEncontrado == null)
            {
                DataContext.Usuarios.InsertOnSubmit(usuario);
                DataContext.SubmitChanges();
            }
            else
            {
                throw new Exception($"E-mail {usuario.Email.ToUpper()} já existe no banco de dados");
            }
        }

        public static void UpdateUsuario(Usuario usuario)
        {
            var usuarioEncontrado = GetUsuarioPorId(usuario.Usuarios_Id);
            usuarioEncontrado.Nome = usuario.Nome.ToUpper();
            usuarioEncontrado.Email = usuario.Email.ToLower();
            usuarioEncontrado.Senha = usuario.Senha;
            DataContext.SubmitChanges();
        }

        public static void DeleteUsuario(int id)
        {
            //linq
            //return (from x in dataContext.GetTable<Usuario>() where x.Usuario_Id == id select x).SingleOrDefault();

            //linq com lambda

            var usuarioId = GetUsuarioPorId(id);

            if (usuarioId != null)
            {
                //deleta usuario
                DataContext.Usuarios.DeleteOnSubmit(usuarioId);
                DataContext.SubmitChanges();
            }
        }

        public static Usuario GetUsuarioPorId(int id)
        {
            //linq puro
            //return (from x in dataContext.GetTable<Usuario>() where x.Usuario_Id == id select x).SingleOrDefault();

            //linq com lambda
            return DataContext.GetTable<Usuario>().SingleOrDefault(x => x.Usuarios_Id == id);
        }

        public static Usuario GetUsuarioByEmail(string email)
        {
            return DataContext.GetTable<Usuario>().SingleOrDefault(x => x.Email == email);
        }

        public static IQueryable<Usuario> GetUsuario()
        {
            //pega todos os usuarios.
            return DataContext.GetTable<Usuario>().OrderBy(x => x.Usuarios_Id);
        }

        public static IQueryable<Usuario> GetUsuarioById(int id)
        {
            //pega todos os usuarios.
            return DataContext.GetTable<Usuario>().Where(x => x.Usuarios_Id == id);
        }

    }
}
