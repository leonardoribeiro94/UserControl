using System;
using System.Windows.Forms;
using UserControl.DataAccessLayer;

namespace UserControl.Usuarios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                CarregaUsuariosExistentes();
            }
            catch (Exception exception)
            {
                MensagemDeErro("Load", exception.Message);
            }
        }

        private void btnIncluirUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = txtNome.Text.ToUpper(),
                    Senha = txtSenha.Text.ToUpper(),
                    Email = txtEmail.Text.ToLower()
                };

                UsuarioRepositorio.InsertUsuario(usuario);
                MessageBox.Show($@"Usuario {txtNome.Text.ToUpper()} incluído com sucesso!");
                CarregaUsuariosExistentes();
            }
            catch (Exception exception)
            {
                MensagemDeErro("Incluir Usuário", exception.Message);
            }
        }

        private void btnAlterarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = new Usuario();
                usuario.Usuarios_Id = Convert.ToInt32(txtIdUsuario.Text);
                usuario.Nome = txtNome.Text;
                usuario.Email = txtEmail.Text;
                usuario.Senha = txtSenha.Text;

                UsuarioRepositorio.UpdateUsuario(usuario);
                MessageBox.Show($@"Usuario {usuario.Nome} Atualizado!", @"Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MensagemDeErro("Atualizar", exception.Message);
            }
        }

        private void btnExcluirUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($@"Deseja Excluir {txtNome.Text}?", @"Atenção!", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    var idUsuario = txtIdUsuario.Text;
                    UsuarioRepositorio.DeleteUsuario(Convert.ToInt32(idUsuario));
                    MessageBox.Show($@"Usuario Excluido com sucesso!", @"Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregaUsuariosExistentes();
                }

            }
            catch (Exception exception)
            {
                MensagemDeErro("Excluir", exception.Message);
            }
        }

        private void btnPesquisarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtIdUsuario.Text))
                {
                    var idUsuario = Convert.ToInt32(txtIdUsuario.Text);
                    dataGridView1.DataSource = UsuarioRepositorio.GetUsuarioById(idUsuario);
                }
                else
                {
                    CarregaUsuariosExistentes();
                }

            }
            catch (Exception exception)
            {

                MensagemDeErro("Pesquisar", exception.Message);
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                var id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                if (id > 0)
                {
                    var usuario = UsuarioRepositorio.GetUsuarioPorId(id);
                    txtNome.Text = usuario.Nome;
                    txtEmail.Text = usuario.Email;
                    txtSenha.Text = usuario.Senha;
                    txtIdUsuario.Text = usuario.Usuarios_Id.ToString();
                }

            }
            catch (Exception exception)
            {
                MensagemDeErro("Leitura do Grid", exception.Message);
            }

        }


        #region Metodos Auxiliares
        private void CarregaUsuariosExistentes()
        {
            dataGridView1.DataSource = UsuarioRepositorio.GetUsuario();

        }

        private static void MensagemDeErro(string metodoDoErro, string excessao)
        {
            MessageBox.Show($@"Error! {excessao}", metodoDoErro, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        #endregion


    }
}
